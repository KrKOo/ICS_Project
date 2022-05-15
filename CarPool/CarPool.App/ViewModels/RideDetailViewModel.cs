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
using System.Linq;

namespace CarPool.App.ViewModels
{
    public class RideDetailViewModel : ViewModelBase, IRideDetailViewModel
    {
        private readonly IMediator _mediator;
        private readonly RideFacade _RideFacade;
        private readonly IMessageDialogService _messageDialogService;

        public RideDetailViewModel(
            RideFacade RideFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _RideFacade = RideFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            JoinCommand = new AsyncRelayCommand(JoinAsync, CanJoin);
            LeaveCommand = new AsyncRelayCommand(LeaveAsync);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);

            RedirectToRideListCommand = new RelayCommand(RedirectToRideListScreen);
            RedirectToCarDetailScreenCommand = new RelayCommand(RedirectToCarDetailScreen);
            RedirectToDriverDetailScreenCommand = new RelayCommand(RedirectToDriverDetailScreen);
            PassengerShowCommand = new RelayCommand<UserListWrapper>(PassengerShow);
            PassengerDeleteCommand = new AsyncRelayCommand<UserListWrapper>(PassengerDelete);

            mediator.Register<UserLoggedMessage>(UserLogged);

            LoggedUser = UserDetailModel.Empty;
        }

        public RideWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }
        public bool IsJoinButtonVisible { get; private set; }
        public bool IsLeaveButtonVisible { get; private set; }
        public bool IsDeleteButtonVisible { get; private set; }

        public ICommand JoinCommand { get; }
        public ICommand LeaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RedirectToRideListCommand { get; set; }
        public ICommand RedirectToCarDetailScreenCommand { get; set; }
        public ICommand RedirectToDriverDetailScreenCommand { get; set; }
        public ICommand PassengerShowCommand { get; set; }
        public ICommand PassengerDeleteCommand { get; set; }

        public async Task LoadAsync(Guid id)
        {
            Model = await _RideFacade.GetAsync(id) ?? RideDetailModel.Empty;
            SetVisibility();
        }

        private void SetVisibility() {
            IsDeleteButtonVisible = false;

            if (GetIsLoggedUserDriver())
            {
                IsDeleteButtonVisible = true;
                IsJoinButtonVisible = false;
                IsLeaveButtonVisible = false;
                return;
            }

            if (GetIsLoggedUserAPassenger())
            {
                IsLeaveButtonVisible = true;
                IsJoinButtonVisible = false;
            }
            else if (!CanJoin())
            {
                IsLeaveButtonVisible = false;
                IsJoinButtonVisible = false;
            }
            else
            {
                IsLeaveButtonVisible = false;
                IsJoinButtonVisible = true;
            }
        }

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            LoggedUser = userLoggedMessage.User;
        }

        public async Task DeleteAsync()
        {
            if (Model is null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            if (Model.Id != Guid.Empty)
            {
                var delete = _messageDialogService.Show(
                    $"Delete",
                    $"Do you want to delete ride from {Model?.RideOrigin} to {Model?.RideDestination}?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No) return;

                try
                {
                    await _RideFacade.DeleteAsync(Model!.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting ride from {Model?.RideOrigin} to {Model?.RideDestination} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<RideWrapper>
                {
                    Model = Model
                });

                RedirectToRideListScreen();
            }
        }

        public bool GetIsLoggedUserAPassenger() {
            if (Model?.Passengers == null || LoggedUser == null) return false;

            var matches = Model.Passengers.Where(passenger => passenger.Id == LoggedUser.Id);
            return matches.Any();
        }

        public bool GetIsLoggedUserDriver() {
            if (Model?.Driver == null || LoggedUser == null) return false;

            return Model.Driver.Id == LoggedUser.Id;
        }

        public async Task JoinAsync()
        {
            if (Model == null || Model.Model.Passengers == null || LoggedUser == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model.Model.Passengers.Add(UserListModel.Empty with { Id = LoggedUser.Id });
            Model = await _RideFacade.SaveAsync(Model.Model);

            _mediator.Send(new UpdateMessage<UserWrapper> { Model = LoggedUser });
            await LoadAsync(Model.Id);
        }

        public async Task LeaveAsync()
        {
            if (Model == null || Model.Model.Passengers == null || LoggedUser == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            var leave = _messageDialogService.Show(
                    $"Leave",
                    $"Do you want to leave this ride?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

            if (leave == MessageDialogResult.No) return;

            Model.Model.Passengers.RemoveAll(passenger => passenger.Id == LoggedUser.Id);
            Model = await _RideFacade.SaveAsync(Model.Model);

            _mediator.Send(new UpdateMessage<UserWrapper> { Model = LoggedUser });
            await LoadAsync(Model.Id);
        }

        private bool CanJoin() {
            if (Model?.Car == null) return false;
            if (GetIsLoggedUserAPassenger()) return false;

            int passengerCount = Model.Passengers.Count();
            if (Model.Car.NumberOfSeats > passengerCount + 1)
            {
                return true;
            }

            return false;
        }

        public void RedirectToRideListScreen()
        {
            _mediator.Send(new RedirectToRideListScreenMessage());
        }

        private void RedirectToCarDetailScreen()
        {
            if (Model?.Car is not null)
            {
                _mediator.Send(new SelectedMessage<CarListWrapper> { Id = Model.Car.Id });
            }
        }

        private void RedirectToDriverDetailScreen()
        {
            if (Model?.Driver is not null)
            {
                _mediator.Send(new SelectedMessage<UserListWrapper> { Id = Model.Driver.Id });
            }
        }

        private void PassengerShow(UserListWrapper? UserListWrapper)
        {
            if (UserListWrapper is not null)
            {
                _mediator.Send(new SelectedMessage<UserListWrapper> { Id = UserListWrapper.Id });
            }
        }

        private async Task PassengerDelete(UserListWrapper? UserListWrapper)
        {
            if (Model == null || Model.Model.Passengers == null || UserListWrapper == null) return;

            Model.Model.Passengers.RemoveAll(passenger => passenger.Id == UserListWrapper.Id);
            Model = await _RideFacade.SaveAsync(Model.Model);

            _mediator.Send(new UpdateMessage<UserWrapper> { Model = LoggedUser });
            await LoadAsync(Model.Id);
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}