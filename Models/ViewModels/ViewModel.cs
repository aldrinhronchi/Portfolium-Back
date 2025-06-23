namespace Portfolium_Back.Models.ViewModels
{
    public interface IViewModel<T, TViewModel>
    {
        TViewModel ToViewModel(T entity);
        T ToEntity();
    }
}