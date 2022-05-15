using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarPool.BL.Models;
using System;
using System.Collections.ObjectModel;
using CarPool.App.Extensions;
using System.Linq;

namespace CarPool.App.Wrappers
{
    public class UserWrapper : ModelWrapper<UserDetailModel>
    {
        public UserWrapper(UserDetailModel model) : base(model)
        {
            InitializeCollectionProperties(model);
        }

        public string? Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? PhoneNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateOnly? DateOfBirth
        {
            get => GetValue<DateOnly>();
            set => SetValue(value);
        }

        public string? PhotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? Info
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ObservableCollection<CarListWrapper> Cars { get; set; } = new();
        public ObservableCollection<RideListWrapper> RidesAsPassenger { get; set; } = new();
        public ObservableCollection<RideListWrapper> RidesAsDriver { get; set; } = new();
        private void InitializeCollectionProperties(UserDetailModel model)
        {
            if (model.Cars == null)
            {
                throw new ArgumentException("Cars cannot be null");
            }
            Cars.AddRange(model.Cars.Select(e => new CarListWrapper(e)));

            RegisterCollection(Cars, model.Cars);

            if (model.RidesAsPassenger == null)
            {
                throw new ArgumentException("RidesAsPassenger cannot be null");
            }
            RidesAsPassenger.AddRange(model.RidesAsPassenger.Select(e => new RideListWrapper(e)));

            RegisterCollection(RidesAsPassenger, model.RidesAsPassenger);

            if (model.RidesAsDriver == null)
            {
                throw new ArgumentException("RidesAsDriver cannot be null");
            }
            RidesAsDriver.AddRange(model.RidesAsDriver.Select(e => new RideListWrapper(e)));

            RegisterCollection(RidesAsDriver, model.RidesAsDriver);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult($"{nameof(Email)} is required", new[] { nameof(Email) });
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                yield return new ValidationResult($"{nameof(FirstName)} is required", new[] { nameof(FirstName) });
            }

            if(string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult($"{nameof(LastName)} is required", new[] { nameof(LastName) });
            }

            if(string.IsNullOrWhiteSpace(PhotoUrl))
            {
                yield return new ValidationResult($"{nameof(PhotoUrl)} is required", new[] { nameof(PhotoUrl) });
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                yield return new ValidationResult($"{nameof(PhoneNumber)} is required", new[] { nameof(PhoneNumber) });
            }
        }

        public static implicit operator UserWrapper(UserDetailModel detailModel)
            => new(detailModel);

        public static implicit operator UserDetailModel(UserWrapper wrapper)
            => wrapper.Model;
    }
}