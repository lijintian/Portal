namespace Portal.Domain.Aggregates.UserAgg.State
{
    public abstract class UserState
    {
        public User User { get; protected set; }
        public abstract void Disable();
        public abstract void Enable();
    }
}