/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

using System.Threading.Tasks.Dataflow;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Kiểm thử  ===\n");

// Khởi tạo các repository
var projectorRepo = new InMemoryRepository<ProjectorRemote>();
var airConditionerRepo = new InMemoryRepository<AirConditionerRemote>();
var micRepo = new InMemoryRepository<WirelessMicrophone>();

// Tạo dữ liệu tối thiểu theo yêu cầu:
// - 3 điều khiển máy chiếu
var pr1 = new ProjectorRemote("PR01", "Điều khiển Sony", "Kho A1", 15); // pin < 20 -> Not ready
var pr2 = new ProjectorRemote("PR02", "Điều khiển Panasonic", "Kho A2", 85); // pin >= 20 -> Ready
var pr3 = new ProjectorRemote("PR03", "Điều khiển Epson", "Kho A3", 50); // pin >= 20 -> Ready

// - 2 điều khiển điều hòa
var ac1 = new AirConditionerRemote("AC01", "Điều khiển Daikin", "Kho B1", new[] { "A201", "A202", "A203" });
var ac2 = new AirConditionerRemote("AC02", "Điều khiển Panasonic AC", "Kho B2", new[] { "B101", "B102" });

// - 3 micro 
var mic1 = new WirelessMicrophone("MC01", "Micro Shure", "Kho C1", 25, 80); // pin < 30 -> Not ready
var mic2 = new WirelessMicrophone("MC02", "Micro Sennheiser", "Kho C2", 90, 85); // pin >= 30, signal >= 40 -> Ready
var mic3 = new WirelessMicrophone("MC03", "Micro AKG", "Kho C3", 70, 30); // signal < 40 -> Not ready

// 1. Thêm thiết bị vào repository
Console.WriteLine("--- 1. Thêm thiết bị vào repository ---");
projectorRepo.Add(pr1);
projectorRepo.Add(pr2);
projectorRepo.Add(pr3);
airConditionerRepo.Add(ac1);
airConditionerRepo.Add(ac2);
micRepo.Add(mic1);
micRepo.Add(mic2);
micRepo.Add(mic3);
Console.WriteLine($"Đã thêm: {projectorRepo.GetAll().Count} máy chiếu, {airConditionerRepo.GetAll().Count} điều hòa, {micRepo.GetAll().Count} micro.\n");

// 2. Thử thêm hai thiết bị trùng mã
Console.WriteLine("--- 2. Thử thêm hai thiết bị trùng mã: Tạo 1 cái PR01 nữa) ---");
try
{
    var duplicateProjector = new ProjectorRemote("PR01", "Trùng mã PR01", "Kho A", 99);
    projectorRepo.Add(duplicateProjector);
}
catch (DuplicateIdException ex)
{
    Console.WriteLine($"Ngoại lệ:{ex.Message} (DuplicateId = {ex.DuplicateId})\n");
}

// 3. Tìm thiết bị theo mã
Console.WriteLine("--- 3. Tìm thử mã PR02 (projector) ---");
var foundPr = projectorRepo.FindById("PR02");
Console.WriteLine($"Tìm thấy thiết bị PR02: {foundPr?.Name}, Vị trí: {foundPr?.StorageLocation}, Pin: {foundPr?.BatteryLevel}%\n");

// 4. Lọc các thiết bị sẵn sàng
Console.WriteLine("--- 4. Lọc điều khiển máy chiếu dùng đc (trên 20% pin và ko bị đang mượn) ---");
var readyProjectors = projectorRepo.FindAll(d => d.IsReadyForUse());
Console.WriteLine($"Số máy chiếu sẵn sàng (Pin >= 20%): {readyProjectors.Count}");
foreach (var p in readyProjectors)
{
    Console.WriteLine($" - [{p.Id}] {p.Name} (Pin: {p.BatteryLevel}%)");
}

var readyMics = micRepo.FindAll(d => d.IsReadyForUse());
Console.WriteLine($"Số micro sẵn sàng (Pin >= 30% và Tín hiệu >= 40%): {readyMics.Count}");
foreach (var m in readyMics)
{
    Console.WriteLine($" - [{m.Id}] {m.Name} (Pin: {m.BatteryLevel}%, Tín hiệu: {m.SignalQuality}%)");
}
Console.WriteLine();

// 5. Chọn điều khiển máy chiếu có điểm cao nhất
Console.WriteLine("--- 5. Cấp phát điều khiển máy chiếu có điểm ưu tiên cao nhất ---");
var highestStrategyPr = new HighestPriorityStrategy<ProjectorRemote>(device => device.GetDefaultPriorityScore());
var prService = new AllocationService<ProjectorRemote>(projectorRepo, highestStrategyPr);
var allocatedPr = prService.Allocate();
Console.WriteLine($"Đã cấp phát máy chiếu tốt nhất: [{allocatedPr.Id}] {allocatedPr.Name} (Pin: {allocatedPr.BatteryLevel}%), Trạng thái: {allocatedPr.Status}\n");

// 6. Chọn điều khiển điều hòa tương thích với phòng A203
Console.WriteLine("--- 6. Cấp phát điều khiển điều hòa tương thích với phòng A203 ---");
var acStrategy = new FirstAvailableStrategy<AirConditionerRemote>();
var acService = new AllocationService<AirConditionerRemote>(airConditionerRepo, acStrategy);
var allocatedAc = acService.Allocate(device => device.IsCompatibleWith("A203"));
Console.WriteLine($"Đã cấp phát điều khiển điều hòa: [{allocatedAc.Id}] {allocatedAc.Name}, Trạng thái: {allocatedAc.Status}\n");

// 7. Chọn micro đầu tiên sẵn sàng
Console.WriteLine("--- 7. Cấp phát micro đầu tiên sẵn sàng ---");
var micStrategy = new FirstAvailableStrategy<WirelessMicrophone>();
var micService = new AllocationService<WirelessMicrophone>(micRepo, micStrategy);
var allocatedMic = micService.Allocate();
Console.WriteLine($"Đã cấp phát micro: [{allocatedMic.Id}] {allocatedMic.Name} (Pin: {allocatedMic.BatteryLevel}%, Tín hiệu: {allocatedMic.SignalQuality}%), Trạng thái: {allocatedMic.Status}\n");

// 8. Đổi chiến lược trong thời gian chạy và quan sát kết quả
Console.WriteLine("--- 8. Đổi chiến lược trong thời gian chạy ---");
Console.WriteLine("Tạo repo mới gồm 2 máy chiếu để test");
var testRepo = new InMemoryRepository<ProjectorRemote>();
var itemA = new ProjectorRemote("P_A", "Máy chiếu A", "Kho 1", 30);
var itemB = new ProjectorRemote("P_B", "Máy chiếu B", "Kho 2", 90);
testRepo.Add(itemA);
testRepo.Add(itemB);

var testService = new AllocationService<ProjectorRemote>(testRepo, new FirstAvailableStrategy<ProjectorRemote>());
Console.WriteLine("Vừa gọi chiến lược này: FirstAvailableStrategy");
// Không gọi Allocate() ngay để không đổi trạng thái sang Borrowed mà kiểm tra việc đổi chiến lược
testService.ChangeStrategy(new HighestPriorityStrategy<ProjectorRemote>(p => p.BatteryLevel));
Console.WriteLine("Giờ đổi qua chiến lược: HighestPriorityStrategy");
var selectedHighest = testService.Allocate();
Console.WriteLine($"Kết quả chọn được: [{selectedHighest.Id}] {selectedHighest.Name} (Pin: {selectedHighest.BatteryLevel}%)\n");

// 9. Thử cấp phát khi không có ứng viên phù hợp
Console.WriteLine("--- 9. Thử cấp phát khi không có ứng viên phù hợp (NoSuitableItemException) ---");
try
{
    // Tìm điều khiển điều hòa cho phòng không tồn tại "Z999"
    Console.WriteLine("Tìm ở phòng Z999");
    acService.Allocate(d => d.IsCompatibleWith("Z999"));
}
catch (NoSuitableItemException ex)
{
    Console.WriteLine($"Ngoại lệ: {ex.Message}\n");
}

// 10. Chứng minh thiết bị được chọn chuyển sang Borrowed
Console.WriteLine("--- 10. Chứng minh thiết bị được chọn chuyển sang Borrowed ---");
Console.WriteLine($"Trạng thái của {allocatedPr.Id} sau khi cấp phát: {allocatedPr.Status}");
try
{
    // Thử mượn lại một thiết bị đã mượn (đang ở trạng thái Borrowed) -> ném DeviceNotReadyException
    Console.WriteLine("Thử mượn lại thiết bị đang ở Borrowed ");
    allocatedPr.Borrow();
}
catch (DeviceNotReadyException ex)
{
    Console.WriteLine($"Ngoại Lệ: {ex.Message}");
}
// Trả thiết bị về kho
allocatedPr.Return();
Console.WriteLine($"Sau khi gọi Return(), trạng thái của {allocatedPr.Id} là: {allocatedPr.Status}\n");

// 11. Chứng minh repository tổng quát có thể dùng cho ít nhất hai loại thiết bị mà không sao chép mã
Console.WriteLine("--- 11. Repository tổng quát IRepository<T> tái sử dụng an toàn kiểu ---");
IRepository<TeachingDevice> genericDeviceRepo = new InMemoryRepository<TeachingDevice>();
genericDeviceRepo.Add(new ProjectorRemote("GEN_PR", "Projector Gen", "K1", 80));
genericDeviceRepo.Add(new WirelessMicrophone("GEN_MIC", "Mic Gen", "K2", 90, 90));
Console.WriteLine($"genericDeviceRepo đang lưu {genericDeviceRepo.GetAll().Count} thiết bị thuộc các kiểu dẫn xuất khác nhau.");
foreach (var dev in genericDeviceRepo.GetAll())
{
    Console.WriteLine($" - [{dev.Id}] {dev.GetType().Name} - {dev.Name}");
}

