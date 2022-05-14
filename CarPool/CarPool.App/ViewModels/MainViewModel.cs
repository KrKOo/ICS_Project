using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarPool.App.Factories;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.ViewModels.Interfaces;
using CarPool.App.Wrappers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarPool.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            IRideListViewModel rideListViewModel,
            ICarListViewModel carListViewModel,
            IUserListViewModel userListViewModel,
            ILoginViewModel loginViewModel,
            IRegisterViewModel registerViewModel,
            IUserProfileViewModel profileViewModel,
            IRideDetailViewModel rideDetailViewModel,
            ICarDetailViewModel carDetailViewModel,
            IAddCarViewModel addCarViewModel,
            IEditCarViewModel editCarViewModel,
            IUserEditViewModel userEditViewModel,
            IMediator mediator)
        {

            RideListViewModel = rideListViewModel;
            RideDetailViewModel = rideDetailViewModel;

            CarListViewModel = carListViewModel;
            CarDetailViewModel = carDetailViewModel;
            AddCarViewModel = addCarViewModel;
            EditCarViewModel = editCarViewModel;

            UserListViewModel = userListViewModel;
            UserEditViewModel = userEditViewModel;

            LoginViewModel = loginViewModel;
            RegisterViewModel = registerViewModel;

            UserProfileViewModel = profileViewModel;

            mediator.Register<NewMessage<RideWrapper>>(OnRideNewMessage);
            mediator.Register<SelectedMessage<RideWrapper>>(OnRideSelected);
            //mediator.Register<DeleteMessage<RideWrapper>>(OnRideDeleted);

            mediator.Register<NewMessage<CarWrapper>>(OnCarNewMessage);
            mediator.Register<SelectedMessage<CarWrapper>>(OnCarSelected);
            //mediator.Register<DeleteMessage<CarWrapper>>(OnCarDeleted);

            mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
            mediator.Register<SelectedMessage<UserWrapper>>(OnUserSelected);
            //mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);

            mediator.Register<RedirectToRegisterScreenMessage>(OnRedirectToRegisterScreen);
            mediator.Register<RedirectToLoginScreenMessage>(OnRedirectToLoginScreen);
            mediator.Register<RedirectToRideListScreenMessage>(OnRedirectToRideListScreen);
            mediator.Register<RedirectToProfileScreenMessage>(OnRedirectToProfileScreen);
            mediator.Register<RedirectToAddCarScreenMessage>(OnRedirectToAddCarScreen);
            mediator.Register<RedirectToUserEditScreenMessage>(OnRedirectToUserEditScreen);

            //UserDetailViewModel.LoadAsync(Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"));
            //RideDetailViewModel.LoadAsync(Guid.Parse("0c3693ae-70bf-48a1-bfc4-7aa9bc42bbc4"));
            // CarDetailViewModel.LoadAsync(Guid.Parse("4ebd0208-8328-5d69-8c44-ec50939c0967"));
            //SelectCar(Guid.Empty);

            CurrentViewModel = (IViewModel) LoginViewModel;
        }

        public IViewModel _currentViewModel;
        public IViewModel CurrentViewModel { get; set; }

        public IRideListViewModel RideListViewModel { get; }
        public IRideDetailViewModel RideDetailViewModel { get; }

        public ICarListViewModel CarListViewModel { get; }
        public ICarDetailViewModel CarDetailViewModel { get; }
        public IAddCarViewModel AddCarViewModel { get; }
        public IEditCarViewModel EditCarViewModel { get; }

        public IUserListViewModel UserListViewModel { get; }
        public IUserEditViewModel UserEditViewModel { get; }
        public IUserProfileViewModel UserProfileViewModel { get; }

        public ILoginViewModel LoginViewModel { get; }
        public IRegisterViewModel RegisterViewModel { get; }
        

        public IRideDetailViewModel? SelectedRideDetailViewModel { get; set; }
        public ICarDetailViewModel? SelectedCarDetailViewModel { get; set; }
        public IUserProfileViewModel? SelectedProfileViewModel { get; set; }

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

        public void OnRedirectToRegisterScreen(RedirectToRegisterScreenMessage _)
        {
            CurrentViewModel = (IViewModel) RegisterViewModel;
        }

        public void OnRedirectToLoginScreen(RedirectToLoginScreenMessage _)
        {
            CurrentViewModel = (IViewModel) LoginViewModel;
        }
        public void OnRedirectToRideListScreen(RedirectToRideListScreenMessage _)
        {
            CurrentViewModel = (IViewModel) RideListViewModel;
        }

        public void OnRedirectToProfileScreen(RedirectToProfileScreenMessage _)
        {
            CurrentViewModel = (IViewModel)UserProfileViewModel;
        }

        public void OnRedirectToAddCarScreen(RedirectToAddCarScreenMessage _)
        {
            CurrentViewModel = (IViewModel)AddCarViewModel;
        }

        public void OnRedirectToUserEditScreen(RedirectToUserEditScreenMessage _)
        {
            CurrentViewModel = (IViewModel)UserEditViewModel;
        }

        private void SelectRide(Guid? id)
        {
            if (id is null)
            {
                SelectedRideDetailViewModel = null;
            }
            else
            {
                RideDetailViewModel.LoadAsync(id.Value);
                SelectedRideDetailViewModel = RideDetailViewModel;
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
                CarDetailViewModel.LoadAsync(id.Value);
                SelectedCarDetailViewModel = CarDetailViewModel;
            }
        }

        private void SelectUser(Guid? id)
        {
            if (id is null)
            {
                SelectedProfileViewModel = null;
            }
            else
            {
                UserProfileViewModel.LoadAsync(id.Value);
                SelectedProfileViewModel = UserProfileViewModel;
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
