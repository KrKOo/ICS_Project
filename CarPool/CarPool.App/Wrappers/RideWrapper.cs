using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarPool.BL.Models;
using CarPool.App.Wrappers;
using System.Collections.ObjectModel;
using CarPool.App.Extensions;
using System.Linq;

namespace CarPool.App.Wrappers
{
    public class RideWrapper : ModelWrapper<RideDetailModel>
    {
        
        public RideWrapper(RideDetailModel model)
            : base(model)
        {
            InitializeCollectionProperties(model);
            InitializeNavigationProperties(model);
        }

        public DateTime TimeOfStart
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public TimeSpan Duration
        {
            get => GetValue<TimeSpan>();
            set => SetValue(value);
        }

        public string? RideOrigin
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? RideDestination
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? Info
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public UserListWrapper? Driver { get; set; }
        public CarListWrapper? Car { get; set; }

        public ObservableCollection<UserListWrapper> Passengers { get; set; } = new();

        private void InitializeNavigationProperties(RideDetailModel model)
        {
            if (model.Driver == null) {
                throw new ArgumentException("Driver cannot be null");
            }

            Driver = model.Driver;

            if (model.Car == null)
            {
                throw new ArgumentException("Car cannot be null");
            }

            Car = model.Car;
        }
        private void InitializeCollectionProperties(RideDetailModel model)
        {
            if (model.Passengers == null)
            {
                throw new ArgumentException("Passenger cannot be null");
            }
            Passengers.AddRange(model.Passengers.Select(e => new UserListWrapper(e)));

            RegisterCollection(Passengers, model.Passengers);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(RideOrigin))
            {
                yield return new ValidationResult($"{nameof(RideOrigin)} is required", new[] { nameof(RideOrigin) });
            }

            if (string.IsNullOrWhiteSpace(RideDestination))
            {
                yield return new ValidationResult($"{nameof(RideDestination)} is required", new[] { nameof(RideDestination) });
            }

            if (TimeOfStart == default)
            {
                yield return new ValidationResult($"{nameof(TimeOfStart)} is required", new[] { nameof(TimeOfStart) });
            }
        }


        public static implicit operator RideWrapper(RideDetailModel detailModel)
            => new(detailModel);

        public static implicit operator RideDetailModel(RideWrapper wrapper)
            => wrapper.Model;
    }
}