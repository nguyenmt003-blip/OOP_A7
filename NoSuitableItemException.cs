/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class NoSuitableItemException : Exception
{
    public NoSuitableItemException()
        : base("No suitable item was found.")
    {
    }

    public NoSuitableItemException(string message)
        : base(message)
    {
    }

    public NoSuitableItemException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
