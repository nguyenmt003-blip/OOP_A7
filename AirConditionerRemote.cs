/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class AirConditionerRemote : TeachingDevice
{
    public AirConditionerRemote(
        string id,
        string name,
        string storageLocation,
        IEnumerable<string> compatibleRooms)
        : base(id, name, storageLocation)
    {
        ArgumentNullException.ThrowIfNull(compatibleRooms);

        CompatibleRooms = new HashSet<string>(compatibleRooms, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlySet<string> CompatibleRooms { get; }

    public bool IsCompatibleWith(string roomId)
    {
        return CompatibleRooms.Contains(roomId);
    }

    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available;
    }

    public override double GetDefaultPriorityScore()
    {
        return CompatibleRooms.Count * 25.0;
    }
}
