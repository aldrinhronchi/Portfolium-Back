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

        public async Task<RequisicaoViewModel<User>> ListarAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequisicaoViewModel<User> Requisicao;
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
            
            Requisicao = await TipografiaHelper.FormatarRequisicaoAsync(_Users, Pagina, RegistrosPorPagina);
            
            return Requisicao;
        }

        public UserViewModel GetById(String id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new ValidationException("UserID não é válido!");
            }
            User? _user = db.Users.AsNoTracking().FirstOrDefault(x => x.GuidID == userId && x.IsActive);
            if (_user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado");
            }
            return new UserViewModel
            {
                GuidID = _user.GuidID,
                Name = _user.Name,
                Email = _user.Email,
                Role = _user.Role
            };
        }

        public async Task<Boolean> SalvarAsync(UserViewModel userViewModel)
        {
            Validator.ValidateObject(userViewModel, new ValidationContext(userViewModel), true);

            IRepository<User> UserRepo = new Repository<User>(db);
            User? _user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GuidID == userViewModel.GuidID);
            
            if (_user == null)
            {
                if (userViewModel.GuidID != null && userViewModel.GuidID != Guid.Empty)
                {
                    throw new ValidationException("ID deve ser vazio para novo usuário!");
                }
                
                // Verificar se email já existe
                User? existingUser = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToUpper() == userViewModel.Email.ToUpper());
                if (existingUser != null)
                {
                    throw new ValidationException("Email já cadastrado");
                }

                User newUser = new User
                {
                    GuidID = Guid.NewGuid(),
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Password = EncryptPassword(userViewModel.Password),
                    Role = userViewModel.Role,
                    DateCreated = TimeZoneManager.GetTimeNow(),
                    IsActive = true,
                    UserCreated = "Sistema" // ou pegar do contexto do usuário logado
                };

                await UserRepo.CreateAsync(newUser);
            }
            else
            {
                User userToUpdate = new User
                {
                    ID = _user.ID,
                    GuidID = _user.GuidID,
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Password = !String.IsNullOrEmpty(userViewModel.Password) ? EncryptPassword(userViewModel.Password) : _user.Password,
                    Role = userViewModel.Role,
                    DateCreated = _user.DateCreated,
                    DateUpdated = TimeZoneManager.GetTimeNow(),
                    IsActive = _user.IsActive,
                    UserCreated = _user.UserCreated,
                    UserUpdated = "Sistema" // ou pegar do contexto do usuário logado
                };

                await UserRepo.UpdateAsync(userToUpdate);
            }
            
            return true;
        }

        public async Task<Boolean> ExcluirAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new ValidationException("ID inválido!");
            }
            
            IRepository<User> UserRepo = new Repository<User>(db);

            User? _user = await UserRepo.GetAsync(x => x.GuidID == userId);
            if (_user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado");
            }
            
            return await UserRepo.DeleteAsync(_user);
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel user)
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
            
            return new UserAuthenticateResponseViewModel(userViewModel, TokenHelper.GenerateToken(_user));
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