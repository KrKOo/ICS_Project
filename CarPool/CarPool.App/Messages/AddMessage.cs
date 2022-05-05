using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record AddedMessage<T> : Message<T>
        where T : IModel
    {
    }
}