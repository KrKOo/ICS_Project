using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Services.MessageDialog;
using CarPool.App.Wrappers;
using CarPool.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CarPool.App.Commands;
using CarPool.BL.Facades;

namespace CarPool.App.ViewModels
{
    public class UserEditViewModel : ViewModelBase, IUserEditViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _UserFacade;

        public UserEditViewModel(
            UserFacade UserFacade,
            IMediator mediator)
        {
            _UserFacade = UserFacade;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            mediator.Register<UserLoggedMessage>(UserLogged);

            RedirectToProfileScreenCommand = new RelayCommand(RedirectToProfileScreen);

            LoggedUser = UserDetailModel.Empty;
        }

        public UserWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand RedirectToProfileScreenCommand { get; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(Guid Id)
        {
            Model = await _UserFacade.GetAsync(Id) ?? UserDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _UserFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<UserWrapper> { Model = Model });
            RedirectToProfileScreen();
        }

        private bool CanSave() => Model?.IsValid ?? false;

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            _ = LoadAsync(userLoggedMessage.User.Id);
            LoggedUser = userLoggedMessage.User;
        }

        private void RedirectToProfileScreen()
        {
            if (LoggedUser == null) return;
            _mediator.Send(new SelectedMessage<UserListWrapper> { Id = LoggedUser.Id });
        }

    }
}