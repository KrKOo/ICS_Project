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
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private readonly IMediator _mediator;
        private readonly UserFacade _usersFacade;

        public LoginViewModel(UserFacade userFacade, IMediator mediator)
        {
            RedirectToRegisterScreenCommand = new RelayCommand(RedirectToRegisterScreen);
            TryToLoginCommand = new RelayCommand(TryToLogin);

            _usersFacade = userFacade;
            _mediator = mediator;
            User = UserDetailModel.Empty;
        }

        public ICommand RedirectToRegisterScreenCommand { get; set; }
        public ICommand TryToLoginCommand { get; set; }

        public UserWrapper? User { get; private set; }

        public async void TryToLogin()
        {
            if (string.IsNullOrEmpty(User?.Email)) return;

            var userFromDb = await _usersFacade.GetUserByEmailAsync(User.Email);

            if (userFromDb == null)
            {
                return;
            }
            
            _mediator.Send(new RedirectToRideListScreenMessage());
            _mediator.Send(new UserLoggedMessage { User = userFromDb });
        }

        public void RedirectToRegisterScreen()
        {
            _mediator.Send(new RedirectToRegisterScreenMessage());
        }
    }

}