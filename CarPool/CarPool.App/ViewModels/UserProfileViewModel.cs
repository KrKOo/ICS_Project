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

        }

        public UserWrapper? Model { get; private set; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            Model = userLoggedMessage.User;
        }


    }
}
