/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

//bắt lỗi về trạng thái

public class DeviceNotReadyException : Exception
{
    public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus)
        : base($"Device '{deviceId}' is not ready for use. Current status: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }
}
