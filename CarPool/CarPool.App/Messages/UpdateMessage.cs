using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record UpdateMessage<T> : Message<T>
        where T : IModel
    {
    }
}

