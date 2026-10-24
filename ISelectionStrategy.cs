/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/
public interface ISelectionStrategy<T>
{
    T Select(IReadOnlyList<T> candidates);
}
