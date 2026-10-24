/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class WirelessMicrophone : TeachingDevice
{
    public WirelessMicrophone(
        string id,
        string name,
        string storageLocation,
        int batteryLevel,
        int signalQuality)
        : base(id, name, storageLocation)
    {
        if (batteryLevel is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(batteryLevel), "Battery level must be between 0 and 100.");
        }

        if (signalQuality is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(signalQuality), "Signal quality must be between 0 and 100.");
        }

        BatteryLevel = batteryLevel;
        SignalQuality = signalQuality;
    }

    public int BatteryLevel { get; }

    public int SignalQuality { get; }

    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 30 && SignalQuality >= 40;
    }

    public override double GetDefaultPriorityScore()
    {
        return (BatteryLevel + SignalQuality) / 2.0;
    }
}
