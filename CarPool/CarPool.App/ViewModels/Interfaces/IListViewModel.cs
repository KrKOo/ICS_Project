using System.Threading.Tasks;

namespace CarPool.App.ViewModels
{
    public interface IListViewModel : IViewModel
    {
        Task LoadAsync();
    }
}
