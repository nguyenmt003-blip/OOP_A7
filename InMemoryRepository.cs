/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
{
    private readonly Dictionary<string, T> _items = new(StringComparer.OrdinalIgnoreCase);

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(item.Id))
        {
            throw new ArgumentException("Item Id cannot be null or whitespace.", nameof(item));
        }

        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateIdException(item.Id);
        }

        _items.Add(item.Id, item);
    }

    public bool Remove(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        return _items.Remove(id);
    }

    public T? FindById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return default;
        }

        return _items.TryGetValue(id, out var item) ? item : default;
    }

    public IReadOnlyList<T> GetAll()
    {
        return _items.Values.ToList().AsReadOnly();
    }

    public IReadOnlyList<T> FindAll(Predicate<T> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);

        return _items.Values.Where(item => condition(item)).ToList().AsReadOnly();
    }
}
