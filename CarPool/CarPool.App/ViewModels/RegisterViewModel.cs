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
using CarPool.BL.Facades;

namespace CarPool.App.ViewModels
{
    public class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _usersFacade;

        public RegisterViewModel(IMediator mediator, UserFacade userFacade)
        {
            RedirectToLoginScreenCommand = new RelayCommand(RedirectToLoginScreen);

            _mediator = mediator;
            _usersFacade = userFacade;
        }
        public ICommand RedirectToLoginScreenCommand { get; set; }

        public void RedirectToLoginScreen()
        {
            _mediator.Send(new RedirectToLoginScreenMessage());
        }
    }
}
