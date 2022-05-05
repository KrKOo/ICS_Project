using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarPool.BL.Models;

namespace CarPool.App.Wrappers
{
    public class RideWrapper : ModelWrapper<RideDetailModel>
    {
        public RideWrapper(RideDetailModel model)
            : base(model)
        {

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

        public string RideOrigin
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string RideDestination
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? Info
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public UserWrapper? Driver
        {
            get => GetValue<UserWrapper>();
            set => SetValue(value);
        }

        public CarWrapper? Car
        {
            get => GetValue<CarWrapper>();
            set => SetValue(value);
        }

        public UserWrapper Passengers
        {
            get => GetValue<UserWrapper>();
            set => SetValue(value);
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

            if (TimeOfStart == null)
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