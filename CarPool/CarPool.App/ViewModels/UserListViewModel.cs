using CarPool.App.Commands;
using CarPool.App.Extensions;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarPool.App.ViewModels
{
    public class UserListViewModel : ViewModelBase, IUserListViewModel
    {
        private readonly UserFacade _UserFacade;
        private readonly IMediator _mediator;
        public UserListViewModel(UserFacade UserFacade, IMediator mediator)
        {
            _UserFacade = UserFacade;
            _mediator = mediator;

            UserSelectedCommand = new RelayCommand<UserListModel>(UserSelected);
            UserNewCommand = new RelayCommand(UserNew);

            mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);
            mediator.Register<DeleteMessage<UserWrapper>>(UserDeleted);
        }
        public ObservableCollection<UserListModel> Users { get; } = new();

        public ICommand UserNewCommand { get; }

        public ICommand UserSelectedCommand { get; }

        private void UserSelected(UserListModel? UserListModel)
        {
            if (UserListModel is not null)
            {
                _mediator.Send(new SelectedMessage<UserWrapper> { Id = UserListModel.Id });
            }
        }

        private void UserNew()
        {
            _mediator.Send(new NewMessage<UserWrapper>());
        }

        private async void UserDeleted(DeleteMessage<UserWrapper> obj) => await LoadAsync();

        private async void UserUpdated(UpdateMessage<UserWrapper> obj) => await LoadAsync();
        public async Task LoadAsync()
        {
            Users.Clear();
            var sers = await _UserFacade.GetAsync();
            Users.AddRange(sers);
        }
    }
}
