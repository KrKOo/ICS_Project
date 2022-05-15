using System;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.ViewModels.Interfaces;
using CarPool.App.Wrappers;

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
            IAddRideViewModel addRideViewModel,
            ICarDetailViewModel carDetailViewModel,
            IAddCarViewModel addCarViewModel,
            IEditCarViewModel editCarViewModel,
            IUserEditViewModel userEditViewModel,
            IMediator mediator)
        {
            RideListViewModel = rideListViewModel;
            RideDetailViewModel = rideDetailViewModel;
            AddRideViewModel = addRideViewModel;

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
            mediator.Register<SelectedMessage<RideListWrapper>>(OnRideSelected);
            mediator.Register<DeleteMessage<RideWrapper>>(OnRideDeleted);

            mediator.Register<NewMessage<CarWrapper>>(OnCarNewMessage);

            mediator.Register<SelectedMessage<CarListWrapper>>(OnCarSelected);
            //mediator.Register<DeleteMessage<CarWrapper>>(OnCarDeleted);

            mediator.Register<NewMessage<UserWrapper>>(OnUserNewMessage);
            mediator.Register<SelectedMessage<UserListWrapper>>(OnUserSelected);
            //mediator.Register<DeleteMessage<UserWrapper>>(OnUserDeleted);

            mediator.Register<RedirectToRegisterScreenMessage>(OnRedirectToRegisterScreen);
            mediator.Register<RedirectToLoginScreenMessage>(OnRedirectToLoginScreen);
            mediator.Register<RedirectToRideListScreenMessage>(OnRedirectToRideListScreen);
            mediator.Register<RedirectToAddCarScreenMessage>(OnRedirectToAddCarScreen);
            mediator.Register<RedirectToUserEditScreenMessage>(OnRedirectToUserEditScreen);
            mediator.Register<RedirectToAddRideScreenMessage>(OnRedirectToAddRideScreen);

            mediator.Register<EditMessage<CarWrapper>>(OnCarEdit);


            //UserDetailViewModel.LoadAsync(Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"));
            //RideDetailViewModel.LoadAsync(Guid.Parse("0c3693ae-70bf-48a1-bfc4-7aa9bc42bbc4"));
            // CarDetailViewModel.LoadAsync(Guid.Parse("4ebd0208-8328-5d69-8c44-ec50939c0967"));
            //SelectCar(Guid.Empty);

            CurrentViewModel = LoginViewModel;
        }

        public IViewModel CurrentViewModel { get; set; }

        public IRideListViewModel RideListViewModel { get; }
        public IRideDetailViewModel RideDetailViewModel { get; }
        public IAddRideViewModel AddRideViewModel { get; }

        public ICarListViewModel CarListViewModel { get; }
        public ICarDetailViewModel CarDetailViewModel { get; }
        public IAddCarViewModel AddCarViewModel { get; }
        public IEditCarViewModel EditCarViewModel { get; }

        public IUserListViewModel UserListViewModel { get; }
        public IUserEditViewModel UserEditViewModel { get; }
        public IUserProfileViewModel UserProfileViewModel { get; }

        public ILoginViewModel LoginViewModel { get; }
        public IRegisterViewModel RegisterViewModel { get; }

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
            CurrentViewModel = RegisterViewModel;
        }

        public void OnRedirectToLoginScreen(RedirectToLoginScreenMessage _)
        {
            CurrentViewModel = LoginViewModel;
        }
        public void OnRedirectToRideListScreen(RedirectToRideListScreenMessage _)
        {
            CurrentViewModel = RideListViewModel;
        }

        public void OnRedirectToAddCarScreen(RedirectToAddCarScreenMessage _)
        {
            CurrentViewModel = AddCarViewModel;
        }

        public void OnRedirectToUserEditScreen(RedirectToUserEditScreenMessage _)
        {
            CurrentViewModel = UserEditViewModel;
        }

        public void OnRedirectToCarEditScreen(RedirectToUserEditScreenMessage _)
        {
            CurrentViewModel = UserEditViewModel;
        }

        public void OnRedirectToAddRideScreen(RedirectToAddRideScreenMessage _)
        {
            CurrentViewModel = AddRideViewModel;
        }


        public void OnCarEdit(EditMessage<CarWrapper> message)
        {
            if (message.Id == null) return;

            EditCarViewModel.LoadAsync((Guid)message.Id);
            CurrentViewModel = EditCarViewModel;
        }

        private void SelectRide(Guid? id)
        {
            if (id == null) return;
            RideDetailViewModel.LoadAsync(id.Value);
           
            CurrentViewModel = RideDetailViewModel;
        }

        private void SelectCar(Guid? id)
        {
            if (id is null) return;

            CarDetailViewModel.LoadAsync(id.Value);
            CurrentViewModel = CarDetailViewModel;
        }

        private void SelectUser(Guid? id)
        {
            if (id is null) return;
            UserProfileViewModel.LoadAsync(id.Value);
            CurrentViewModel = UserProfileViewModel;
        }

        private void OnRideSelected(SelectedMessage<RideListWrapper> message)
        {
            SelectRide(message.Id);
        }

        private void OnCarSelected(SelectedMessage<CarListWrapper> message)
        {
            SelectCar(message.Id);
        }

        private void OnUserSelected(SelectedMessage<UserListWrapper> message)
        {
            SelectUser(message.Id);
        }

        private void OnRideDeleted(DeleteMessage<RideWrapper> _)
        {
            RideListViewModel.LoadAsync();
        }

    }
}
