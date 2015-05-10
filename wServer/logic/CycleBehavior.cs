namespace wServer.logic
{
    internal enum CycleStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    internal abstract class CycleBehavior : Behavior
    {
        public CycleStatus Status { get; protected set; }
    }
}