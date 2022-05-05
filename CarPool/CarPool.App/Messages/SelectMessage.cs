using CarPool.BL.Models;

namespace CarPool.App.Messages
{
    public record SelectedMessage<T> : Message<T>
        where T : IModel
    {
    }
}