using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarPool.BL.Models;
using CarPool.App.Wrappers;

namespace CarPool.App.Wrappers
{
    public class RideListWrapper : ModelWrapper<RideListModel>
    {
        public RideListWrapper(RideListModel model)
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

        public static implicit operator RideListWrapper(RideListModel listModel)
            => new(listModel);

        public static implicit operator RideListModel(RideListWrapper wrapper)
            => wrapper.Model;
    }
}