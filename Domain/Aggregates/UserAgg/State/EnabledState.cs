using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Aggregates.UserAgg.State
{
    public class EnabledState : UserState
    {
        public EnabledState(User user)
        {
            User = user;
        }

        public override void Enable()
        {
            throw new PortalException(ErrorCodes.StringCodes.CannotChangeUserStateToEnabled,
                ErrorMessage.CannotChangeUserStateToEnabled);
        }

        public override void Disable()
        {
            User.SetState(new DisabledState(User));
        }
    }
}