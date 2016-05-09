using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Aggregates.UserAgg.State
{
    public class DisabledState : UserState
    {
        public DisabledState(User user)
        {
            User = user;
        }

        public override void Enable()
        {
            User.SetState(new EnabledState(User));


        }

        public override void Disable()
        {
            throw new PortalException(ErrorCodes.StringCodes.CannotChangeUserStateToDisabled,
                ErrorMessage.CannotChangeUserStateToDisabled);
        }
    }
}