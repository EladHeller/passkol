using Model.Confirm;
using Model.Users;

namespace Services.Messaging.Requests
{
    public class ConfirmArtistRequest
    {
        public PaskolUser User { get; set; }
        public ConfirmTypeAction Action { get; set; }

        public ConfirmArtistRequest(ref PaskolUser user, ConfirmTypeAction action)
        {
            this.User = user;
            this.Action = action;
        }
    }
}
