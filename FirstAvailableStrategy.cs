/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
{
    public T Select(IReadOnlyList<T> candidates)
    {
        ArgumentNullException.ThrowIfNull(candidates);

        if (candidates.Count == 0)
        {
            throw new NoSuitableItemException();
        }

        return candidates[0];
    }
}
