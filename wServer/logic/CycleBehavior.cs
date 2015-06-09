namespace wServer.logic
{
    public enum CycleStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public abstract class CycleBehavior : Behavior
    {
        public CycleStatus Status { get; protected set; }
    }
}