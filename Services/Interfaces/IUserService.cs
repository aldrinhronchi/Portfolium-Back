using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    public interface IUserService
    {
        Task<RequestViewModel<UserViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Task<RequestViewModel<UserViewModel>> CreateAsync(UserViewModel userViewModel);

        Task<RequestViewModel<UserViewModel>> UpdateAsync(UserViewModel userViewModel);

        Task<RequestViewModel<UserViewModel>> DeleteAsync(String id);

        Task<RequestViewModel<UserViewModel>> GetByIdAsync(String id);

        RequestViewModel<UserViewModel> Authenticate(UserAuthenticateRequestViewModel user);
    }
}