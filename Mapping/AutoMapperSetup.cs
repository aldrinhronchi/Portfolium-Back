using AutoMapper;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Mapping
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            #region ViewModelToDomain

            CreateMap<UserViewModel, User>();

            #endregion ViewModelToDomain

            #region DomainToViewModel

            CreateMap<User, UserViewModel>();

            #endregion DomainToViewModel
        }
    }
}