namespace BillingSystem.Interfaces
{
    public interface IMissedCallStatistic: IStatistic
    {
        int SourceNumber { get; }
    }
}
