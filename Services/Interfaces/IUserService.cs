using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    public interface IUserService
    {
        List<UserViewModel> Get();

        bool Post(UserViewModel userViewModel);

        UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel user);
        UserViewModel GetById(String id);
        UserViewModel GetOne(String field, String value);
    }
}