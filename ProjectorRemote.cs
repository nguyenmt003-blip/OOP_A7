/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class ProjectorRemote : TeachingDevice
{
    public ProjectorRemote(string id, string name, string storageLocation, int batteryLevel)
        : base(id, name, storageLocation)
    {
        if (batteryLevel is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(batteryLevel), "Battery level must be between 0 and 100.");
        }

        BatteryLevel = batteryLevel;
    }

    public int BatteryLevel { get; }

    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 20;
    }

    public override double GetDefaultPriorityScore()
    {
        return BatteryLevel;
    }
}
