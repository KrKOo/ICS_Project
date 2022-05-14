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

        }

        public CarWrapper? Model { get; private set; }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task LoadAsync(Guid id)
        {
            Model = await _CarFacade.GetAsync(id) ?? CarDetailModel.Empty;
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}