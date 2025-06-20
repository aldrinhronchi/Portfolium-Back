using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    public interface IUserService
    {
        Task<RequisicaoViewModel<User>> ListarAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Task<Boolean> SalvarAsync(UserViewModel userViewModel);

        Task<Boolean> ExcluirAsync(String id);

        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel user);

        UserViewModel GetById(String id);
    }
}