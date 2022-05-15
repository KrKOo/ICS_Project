using CarPool.App.Commands;
using CarPool.App.Extensions;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarPool.App.ViewModels
{
    public class RideListViewModel : ViewModelBase, IRideListViewModel
    {
        private readonly RideFacade _rideFacade;
        private readonly IMediator _mediator;
        public RideListViewModel(RideFacade rideFacade, IMediator mediator) {
            _rideFacade = rideFacade;
            _mediator = mediator;

            RedirectToProfileScreenCommand = new RelayCommand(RedirectToProfileScreen);
            RedirectToAddRideScreenCommand = new RelayCommand(RedirectToAddRideScreen);
            RideNewCommand = new RelayCommand(RideNew);
            ReloadRidesCommand = new RelayCommand(ReloadRides);
            RideSelectedCommand = new RelayCommand<RideListModel>(RideSelected);
            LogOutCommand = new RelayCommand(LogOut);

            mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
            mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
            mediator.Register<UserLoggedMessage>(UserLogged);

            Origin = string.Empty;
            Destination = string.Empty;
            LoggedUser = UserDetailModel.Empty;
        }

        public ObservableCollection<RideListModel> Rides { get; } = new();
        public UserWrapper? LoggedUser { get; private set; }

        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateOnly RideDate { get; set; }

        public ICommand RideNewCommand { get; }

        public ICommand RideSelectedCommand { get; }

        public ICommand RedirectToProfileScreenCommand { get; set; }
        public ICommand RedirectToAddRideScreenCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        public ICommand ReloadRidesCommand { get; set; }

        private void RideSelected(RideListModel? rideListWrapper)
        {
            if (rideListWrapper is not null) {
                _mediator.Send(new SelectedMessage<RideListWrapper> { Id = rideListWrapper.Id });
            }
        }

        private void RideNew()
        {
            _mediator.Send(new NewMessage<RideWrapper>());
        }

        private async void RideDeleted(DeleteMessage<RideWrapper> obj) => await LoadAsync();

        private async void RideUpdated(UpdateMessage<RideWrapper> obj) => await LoadAsync();
        public async Task LoadAsync()
        {
            Rides.Clear();
            var rides = await _rideFacade.GetRideByFilterAsync(Origin ?? "", Destination ?? "", RideDate);
            Rides.AddRange(rides);
        }

        public void RedirectToProfileScreen()
        {
            if (LoggedUser == null) return;
            _mediator.Send(new SelectedMessage<UserListWrapper> { Id = LoggedUser.Id });
        }

        public void RedirectToAddRideScreen()
        {
            _mediator.Send(new RedirectToAddRideScreenMessage());
        }

        public void ReloadRides()
        {
            _ = LoadAsync();
        }
        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            LoggedUser = userLoggedMessage.User;
        }

        public void LogOut()
        {
            _mediator.Send(new RedirectToLoginScreenMessage());
        }
    }
}
