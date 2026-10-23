/*
Họ tên: Nguyễn Minh Tuấn
Mã sinh viên: 202419010
*/

public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
{
    private readonly Func<T, double> _scoreSelector;

    public HighestPriorityStrategy(Func<T, double> scoreSelector)
    {
        _scoreSelector = scoreSelector ?? throw new ArgumentNullException(nameof(scoreSelector));
    }

    public T Select(IReadOnlyList<T> candidates)
    {
        ArgumentNullException.ThrowIfNull(candidates);

        if (candidates.Count == 0)
        {
            throw new NoSuitableItemException();
        }

        var best = candidates[0];
        var bestScore = _scoreSelector(best);

        for (var i = 1; i < candidates.Count; i++)
        {
            var candidate = candidates[i];
            var score = _scoreSelector(candidate);

            if (score > bestScore)
            {
                best = candidate;
                bestScore = score;
            }
        }

        return best;
    }
}
