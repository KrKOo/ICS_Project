using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public class UserLoggedMessage : IMessage
    {
        public UserDetailModel? User { get; set; }
    }
}
