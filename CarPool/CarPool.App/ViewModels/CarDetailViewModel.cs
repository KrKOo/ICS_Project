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
    public class CarDetailViewModel : ViewModelBase, ICarDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly CarFacade _CarFacade;

        public CarDetailViewModel(
            CarFacade CarFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _CarFacade = CarFacade;
            _mediator = mediator;

            RedirectToRideListCommand = new RelayCommand(RedirectToRideListScreen);
            RedirectToCarEditScreenCommand = new RelayCommand(RedirectToCarEditScreen);
            mediator.Register<UserLoggedMessage>(UserLogged);

            LoggedUser = UserDetailModel.Empty;
            Model = CarDetailModel.Empty with { Owner = UserListModel.Empty };
        }

        public CarWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }
        public bool IsLoggedUser { get; private set;}

        public ICommand RedirectToRideListCommand { get; set; }
        public ICommand RedirectToCarEditScreenCommand { get; set; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _CarFacade.GetAsync(id) ?? CarDetailModel.Empty;
            IsLoggedUser = Model.Owner?.Id == LoggedUser?.Id;
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void RedirectToRideListScreen()
        {
            _mediator.Send(new RedirectToRideListScreenMessage());
        }

        public void RedirectToCarEditScreen()
        {
            if (Model == null) return;
            _mediator.Send(new EditMessage<CarWrapper> { Id = Model.Id });
        }

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            LoggedUser = userLoggedMessage.User;
        }
    }
}