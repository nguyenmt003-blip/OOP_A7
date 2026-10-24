/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/
//bắt lỗi lặp thiết bị mượn
public class DuplicateIdException : Exception
{
    public DuplicateIdException(string duplicateId)
        : base($"An item with the id '{duplicateId}' already exists.")
    {
        DuplicateId = duplicateId;
    }

    public string DuplicateId { get; }
}
