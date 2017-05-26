namespace ATE.Interfaces
{
    public interface ICallingEventArgs
    {
        int SourceNumber { get; }
        int TargetNumber { get; }
    }
}
