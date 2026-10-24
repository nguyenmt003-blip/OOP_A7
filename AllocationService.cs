/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class AllocationService<T> where T : TeachingDevice
{
    private readonly IRepository<T> _repository;
    private ISelectionStrategy<T> _strategy;

    public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        _strategy = newStrategy ?? throw new ArgumentNullException(nameof(newStrategy));
    }

    public T Allocate()
    {
        return Allocate(null);
    }

    public T Allocate(Predicate<T>? additionalCondition)
    {
        // 1. Lấy danh sách từ repository
        // 2. Lọc chỉ giữ các thiết bị có IsReadyForUse() == true
        // 3. Áp dụng thêm additionalCondition nếu có
        var candidates = _repository.FindAll(device =>
            device != null &&
            device.IsReadyForUse() &&
            (additionalCondition == null || additionalCondition(device)));

        // 4. Nếu danh sách ứng viên rỗng -> ném NoSuitableItemException
        if (candidates.Count == 0)
        {
            throw new NoSuitableItemException();
        }

        // 5. Dùng _strategy.Select(candidates) để chọn thiết bị tốt nhất
        var selected = _strategy.Select(candidates);

        // 6. Chuyển trạng thái thiết bị sang borrowed 
        selected.Borrow();

        return selected;
    }
}
