/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public enum DeviceStatus
{
    Available,
    Borrowed,
    UnderMaintenance
}

public abstract class TeachingDevice : IIdentifiable
{
    protected TeachingDevice(string id, string name, string storageLocation)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id must not be null or whitespace.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must not be null or whitespace.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(storageLocation))
        {
            throw new ArgumentException("Storage location must not be null or whitespace.", nameof(storageLocation));
        }

        Id = id.Trim();
        Name = name.Trim();
        StorageLocation = storageLocation.Trim();
        Status = DeviceStatus.Available;
    }

    public string Id { get; }

    public string Name { get; }

    public string StorageLocation { get; }

    public DeviceStatus Status { get; protected set; }

    public abstract bool IsReadyForUse();

    public abstract double GetDefaultPriorityScore();

    public void Borrow()
    {
        if (!IsReadyForUse())
        {
            throw new DeviceNotReadyException(Id, Status);
        }

        Status = DeviceStatus.Borrowed;
    }

    public void Return()
    {
        if (Status != DeviceStatus.Borrowed)
        {
            throw new InvalidOperationException($"Device '{Id}' cannot be returned because its current status is {Status}.");
        }

        Status = DeviceStatus.Available;
    }
}
