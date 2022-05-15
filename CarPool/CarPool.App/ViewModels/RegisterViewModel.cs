using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarPool.App.Commands;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.ViewModels.Interfaces;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;

namespace CarPool.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _UserFacade;

        public RegisterViewModel(IMediator mediator, UserFacade userFacade)
        {
            RedirectToLoginScreenCommand = new RelayCommand(RedirectToLoginScreen);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

            _mediator = mediator;
            _UserFacade = userFacade;

            Model = UserDetailModel.Empty;
        }

        public UserWrapper? Model { get; private set; }
        public ICommand RedirectToLoginScreenCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public Task LoadAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model.Id = Guid.Empty;
            Model = await _UserFacade.SaveAsync(Model.Model);
            _mediator.Send(new UserLoggedMessage { User = Model });
            _mediator.Send(new RedirectToRideListScreenMessage());
            Model = UserDetailModel.Empty;
        }

        private bool CanSave() => Model?.IsValid ?? false;

        public void RedirectToLoginScreen()
        {
            _mediator.Send(new RedirectToLoginScreenMessage());
        }

    }
}
