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
        private readonly IMessageDialogService _messageDialogService;

        public CarDetailViewModel(
            CarFacade CarFacade,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _CarFacade = CarFacade;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync);
        }

        public CarWrapper? Model { get; private set; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }


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
        }

        private bool CanSave() => Model?.IsValid ?? false;

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
                    $"Do you want to delete {Model?.LicensePlate}?.",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No) return;

                try
                {
                    await _CarFacade.DeleteAsync(Model!.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of {Model?.LicensePlate} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<CarWrapper>
                {
                    Model = Model
                });
            }
        }

        public override void LoadInDesignMode()
        {
            base.LoadInDesignMode();
            Model = new CarWrapper(new CarDetailModel(
                Manufacturer: "Volvo",
                Model: "V40",
                LicensePlate: "BL340UZ",
                DateOfRegistration: new DateOnly(2000, 12, 3),
                PhotoUrl: @"https://img.tipcars.com/fotky_velke/18083317_1/1636098034/E/volvo-v40-2-0-d2-kinetic.jpg",
                NumberOfSeats: 4
            ));
        }
    }
}