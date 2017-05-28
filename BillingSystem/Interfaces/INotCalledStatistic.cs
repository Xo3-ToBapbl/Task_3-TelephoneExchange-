namespace BillingSystem.Interfaces
{
    public interface INotCalledStatistic: IStatistic
    {
        int TargetNumber { get; }
    }
}
