using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CarPool.App.Commands;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Services.MessageDialog;
using CarPool.App.ViewModels.Interfaces;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;

namespace CarPool.App.ViewModels
{
    public class UserProfileViewModel : ViewModelBase, IUserProfileViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _UserFacade;
        private readonly IMessageDialogService _messageDialogService;

        public UserProfileViewModel(
            UserFacade userFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _UserFacade = userFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;
            Model = UserDetailModel.Empty;
            mediator.Register<UserLoggedMessage>(UserLogged);
            mediator.Register<UpdateMessage<UserWrapper>>(UserUpdated);

            RedirectToRideListCommand = new RelayCommand(RedirectToRideListScreen);
            RedirectToUserEditCommand = new RelayCommand(RedirectToUserEditScreen);
            RedirectToAddCarScreenCommand = new RelayCommand(RedirectToAddCarScreen);
        }
        public ICommand RedirectToRideListCommand { get; set; }
        public ICommand RedirectToUserEditCommand { get; set; }
        public ICommand RedirectToAddCarScreenCommand { get; set; }
        public UserWrapper? Model { get; private set; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _UserFacade.GetAsync(id) ?? UserDetailModel.Empty;
        }

        private async void UserUpdated(UpdateMessage<UserWrapper> obj) => await LoadAsync(Model?.Id ?? Guid.Empty);

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            _ = LoadAsync(userLoggedMessage.User.Id);
        }
        public void RedirectToRideListScreen()
        {
            _mediator.Send(new RedirectToRideListScreenMessage());
        }
        public void RedirectToAddCarScreen()
        {
            _mediator.Send(new RedirectToAddCarScreenMessage());
        }

        public void RedirectToUserEditScreen()
        {
            _mediator.Send(new RedirectToUserEditScreenMessage());
        }

    }
}
