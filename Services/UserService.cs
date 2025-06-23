using Portfolium_Back.Context;
using Portfolium_Back.Connections.Repositories;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Portfolium_Back.Services
{
    public class UserService : IUserService
    {
        private readonly PortfoliumContext db;

        public UserService(PortfoliumContext db)
        {
            this.db = db;
        }

        public async Task<RequestViewModel<UserViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequestViewModel<UserViewModel> Requisicao;
            IQueryable<User> _Users = db.Users.Where(x => x.IsActive);

            if (!String.IsNullOrWhiteSpace(CamposQuery))
            {
                String[] CamposArray = CamposQuery.Split(";|;");
                String[] ValoresArray = ValoresQuery.Split(";|;");
                if (CamposArray.Length == ValoresArray.Length)
                {
                    Dictionary<String, String> Filtros = new Dictionary<String, String>();
                    for (Int32 index = 0; index < CamposArray.Length; index++)
                    {
                        String? Campo = CamposArray[index];
                        String? Valor = ValoresArray[index];
                        if (!(String.IsNullOrWhiteSpace(Campo) && String.IsNullOrWhiteSpace(Valor)))
                        {
                            Filtros.Add(Campo, Valor);
                        }
                    }
                    IQueryable<User> UserFiltrado = _Users;
                    foreach (KeyValuePair<String, String> Filtro in Filtros)
                    {
                        UserFiltrado = TipografiaHelper.Filtrar(UserFiltrado, Filtro.Key, Filtro.Value);
                    }
                    _Users = UserFiltrado;
                }
                else
                {
                    throw new ValidationException("Não foi possível filtrar!");
                }
            }

            if (!String.IsNullOrWhiteSpace(Ordenacao))
            {
                _Users = TipografiaHelper.Ordenar(_Users, Ordenacao, Ordem);
            }
            else
            {
                _Users = TipografiaHelper.Ordenar(_Users, "ID", Ordem);
            }

            Requisicao = await TipografiaHelper.FormatarRequisicaoParaViewModelAsync<User, UserViewModel>(_Users, Pagina, RegistrosPorPagina, new UserViewModel());

            return Requisicao;
        }

        public async Task<RequestViewModel<UserViewModel>> GetByIdAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new ValidationException("ID inválido!");
            }

            User? _user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GuidID == userId && x.IsActive);
            if (_user == null)
            {
                throw new ValidationException("Usuário não encontrado!");
            }



            return new RequestViewModel<UserViewModel>
            {
                Data = new List<UserViewModel> { new UserViewModel(_user) },
                Type = nameof(UserViewModel),
                Status = "Success",
                Message = "Usuário encontrado com sucesso"
            };
        }

        public async Task<RequestViewModel<UserViewModel>> CreateAsync(UserViewModel userViewModel)
        {

            Validator.ValidateObject(userViewModel, new ValidationContext(userViewModel), true);

            IRepository<User> UserRepo = new Repository<User>(db);

            // Verificar se email já existe
            User? existingUser = await UserRepo.GetAsync(x => x.Email.ToUpper() == userViewModel.Email.ToUpper());
            if (existingUser != null)
            {
                throw new ValidationException("Email já cadastrado");
            }

            User newUser = userViewModel.ToEntity();

            newUser.Password = EncryptPassword(newUser.Password);
            newUser.IsActive = true;
            newUser.DateCreated = TimeZoneManager.GetTimeNow();
            newUser.UserCreated = "Sistema";

            await UserRepo.CreateAsync(newUser);

            return new RequestViewModel<UserViewModel>
            {
                Data = new List<UserViewModel> { new UserViewModel(newUser) },
                Type = nameof(UserViewModel),
                Status = "Success",
                Message = "Usuário criado com sucesso"
            };


        }

        public async Task<RequestViewModel<UserViewModel>> UpdateAsync(UserViewModel userViewModel)
        {

            Validator.ValidateObject(userViewModel, new ValidationContext(userViewModel), true);

            IRepository<User> UserRepo = new Repository<User>(db);
            User? _user = await UserRepo.GetAsync(x => x.GuidID == userViewModel.GuidID);

            if (_user == null)
            {
                throw new ValidationException("Usuário não encontrado");
            }

            // Verificar se email já existe em outro usuário
            User? existingUser = await UserRepo.GetAsync(x => x.Email.ToUpper() == userViewModel.Email.ToUpper() && x.GuidID != userViewModel.GuidID);
            if (existingUser != null)
            {
                throw new ValidationException("Email já cadastrado por outro usuário");
            }

            User userToUpdate = new User
            {
                ID = _user.ID,
                GuidID = _user.GuidID,
                Name = userViewModel.Name,
                Email = userViewModel.Email,
                Password = !String.IsNullOrEmpty(userViewModel.Password) ? EncryptPassword(userViewModel.Password) : _user.Password,
                Role = userViewModel.Role,
                DateCreated = _user.DateCreated,
                DateUpdated = DateTime.UtcNow,
                IsActive = _user.IsActive,
                UserCreated = _user.UserCreated,
                UserUpdated = "Sistema" // ou pegar do contexto do usuário logado
            };

            await UserRepo.UpdateAsync(userToUpdate);

            return new RequestViewModel<UserViewModel>
            {
                Data = new List<UserViewModel> { userViewModel },
                Type = nameof(UserViewModel),
                Status = "Success",
                Message = "Usuário atualizado com sucesso"
            };

        }

        public async Task<RequestViewModel<UserViewModel>> DeleteAsync(String id)
        {

            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new ValidationException("ID inválido!");
            }

            IRepository<User> UserRepo = new Repository<User>(db);

            User? _user = await UserRepo.GetAsync(x => x.GuidID == userId);
            if (_user == null)
            {
                throw new ValidationException("Usuário não encontrado");
            }

            _user.IsActive = false;
            _user.DateUpdated = DateTime.UtcNow;
            _user.UserUpdated = "Sistema";
            await UserRepo.UpdateAsync(_user);

            return new RequestViewModel<UserViewModel>
            {
                Data = new List<UserViewModel>(),
                Type = nameof(UserViewModel),
                Status = "Success",
                Message = "Usuário excluído com sucesso"
            };

        }

        public RequestViewModel<UserViewModel> Authenticate(UserAuthenticateRequestViewModel user)
        {
            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                throw new ValidationException("Email/Password são obrigatórios.");
            }

            user.Password = EncryptPassword(user.Password);

            User? _user = db.Users.FirstOrDefault(x => x.IsActive && x.Email.ToUpper() == user.Email.ToUpper()
                                              && x.Password.ToUpper() == user.Password.ToUpper());
            if (_user == null)
            {
                throw new UnauthorizedAccessException("Usuário não encontrado ou senha incorreta");
            }

            UserViewModel userViewModel = new UserViewModel(_user);
            userViewModel.Token = TokenHelper.GenerateToken(_user);
            return new RequestViewModel<UserViewModel>
            {
                Data = new List<UserViewModel> { userViewModel },
                Type = nameof(UserViewModel),
                Status = "Success",
                Message = "Usuário autenticado com sucesso"
            };
        }

        private String EncryptPassword(String password)
        {
            HashAlgorithm sha = SHA1.Create();

            Byte[] encryptedPassword = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                stringBuilder.Append(caracter.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}