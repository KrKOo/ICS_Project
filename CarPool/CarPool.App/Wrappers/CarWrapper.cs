using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarPool.BL.Models;
using CarPool.App.Wrappers;

namespace CarPool.App.Wrappers
{
    public class CarWrapper : ModelWrapper<CarDetailModel>
    {
        public CarWrapper(CarDetailModel model) : base(model)
        {
            InitializeNavigationProperties(model);
        }

        public string? Manufacturer
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? CarModel
        {
            get => GetValue<string>("Model");
            set => SetValue(value, "Model");
        }

        public string? LicensePlate
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateOnly DateOfRegistration
        {
            get => GetValue<DateOnly>();
            set => SetValue(value);
        }

        public string? PhotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int NumberOfSeats
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public UserListWrapper? Owner { get; set; }

        private void InitializeNavigationProperties(CarDetailModel model)
        {
            if (model.Owner == null)
            {
                throw new ArgumentException("Owner cannot be null");
            }

            Owner = model.Owner;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Manufacturer))
            {
                yield return new ValidationResult($"{nameof(Manufacturer)} is required", new[] { nameof(Manufacturer) });
            }

            if (string.IsNullOrWhiteSpace(CarModel))
            {
                yield return new ValidationResult($"{nameof(Model)} is required", new[] { nameof(Model) });
            }

            if (string.IsNullOrWhiteSpace(LicensePlate))
            {
                yield return new ValidationResult($"{nameof(LicensePlate)} is required", new[] { nameof(LicensePlate) });
            }

            if (string.IsNullOrWhiteSpace(PhotoUrl))
            {
                yield return new ValidationResult($"{nameof(PhotoUrl)} is required", new[] { nameof(PhotoUrl) });
            }
        }

        public static implicit operator CarWrapper(CarDetailModel detailModel)
            => new CarWrapper(detailModel);

        public static implicit operator CarDetailModel(CarWrapper wrapper)
            => wrapper.Model;
    }
}