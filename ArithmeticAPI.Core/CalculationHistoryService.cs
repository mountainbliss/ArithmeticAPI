namespace ArithmeticAPI.Core;

public class CalculationHistoryService
{
    private readonly List<CalculationResult> _history = new();

    public void AddToHistory(CalculationResult result)
    {
        _history.Add(result);
    }

    public IEnumerable<CalculationResult> GetAll()
    {
        return _history;
    }

    public IEnumerable<CalculationResult> GetTopTwoResults()
    {
        return _history.OrderByDescending(c => c.Result).Take(2);
    }

    public IEnumerable<CalculationResult> GetByOperation(string operation)
    {
        return _history.Where(c => c.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase));
    }
}