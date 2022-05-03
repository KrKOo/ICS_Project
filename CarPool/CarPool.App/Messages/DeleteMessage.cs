using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record DeleteMessage<T> : Message<T>
        where T : IModel
    {
    }
}

