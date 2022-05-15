using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record EditMessage<T> : Message<T>
        where T : IModel
    {
    }
}
