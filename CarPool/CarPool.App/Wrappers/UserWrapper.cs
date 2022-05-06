using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using CarPool.BL.Models;
using CarPool.App.Wrappers;

namespace CarPool.App.Wrappers
{
    public class UserWrapper : ModelWrapper<UserDetailModel>
    {
        public UserWrapper(UserDetailModel model) : base(model)
        {

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

        public CarWrapper? Cars
        {
            get => GetValue<CarWrapper>();
            set => SetValue(value);
        }

        public RideWrapper? RideAsPassenger
        {
            get => GetValue<RideWrapper>();
            set => SetValue(value);
        }

        public RideWrapper? RideAsDriver
        {
            get => GetValue<RideWrapper>();
            set => SetValue(value);
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
    }
}