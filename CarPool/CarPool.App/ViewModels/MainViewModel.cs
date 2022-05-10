using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarPool.App.Factories;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;

namespace CarPool.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFactory<IRideDetailViewModel> _rideDetailViewModelFactory;
        private readonly IFactory<ICarDetailViewModel> _carDetailViewModelFactory;
        private readonly IFactory<IUserDetailViewModel> _userDetailViewModelFactory;

        public MainViewModel(

            IRideListViewModel rideListViewModel,
            ICarListViewModel carListViewModel,
            IUserListViewModel userListViewModel,
            IMediator mediator,
            IFactory<IRideDetailViewModel> rideDetailViewModelFactory,
            IFactory<ICarDetailViewModel> carDetailViewModelFactory,
            IFactory<IUserDetailViewModel> userDetailViewModelFactory)
        {
            _rideDetailViewModelFactory = rideDetailViewModelFactory;
            _carDetailViewModelFactory = carDetailViewModelFactory;
            _userDetailViewModelFactory = userDetailViewModelFactory;

            RideListViewModel = rideListViewModel;
            RideDetailViewModel = rideDetailViewModelFactory.Create();

            CarListViewModel = carListViewModel;
            CarDetailViewModel = carDetailViewModelFactory.Create();

            UserListViewModel = userListViewModel;
            UserDetailViewModel = userDetailViewModelFactory.Create();

            mediator.Register<NewMessage<RideWrapper>>(OnRideNewMessage);
            mediator.Register<SelectedMessage<RideWrapper>>(OnRideSelected);
            //mediator.Register<DeleteMessage<RideWrapper>>(OnRideDeleted);

            mediator.Register<NewMessage<CarWrapper>>(OnCarNewMessage);
            mediator.Register<SelectedMessage<CarWrapper>>(OnCarSelected);
            //mediator.Register<DeleteMessage<CarWrapper>>(OnCarDeleted);

            mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            //mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);



            //UserDetailViewModel.LoadAsync(Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"));
            //RideDetailViewModel.LoadAsync(Guid.Parse("0c3693ae-70bf-48a1-bfc4-7aa9bc42bbc4"));
            CarDetailViewModel.LoadAsync(Guid.Parse("4ebd0208-8328-5d69-8c44-ec50939c0967"));
            //SelectCar(Guid.Empty);
        }

        public IRideListViewModel RideListViewModel { get; }
        public IRideDetailViewModel RideDetailViewModel { get; }

        public ICarListViewModel CarListViewModel { get; }
        public ICarDetailViewModel CarDetailViewModel { get; }

        public IUserListViewModel UserListViewModel { get; }
        public IUserDetailViewModel UserDetailViewModel { get; }

        public IRideDetailViewModel? SelectedRideDetailViewModel { get; set; }
        public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }
        public IUserDetailViewModel? SelectedUserDetailViewModel { get; set; }

        public void OnRideNewMessage(NewMessage<RideWrapper> _) {
            SelectRide(Guid.Empty);
        }

        public void OnCarNewMessage(NewMessage<CarWrapper> _)
        {
            SelectCar(Guid.Empty);
        }

        public void OnUserNewMessage(NewMessage<UserWrapper> _)
        {
            SelectUser(Guid.Empty);
        }

        private void SelectRide(Guid? id)
        {
            if (id is null)
            {
                SelectedRideDetailViewModel = null;
            }
            else
            {
                var rideDetailViewModel = _rideDetailViewModelFactory.Create();
                rideDetailViewModel.LoadAsync(id.Value);
                SelectedRideDetailViewModel = rideDetailViewModel;
            }
        }

        private void SelectCar(Guid? id)
        {
            if (id is null)
            {
                SelectedCarDetailViewModel = null;
            }
            else
            {
                var carDetailViewModel = _carDetailViewModelFactory.Create();
                carDetailViewModel.LoadAsync(id.Value);
                SelectedCarDetailViewModel = carDetailViewModel;
            }
        }

        private void SelectUser(Guid? id)
        {
            if (id is null)
            {
                SelectedUserDetailViewModel = null;
            }
            else
            {
                var userDetailViewModel = _userDetailViewModelFactory.Create();    
                userDetailViewModel.LoadAsync(id.Value);
                SelectedUserDetailViewModel = userDetailViewModel;
            }
        }

        private void OnRideSelected(SelectedMessage<RideWrapper> message)
        {
            SelectRide(message.Id);
        }

        private void OnCarSelected(SelectedMessage<CarWrapper> message)
        {
            SelectRide(message.Id);
        }

        private void OnUserSelected(SelectedMessage<UserWrapper> message)
        {
            SelectRide(message.Id);
        }

    }
}
