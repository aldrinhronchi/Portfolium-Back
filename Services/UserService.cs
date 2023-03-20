using AutoMapper;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Extensions;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Portfolium_Back.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public List<UserViewModel> Get()
        {
            List<UserViewModel> _userViewModels = new List<UserViewModel>();
            IEnumerable<User> _users = this.userRepository.GetAll();

            _userViewModels = mapper.Map<List<UserViewModel>>(_users);

            return _userViewModels;
        }

        public UserViewModel GetOne(String field, String value)
        {
            UserViewModel _userViewModel = new UserViewModel();
            User _user;
            IEnumerable<User> _users = this.userRepository.GetAll();

            switch (field.ToUpper())
            {
                default:
                case "NAME":
                    _user = _users.FirstOrDefault(x => x.Name == value);
                    break;

                case "EMAIL":
                    _user = _users.FirstOrDefault(x => x.Email == value);
                    break;

                case "PASSWORD":
                    _user = _users.FirstOrDefault(x => x.Password == value);
                    break;

                case "ROLE":
                    _user = _users.FirstOrDefault(x => x.Role == value);
                    break;
            }

            if (_users.IsNullOrEmpty())
            {
                return null;
            }

            _userViewModel = mapper.Map<UserViewModel>(_user);
            return _userViewModel;
        }

        public UserViewModel GetById(String id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new Exception("UserID is not valid!");
            }
            User _user = this.userRepository.Find(x => x.GuidID == userId & x.IsActive);
            if (_user == null)
            {
                throw new Exception("User not found");
            }
            return mapper.Map<UserViewModel>(_user);
        }

        public bool Post(UserViewModel userViewModel)
        {
            if (userViewModel.GuidID != Guid.Empty)
            {
                throw new Exception("UserID must be empty!");
            }

            Validator.ValidateObject(userViewModel, new ValidationContext(userViewModel), true);

            User _user = mapper.Map<User>(userViewModel);
            _user.Password = EncryptPassword(_user.Password);

            this.userRepository.Create(_user);
            return true;
        }

        public UserAuthenticateResponseViewModel Authenticate(UserAuthenticateRequestViewModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                throw new Exception("Email/Password are required.");
            }

            user.Password = EncryptPassword(user.Password);

            User _user = this.userRepository.Find(x => x.IsActive && x.Email.ToUpper() == user.Email.ToUpper()
                                                  && x.Password.ToUpper() == user.Password.ToUpper());
            if (_user == null)
            {
                throw new Exception("User not found");
            }
            return new UserAuthenticateResponseViewModel(mapper.Map<UserViewModel>(_user), TokenService.GenerateToken(_user));
        }

        private string EncryptPassword(string password)
        {
            HashAlgorithm sha = new SHA1CryptoServiceProvider();

            byte[] encryptedPassword = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                stringBuilder.Append(caracter.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}