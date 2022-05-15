using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;
using CarPool.BL.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CarPool.App.Commands;
using CarPool.BL.Facades;

namespace CarPool.App.ViewModels
{
    public class AddCarViewModel : ViewModelBase, IAddCarViewModel
    {
        private readonly IMediator _mediator;
        private readonly CarFacade _CarFacade;
        public AddCarViewModel(
            CarFacade CarFacade,
            IMediator mediator)
        {
            _CarFacade = CarFacade;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

            mediator.Register<UserLoggedMessage>(UserLogged);

            ClearModel();
            LoggedUser = UserDetailModel.Empty;
            RedirectToProfileScreenCommand = new RelayCommand(RedirectToProfileScreen);
        }

        public CarWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }

        public ICommand SaveCommand { get; }

        public ICommand RedirectToProfileScreenCommand { get; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            if (Model == null || Model.Model.Owner == null || LoggedUser == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model.Model.Owner.Id = LoggedUser.Id;
            Model = await _CarFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<CarWrapper> { Model = Model });
            _mediator.Send(new UpdateMessage<UserWrapper> { Model = LoggedUser });
            ClearModel();
            RedirectToProfileScreen();
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

        private void RedirectToProfileScreen()
        {
            if (LoggedUser == null) return;
            _mediator.Send(new SelectedMessage<UserListWrapper> { Id = LoggedUser.Id });
        }
        private void ClearModel() {
            Model = CarDetailModel.Empty with { Owner = UserListModel.Empty};
            Model.Model.Owner = UserListModel.Empty;
        }

    }
}