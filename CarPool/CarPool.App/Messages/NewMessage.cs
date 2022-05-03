using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record NewMessage<T> : Message<T>
        where T : IModel
    {
    }
}