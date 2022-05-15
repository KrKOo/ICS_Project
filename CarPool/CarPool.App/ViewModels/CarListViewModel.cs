using CarPool.App.Commands;
using CarPool.App.Extensions;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarPool.App.ViewModels
{
    public class CarListViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _CarFacade;
        private readonly IMediator _mediator;
        public CarListViewModel(CarFacade CarFacade, IMediator mediator)
        {
            _CarFacade = CarFacade;
            _mediator = mediator;

            CarSelectedCommand = new RelayCommand<CarListModel>(CarSelected);
            CarNewCommand = new RelayCommand(CarNew);

            mediator.Register<UpdateMessage<CarWrapper>>(CarUpdated);
            mediator.Register<DeleteMessage<CarWrapper>>(CarDeleted);
        }
        public ObservableCollection<CarListModel> Cars { get; } = new();

        public ICommand CarNewCommand { get; }

        public ICommand CarSelectedCommand { get; }

        private void CarSelected(CarListModel? CarListModel)
        {
            if (CarListModel is not null)
            {
                _mediator.Send(new SelectedMessage<CarWrapper> { Id = CarListModel.Id });
            }
        }

        private void CarNew()
        {
            _mediator.Send(new NewMessage<CarWrapper>());
        }

        private async void CarDeleted(DeleteMessage<CarWrapper> obj) => await LoadAsync();

        private async void CarUpdated(UpdateMessage<CarWrapper> obj) => await LoadAsync();

        public async Task LoadAsync()
        {
            Cars.Clear();
            var cars = await _CarFacade.GetAsync();
            Cars.AddRange(cars);
        }
    }
}
