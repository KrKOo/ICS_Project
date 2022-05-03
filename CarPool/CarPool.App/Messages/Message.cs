using CarPool.BL.Models;
using System;

namespace CarPool.App.Messages
{
    public abstract record Message<T> : IMessage
        where T : IModel
    {
        private readonly Guid? _id;

        public Guid? Id
        {
            get => _id ?? Model?.Id;
            set => _id = value;
        }

        public Guid? TargetId { get; init; }
        public T? Model { get; init; }
    }
}

