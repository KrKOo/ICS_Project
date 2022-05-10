using CarPool.BL.Models;

namespace CarPool.App.Wrappers
{
    public class UserListWrapper : ModelWrapper<UserListModel>
    {
        public UserListWrapper(UserListModel model) : base(model)
        {
        }

        public string? Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? PhotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public static implicit operator UserListWrapper(UserListModel listModel)
            => new(listModel);

        public static implicit operator UserListModel(UserListWrapper wrapper)
            => wrapper.Model;
    }
}