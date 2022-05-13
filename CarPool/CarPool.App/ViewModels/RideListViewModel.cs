using CarPool.App.Commands;
using CarPool.App.Extensions;
using CarPool.App.Messages;
using CarPool.App.Services;
using CarPool.App.Wrappers;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

            RideSelectedCommand = new RelayCommand<RideListModel>(RideSelected);
            RedirectToProfileScreenCommand = new RelayCommand(RedirectToProfileScreen);
            RideNewCommand = new RelayCommand(RideNew);

            mediator.Register<UpdateMessage<RideWrapper>>(RideUpdated);
            mediator.Register<DeleteMessage<RideWrapper>>(RideDeleted);
        }
        public ObservableCollection<RideListModel> Rides { get; } = new();

        public ICommand RideNewCommand { get; }

        public ICommand RideSelectedCommand { get; }

        public ICommand RedirectToProfileScreenCommand { get; set; }

        private void RideSelected(RideListModel? rideListModel)
        {
            if (rideListModel is not null) {
                _mediator.Send(new SelectedMessage<RideWrapper> { Id = rideListModel.Id });
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
            var rides = await _rideFacade.GetAsync();
            Rides.AddRange(rides);
        }

        public void RedirectToProfileScreen()
        {
            _mediator.Send(new RedirectToProfileScreenMessage());
        }
    }
}
