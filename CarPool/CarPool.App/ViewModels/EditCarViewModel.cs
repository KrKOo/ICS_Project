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
    public class EditCarViewModel : ViewModelBase, IEditCarViewModel
    {
        private readonly IMediator _mediator;
        private readonly CarFacade _CarFacade;
        public EditCarViewModel(
            CarFacade CarFacade,
            IMediator mediator)
        {
            _CarFacade = CarFacade;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);

            mediator.Register<UserLoggedMessage>(UserLogged);

            LoggedUser = UserDetailModel.Empty;
            RedirectToCarDetailScreenCommand = new RelayCommand(RedirectToCarDetailScreen);
        }

        public CarWrapper? Model { get; private set; }
        public UserWrapper? LoggedUser { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand RedirectToCarDetailScreenCommand { get; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _CarFacade.GetAsync(id) ?? CarDetailModel.Empty;
        }

        public async Task SaveAsync()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            Model = await _CarFacade.SaveAsync(Model.Model);
            _mediator.Send(new UpdateMessage<CarWrapper> { Model = Model });
            RedirectToCarDetailScreen();
        }

        private bool CanSave() => Model?.IsValid ?? false;

        public void UserLogged(UserLoggedMessage userLoggedMessage)
        {
            if (userLoggedMessage.User == null) return;
            LoggedUser = userLoggedMessage.User;
        }

        private void RedirectToCarDetailScreen()
        {
            if (Model is not null) 
            {
                _mediator.Send(new SelectedMessage<CarListWrapper> { Id = Model.Id });
            }
        }
    }
}