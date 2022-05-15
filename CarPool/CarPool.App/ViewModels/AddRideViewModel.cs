using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CarPool.App.ViewModels;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.App.Services;
using CarPool.App.Commands;
using CarPool.App.Messages;
using CarPool.BL.Models;

namespace CarPool.App.ViewModels
{
    public class AddRideViewModel : ViewModelBase, IAddRideViewModel
    {
        private readonly IMediator _mediator;
        private readonly RideFacade _RideFacade;
        public AddRideViewModel(
            RideFacade RideFacade,
            IMediator mediator)
        {
            _RideFacade = RideFacade;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

            mediator.Register<UserLoggedMessage>(UserLogged);
            CarSelectedCommand = new RelayCommand<CarListWrapper>(CarSelected);

            LoggedUser = UserDetailModel.Empty;
            RedirectToRideListScreenCommand = new RelayCommand(RedirectToRideListScreen);
            ClearModel();
        }

        public RideWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }
        public CarListWrapper? Car { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand CarSelectedCommand { get; }
        public ICommand RedirectToRideListScreenCommand { get; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            if (Model == null || Model.Model.Driver == null || LoggedUser == null || Model.Model.Car == null || Car == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model.Model.Driver.Id = LoggedUser.Id;
            
            Model.Model.Car.Id = Car.Id;
            Model = await _RideFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<RideWrapper> { Model = Model });
            _mediator.Send(new UpdateMessage<UserWrapper> { Model = LoggedUser });

            ClearModel();
            RedirectToRideListScreen();
        }

        private bool CanSave() => Model?.IsValid ?? false;

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            LoggedUser = userLoggedMessage.User;
        }

        public Task LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private void RedirectToRideListScreen()
        {
            _mediator.Send(new RedirectToRideListScreenMessage());
        }
        private void CarSelected(CarListWrapper? CarListWrapper)
        {
            Car = CarListWrapper;
        }

        private void ClearModel() {
            Model = RideDetailModel.Empty with { Driver = UserListModel.Empty, Car = CarListModel.Empty };
            if (LoggedUser!.Cars.Count > 0)
            {
                Car = LoggedUser.Cars[0];
            }
            else {
                Car = null;
            }
        }

    }
}