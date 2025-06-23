using System.ComponentModel.DataAnnotations;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Context;
using Portfolium_Back.Connections.Repositories;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Portfolium_Back.Services
{
    /// <summary>
    /// Serviço para gestão de contatos
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly PortfoliumContext db;
        private readonly IContactRepository _contactRepository;

        public ContactService(PortfoliumContext db, IContactRepository contactRepository)
        {
            this.db = db;
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// Envia uma mensagem de contato
        /// </summary>
        /// <param name="contactMessage">Dados da mensagem de contato</param>
        /// <param name="ipAddress">Endereço IP do remetente</param>
        /// <param name="userAgent">User Agent do navegador</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ContactMessageViewModel>> SendMessageAsync(ContactMessageViewModel contactMessage, string? ipAddress, string? userAgent)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(contactMessage.Name))
                throw new ValidationException("Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(contactMessage.Email))
                throw new ValidationException("Email é obrigatório");

            if (string.IsNullOrWhiteSpace(contactMessage.Subject))
                throw new ValidationException("Assunto é obrigatório");

            if (string.IsNullOrWhiteSpace(contactMessage.Message))
                throw new ValidationException("Mensagem é obrigatória");

            // Captura informações adicionais
            contactMessage.IpAddress = ipAddress;
            contactMessage.UserAgent = userAgent;

            // Converter para entidade e salvar no banco
            var entity = contactMessage.ToEntity();
            var result = await _contactRepository.CreateContactMessageAsync(entity);

            var viewModel = new ContactMessageViewModel(result);

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel> { viewModel },
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem enviada com sucesso! Entraremos em contato em breve."
            };
        }

        /// <summary>
        /// Obtém informações de contato públicas
        /// </summary>
        /// <returns>Informações de contato</returns>
        public async Task<RequestViewModel<object>> GetContactInfoAsync()
        {
            var contactInfo = new
            {
                Email = "work.aldrironchi@gmail.com",
                Phone = "+55 (48) 99976-9594",
                WhatsApp = "+5548999769594",
                Location = "Curitibanos, SC - Brasil",
                SocialMedia = new
                {
                    LinkedIn = "https://www.linkedin.com/in/aldrin-ronchi-538320172/",
                    GitHub = "https://github.com/aldrinhronchi",
                    Instagram = "https://instagram.com/aldrinhronchi",
                    Twitter = "https://twitter.com/aldrinhronchi"
                },
                WorkingHours = new
                {
                    Monday = "09:00 - 18:00",
                    Tuesday = "09:00 - 18:00",
                    Wednesday = "09:00 - 18:00",
                    Thursday = "09:00 - 18:00",
                    Friday = "09:00 - 18:00",
                    Saturday = "Fechado",
                    Sunday = "Fechado"
                },
                AvailableServices = new[]
                {
                    "Desenvolvimento Web",
                    "Desenvolvimento Mobile",
                    "Consultoria em TI",
                    "Suporte Técnico"
                }
            };

            return new RequestViewModel<object>
            {
                Data = new List<object> { contactInfo },
                Type = "ContactInfo",
                Status = "Success",
                Message = "Informações de contato encontradas com sucesso"
            };
        }


        

        /// <summary>
        /// Marca uma mensagem como lida
        /// </summary>
        /// <param name="id">ID da mensagem</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ContactMessageViewModel>> MarkAsReadAsync(Guid id)
        {
            var success = await _contactRepository.MarkAsReadAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao marcar mensagem como lida");
            }

            var contactMessage = await _contactRepository.GetContactMessageByIdAsync(id);
            if (contactMessage == null)
            {
                throw new InvalidOperationException("Mensagem não encontrada");
            }

            var viewModel = new ContactMessageViewModel(contactMessage);

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel> { viewModel },
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem marcada como lida"
            };
        }

        /// <summary>
        /// Obtém a contagem de mensagens não lidas
        /// </summary>
        /// <returns>Contagem de mensagens não lidas</returns>
        public async Task<RequestViewModel<int>> GetUnreadCountAsync()
        {
            var count = await _contactRepository.GetUnreadCountAsync();

            return new RequestViewModel<int>
            {
                Data = new List<int> { count },
                Type = "UnreadCount",
                Status = "Success",
                Message = $"Existem {count} mensagens não lidas"
            };
        }

        public async Task<RequestViewModel<ContactMessageViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequestViewModel<ContactMessageViewModel> Requisicao;
            IQueryable<ContactMessage> _ContactMessages = db.ContactMessages.Where(x => x.IsActive);

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
                    IQueryable<ContactMessage> ContactMessageFiltrado = _ContactMessages;
                    foreach (KeyValuePair<String, String> Filtro in Filtros)
                    {
                        ContactMessageFiltrado = TipografiaHelper.Filtrar(ContactMessageFiltrado, Filtro.Key, Filtro.Value);
                    }
                    _ContactMessages = ContactMessageFiltrado;
                }
                else
                {
                    throw new ValidationException("Não foi possível filtrar!");
                }
            }

            if (!String.IsNullOrWhiteSpace(Ordenacao))
            {
                _ContactMessages = TipografiaHelper.Ordenar(_ContactMessages, Ordenacao, Ordem);
            }
            else
            {
                _ContactMessages = TipografiaHelper.Ordenar(_ContactMessages, "ID", Ordem);
            }

            Requisicao = await TipografiaHelper.FormatarRequisicaoParaViewModelAsync<ContactMessage, ContactMessageViewModel>(_ContactMessages, Pagina, RegistrosPorPagina, new ContactMessageViewModel());

            return Requisicao;
        }

        public async Task<RequestViewModel<ContactMessageViewModel>> GetByIdAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid messageId))
            {
                throw new ValidationException("ID inválido!");
            }

            ContactMessage? _contactMessage = await db.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.GuidID == messageId && x.IsActive);
            if (_contactMessage == null)
            {
                throw new ValidationException("Mensagem não encontrada!");
            }

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel> { new ContactMessageViewModel(_contactMessage) },
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem encontrada com sucesso"
            };
        }

        public async Task<RequestViewModel<ContactMessageViewModel>> CreateAsync(ContactMessageViewModel contactMessageViewModel)
        {
            Validator.ValidateObject(contactMessageViewModel, new ValidationContext(contactMessageViewModel), true);

            IRepository<ContactMessage> ContactMessageRepo = new Repository<ContactMessage>(db);

            ContactMessage newContactMessage = contactMessageViewModel.ToEntity();
            newContactMessage.IsActive = true;
            newContactMessage.DateCreated = TimeZoneManager.GetTimeNow();
            newContactMessage.UserCreated = "Sistema";

            await ContactMessageRepo.CreateAsync(newContactMessage);

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel> { new ContactMessageViewModel(newContactMessage) },
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem criada com sucesso"
            };
        }

        public async Task<RequestViewModel<ContactMessageViewModel>> UpdateAsync(ContactMessageViewModel contactMessageViewModel)
        {
            Validator.ValidateObject(contactMessageViewModel, new ValidationContext(contactMessageViewModel), true);

            IRepository<ContactMessage> ContactMessageRepo = new Repository<ContactMessage>(db);
            ContactMessage? _contactMessage = await db.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.GuidID == contactMessageViewModel.GuidID);

            if (_contactMessage == null)
            {
                throw new ValidationException("Mensagem não encontrada");
            }

            ContactMessage contactMessageToUpdate = new ContactMessage
            {
                ID = _contactMessage.ID,
                GuidID = _contactMessage.GuidID,
                Name = contactMessageViewModel.Name,
                Email = contactMessageViewModel.Email,
                Subject = contactMessageViewModel.Subject,
                Message = contactMessageViewModel.Message,
                IsRead = contactMessageViewModel.IsRead,
                ReadAt = contactMessageViewModel.ReadAt,
                Response = contactMessageViewModel.Response,
                ResponseAt = contactMessageViewModel.ResponseAt,
                IpAddress = contactMessageViewModel.IpAddress,
                UserAgent = contactMessageViewModel.UserAgent,
                DateCreated = _contactMessage.DateCreated,
                DateUpdated = DateTime.UtcNow,
                IsActive = _contactMessage.IsActive,
                UserCreated = _contactMessage.UserCreated,
                UserUpdated = "Sistema"
            };

            await ContactMessageRepo.UpdateAsync(contactMessageToUpdate);

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel> { contactMessageViewModel },
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem atualizada com sucesso"
            };
        }

        public async Task<RequestViewModel<ContactMessageViewModel>> DeleteAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid messageId))
            {
                throw new ValidationException("ID inválido!");
            }

            IRepository<ContactMessage> ContactMessageRepo = new Repository<ContactMessage>(db);

            ContactMessage? _contactMessage = await ContactMessageRepo.GetAsync(x => x.GuidID == messageId);
            if (_contactMessage == null)
            {
                throw new ValidationException("Mensagem não encontrada");
            }

            _contactMessage.IsActive = false;
            _contactMessage.DateUpdated = DateTime.UtcNow;
            _contactMessage.UserUpdated = "Sistema";
            await ContactMessageRepo.UpdateAsync(_contactMessage);

            return new RequestViewModel<ContactMessageViewModel>
            {
                Data = new List<ContactMessageViewModel>(),
                Type = nameof(ContactMessageViewModel),
                Status = "Success",
                Message = "Mensagem excluída com sucesso"
            };
        }
    }
} 