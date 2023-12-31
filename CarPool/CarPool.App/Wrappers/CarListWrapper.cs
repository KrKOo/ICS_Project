﻿using CarPool.BL.Models;

namespace CarPool.App.Wrappers
{
    public class CarListWrapper : ModelWrapper<CarListModel>
    {
        public CarListWrapper(CarListModel model) : base(model)
        {

        }

        public string? LicensePlate
        {
            get => GetValue<string>();
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


        public static implicit operator CarListWrapper(CarListModel listModel)
            => new CarListWrapper(listModel);

        public static implicit operator CarListModel(CarListWrapper wrapper)
            => wrapper.Model;
    }
}