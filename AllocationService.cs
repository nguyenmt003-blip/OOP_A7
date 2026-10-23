/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class AllocationService<T> where T : class, IIdentifiable
{
    private readonly IRepository<T> _repository;
    private ISelectionStrategy<T> _strategy;

    public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        _strategy = newStrategy ?? throw new ArgumentNullException(nameof(newStrategy));
    }

    public T Allocate(Predicate<T> additionalCondition)
    {
        ArgumentNullException.ThrowIfNull(additionalCondition);

        var candidates = _repository.FindAll(additionalCondition);

        if (candidates.Count == 0)
        {
            throw new NoSuitableItemException();
        }

        return _strategy.Select(candidates);
    }
}
