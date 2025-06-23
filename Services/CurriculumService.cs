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
    /// Serviço para gestão de currículo
    /// </summary>
    public class CurriculumService : ICurriculumService
    {
        private readonly PortfoliumContext db;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUserRepository _userRepository;
        private readonly Guid userId = TokenHelper.GetSingleUserId();

        public CurriculumService(PortfoliumContext db, ICurriculumRepository curriculumRepository, IUserRepository userRepository)
        {
            this.db = db;
            _curriculumRepository = curriculumRepository;
            _userRepository = new UserRepository(db);
        }


        /// <summary>
        /// Obtém o currículo completo
        /// </summary>

        /// <returns>Dados completos do currículo</returns>
        public async Task<RequestViewModel<CurriculumViewModel>> GetCurriculumAsync()
        {


            var personalInfo = await GetPersonalInfoAsync();
            var skills = await GetSkillsAsync();
            var experiences = await GetExperiencesAsync();
            var education = await GetEducationAsync();
            var certifications = await GetCertificationsAsync();
            var services = await GetServicesAsync();

            var curriculum = new CurriculumViewModel
            {
                PersonalInfo = personalInfo.Data.FirstOrDefault(),
                Skills = skills.Data,
                Experiences = experiences.Data,
                Education = education.Data,
                Certifications = certifications.Data,
                Services = services.Data
            };

            return new RequestViewModel<CurriculumViewModel>
            {
                Data = new List<CurriculumViewModel> { curriculum },
                Type = nameof(CurriculumViewModel),
                Status = "Success",
                Message = "Currículo encontrado com sucesso"
            };
        }

        /// <summary>
        /// Obtém as informações pessoais
        /// </summary>
        /// <returns>Informações pessoais</returns>
        public async Task<RequestViewModel<PersonalInfoViewModel>> GetPersonalInfoAsync()
        {


            var personalInfo = await _curriculumRepository.GetPersonalInfoAsync(userId);

            if (personalInfo == null)
            {
                throw new ValidationException("Informações pessoais não encontradas");
            }

            var viewModel = new PersonalInfoViewModel(personalInfo);

            return new RequestViewModel<PersonalInfoViewModel>
            {
                Data = new List<PersonalInfoViewModel> { viewModel },
                Type = nameof(PersonalInfoViewModel),
                Status = "Success",
                Message = "Informações pessoais encontradas com sucesso"
            };
        }

        /// <summary>
        /// Cria as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
       
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<PersonalInfoViewModel>> CreatePersonalInfoAsync(PersonalInfoViewModel personalInfo)
        {
            if (personalInfo == null)
            {
                throw new ArgumentNullException(nameof(personalInfo));
            }

            var user = await _userRepository.GetAsync(x => x.GuidID ==  userId);

            if (user == null)
            {
                throw new ValidationException("Usuário não encontrado");
            }


            if (personalInfo.GuidID.HasValue && personalInfo.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar informações pessoais, não informe o ID");
            }


            var entity = personalInfo.ToEntity();
            entity.UserID = userId;
            entity.UserCreated = $"{user.ID} - {user.Name}";

            var result = await _curriculumRepository.CreatePersonalInfoAsync(entity);
            var viewModel = new PersonalInfoViewModel(result);

            return new RequestViewModel<PersonalInfoViewModel>
            {
                Data = new List<PersonalInfoViewModel> { viewModel },
                Type = nameof(PersonalInfoViewModel),
                Status = "Success",
                Message = "Informações pessoais criadas com sucesso"
            };
        }

        /// <summary>
        /// Atualiza as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<PersonalInfoViewModel>> UpdatePersonalInfoAsync(PersonalInfoViewModel personalInfo)
        {
            if (personalInfo == null)
            {
                throw new ArgumentNullException(nameof(personalInfo));
            }

            var user = await _userRepository.GetAsync(x => x.GuidID == userId);

            if (user == null)
            {
                throw new ValidationException("Usuário não encontrado");
            }

            if (!personalInfo.GuidID.HasValue || personalInfo.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar informações pessoais, informe o ID");
            }

            var personalInfoEntity = await _curriculumRepository.GetPersonalInfoAsync(userId);
            if (personalInfoEntity == null || personalInfoEntity.GuidID != personalInfo.GuidID.Value)
            {
                throw new ValidationException("Informações pessoais não encontradas");
            }

            // Atualizar a entidade existente ao invés de criar nova
            personalInfoEntity.Name = personalInfo.Name;
            personalInfoEntity.Title = personalInfo.Title;
            personalInfoEntity.Description = personalInfo.Description;
            personalInfoEntity.Location = personalInfo.Location;
            personalInfoEntity.Phone = personalInfo.Phone;
            personalInfoEntity.Email = personalInfo.Email;
            personalInfoEntity.YearsExperience = personalInfo.YearsExperience;
            personalInfoEntity.ProjectsCompleted = personalInfo.ProjectsCompleted;
            personalInfoEntity.HappyClients = personalInfo.HappyClients;
            personalInfoEntity.Certifications = personalInfo.Certifications;
            personalInfoEntity.LinkedInUrl = personalInfo.LinkedInUrl;
            personalInfoEntity.GitHubUrl = personalInfo.GitHubUrl;
            personalInfoEntity.PortfolioUrl = personalInfo.PortfolioUrl;
            personalInfoEntity.UserUpdated = $"{user.ID} - {user.Name}";
            personalInfoEntity.DateUpdated = DateTime.UtcNow;

            var entity = personalInfoEntity;

            var success = await _curriculumRepository.UpdatePersonalInfoAsync(entity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar informações pessoais");
            }

            var viewModel = new PersonalInfoViewModel(entity);

            return new RequestViewModel<PersonalInfoViewModel>
            {
                Data = new List<PersonalInfoViewModel> { viewModel },
                Type = nameof(PersonalInfoViewModel),
                Status = "Success",
                Message = "Informações pessoais atualizadas com sucesso"
            };
        }

        /// <summary>
        /// Lista as habilidades
        /// </summary>
       
        /// <param name="category">Categoria específica</param>
        /// <returns>Lista de habilidades</returns>
        public async Task<RequestViewModel<SkillViewModel>> GetSkillsAsync(string? category = null)
        {


            var skills = await _curriculumRepository.GetSkillsAsync(userId, category);
            var viewModels = skills.Select(s => new SkillViewModel(s)).ToList();

            return new RequestViewModel<SkillViewModel>
            {
                Data = viewModels,
                Type = nameof(SkillViewModel),
                Status = "Success",
                Message = $"Encontradas {viewModels.Count} habilidades"
            };
        }

        /// <summary>
        /// Cria uma nova habilidade
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
       
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<SkillViewModel>> CreateSkillAsync(SkillViewModel skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }



            if (skill.GuidID.HasValue && skill.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar uma habilidade, não informe o ID");
            }

            var entity = skill.ToEntity();
            entity.UserID = userId;

            var result = await _curriculumRepository.CreateSkillAsync(entity);
            var viewModel = new SkillViewModel(result);

            return new RequestViewModel<SkillViewModel>
            {
                Data = new List<SkillViewModel> { viewModel },
                Type = nameof(SkillViewModel),
                Status = "Success",
                Message = "Habilidade criada com sucesso"
            };
        }

        /// <summary>
        /// Atualiza uma habilidade existente
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
        /// <param name="userId">ID do usuário</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<SkillViewModel>> UpdateSkillAsync(SkillViewModel skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            if (!skill.GuidID.HasValue || skill.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar uma habilidade, informe o ID");
            }

            var skillEntity = await _curriculumRepository.GetSkillByIdAsync(skill.GuidID.Value);
            if (skillEntity == null)
            {
                throw new ValidationException("Habilidade não encontrada");
            }

            // Atualizar a entidade existente
            skillEntity.Name = skill.Name;
            skillEntity.Level = skill.Level;
            skillEntity.Category = skill.Category;
            skillEntity.DisplayOrder = skill.DisplayOrder;
            skillEntity.Icon = skill.Icon;
            skillEntity.Color = skill.Color;
            skillEntity.DateUpdated = DateTime.UtcNow;

            var success = await _curriculumRepository.UpdateSkillAsync(skillEntity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar habilidade");
            }

            var viewModel = new SkillViewModel(skillEntity);

            return new RequestViewModel<SkillViewModel>
            {
                Data = new List<SkillViewModel> { viewModel },
                Type = nameof(SkillViewModel),
                Status = "Success",
                Message = "Habilidade atualizada com sucesso"
            };
        }

        /// <summary>
        /// Remove uma habilidade
        /// </summary>
        /// <param name="id">ID da habilidade</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<SkillViewModel>> DeleteSkillAsync(Guid id)
        {
            var success = await _curriculumRepository.DeleteSkillAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao excluir habilidade");
            }

            return new RequestViewModel<SkillViewModel>
            {
                Data = new List<SkillViewModel>(),
                Type = nameof(SkillViewModel),
                Status = "Success",
                Message = "Habilidade excluída com sucesso"
            };
        }

        /// <summary>
        /// Lista as experiências profissionais
        /// </summary>
       
        /// <returns>Lista de experiências</returns>
        public async Task<RequestViewModel<ExperienceViewModel>> GetExperiencesAsync()
        {


            var experiences = await _curriculumRepository.GetExperiencesAsync(userId);
            var viewModels = experiences.Select(e => new ExperienceViewModel(e)).ToList();

            return new RequestViewModel<ExperienceViewModel>
            {
                Data = viewModels,
                Type = nameof(ExperienceViewModel),
                Status = "Success",
                Message = $"Encontradas {viewModels.Count} experiências"
            };
        }

        /// <summary>
        /// Cria uma nova experiência
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ExperienceViewModel>> CreateExperienceAsync(ExperienceViewModel experience)
        {
            if (experience == null)
            {
                throw new ArgumentNullException(nameof(experience));
            }



            if (experience.GuidID.HasValue && experience.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar uma experiência, não informe o ID");
            }

            var entity = experience.ToEntity();
            entity.UserID = userId;

            var result = await _curriculumRepository.CreateExperienceAsync(entity);
            var viewModel = new ExperienceViewModel(result);

            return new RequestViewModel<ExperienceViewModel>
            {
                Data = new List<ExperienceViewModel> { viewModel },
                Type = nameof(ExperienceViewModel),
                Status = "Success",
                Message = "Experiência criada com sucesso"
            };
        }

        /// <summary>
        /// Atualiza uma experiência existente
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ExperienceViewModel>> UpdateExperienceAsync(ExperienceViewModel experience)
        {
            if (experience == null)
            {
                throw new ArgumentNullException(nameof(experience));
            }

            if (!experience.GuidID.HasValue || experience.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar uma experiência, informe o ID");
            }

            var experienceEntity = await _curriculumRepository.GetExperienceByIdAsync(experience.GuidID.Value);
            if (experienceEntity == null)
            {
                throw new ValidationException("Experiência não encontrada");
            }

            // Atualizar a entidade existente
            experienceEntity.Title = experience.Title;
            experienceEntity.Company = experience.Company;
            experienceEntity.StartDate = experience.StartDate;
            experienceEntity.EndDate = experience.EndDate;
            experienceEntity.Location = experience.Location;
            experienceEntity.Description = experience.Description;
            experienceEntity.Responsibilities = experience.Responsibilities;
            experienceEntity.Technologies = experience.Technologies;
            experienceEntity.Achievements = experience.Achievements;
            experienceEntity.IsCurrentJob = experience.IsCurrentJob;
            experienceEntity.DisplayOrder = experience.DisplayOrder;
            experienceEntity.DateUpdated = DateTime.UtcNow;

            var success = await _curriculumRepository.UpdateExperienceAsync(experienceEntity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar experiência");
            }

            var viewModel = new ExperienceViewModel(experienceEntity);

            return new RequestViewModel<ExperienceViewModel>
            {
                Data = new List<ExperienceViewModel> { viewModel },
                Type = nameof(ExperienceViewModel),
                Status = "Success",
                Message = "Experiência atualizada com sucesso"
            };
        }

        /// <summary>
        /// Remove uma experiência
        /// </summary>
        /// <param name="id">ID da experiência</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ExperienceViewModel>> DeleteExperienceAsync(Guid id)
        {
            var success = await _curriculumRepository.DeleteExperienceAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao excluir experiência");
            }

            return new RequestViewModel<ExperienceViewModel>
            {
                Data = new List<ExperienceViewModel>(),
                Type = nameof(ExperienceViewModel),
                Status = "Success",
                Message = "Experiência excluída com sucesso"
            };
        }

        /// <summary>
        /// Lista a educação
        /// </summary>
       
        /// <returns>Lista de educação</returns>
        public async Task<RequestViewModel<EducationViewModel>> GetEducationAsync()
        {

            var education = await _curriculumRepository.GetEducationAsync(userId);
            var viewModels = education.Select(e => new EducationViewModel(e)).ToList();

            return new RequestViewModel<EducationViewModel>
            {
                Data = viewModels,
                Type = nameof(EducationViewModel),
                Status = "Success",
                Message = $"Encontradas {viewModels.Count} formações"
            };
        }

        /// <summary>
        /// Cria uma nova educação
        /// </summary>
        /// <param name="education">Dados da educação</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<EducationViewModel>> CreateEducationAsync(EducationViewModel education)
        {
            if (education == null)
            {
                throw new ArgumentNullException(nameof(education));
            }



            if (education.GuidID.HasValue && education.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar uma formação, não informe o ID");
            }

            var entity = education.ToEntity();
            entity.UserID = userId;

            var result = await _curriculumRepository.CreateEducationAsync(entity);
            var viewModel = new EducationViewModel(result);

            return new RequestViewModel<EducationViewModel>
            {
                Data = new List<EducationViewModel> { viewModel },
                Type = nameof(EducationViewModel),
                Status = "Success",
                Message = "Formação criada com sucesso"
            };
        }

        /// <summary>
        /// Atualiza uma educação existente
        /// </summary>
        /// <param name="education">Dados da educação</param>
       
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<EducationViewModel>> UpdateEducationAsync(EducationViewModel education)
        {
            if (education == null)
            {
                throw new ArgumentNullException(nameof(education));
            }

            if (!education.GuidID.HasValue || education.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar uma formação, informe o ID");
            }

            var educationEntity = await _curriculumRepository.GetEducationByIdAsync(education.GuidID.Value);
            if (educationEntity == null)
            {
                throw new ValidationException("Formação não encontrada");
            }

            // Atualizar a entidade existente
            educationEntity.Degree = education.Degree;
            educationEntity.Institution = education.Institution;
            educationEntity.StartDate = education.StartDate;
            educationEntity.EndDate = education.EndDate;
            educationEntity.Location = education.Location;
            educationEntity.Description = education.Description;
            educationEntity.Grade = education.Grade;
            educationEntity.Achievements = education.Achievements;
            educationEntity.DisplayOrder = education.DisplayOrder;
            educationEntity.DateUpdated = DateTime.UtcNow;

            var success = await _curriculumRepository.UpdateEducationAsync(educationEntity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar formação");
            }

            var viewModel = new EducationViewModel(educationEntity);

            return new RequestViewModel<EducationViewModel>
            {
                Data = new List<EducationViewModel> { viewModel },
                Type = nameof(EducationViewModel),
                Status = "Success",
                Message = "Formação atualizada com sucesso"
            };
        }

        /// <summary>
        /// Remove uma educação
        /// </summary>
        /// <param name="id">ID da educação</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<EducationViewModel>> DeleteEducationAsync(Guid id)
        {
            var success = await _curriculumRepository.DeleteEducationAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao excluir formação");
            }

            return new RequestViewModel<EducationViewModel>
            {
                Data = new List<EducationViewModel>(),
                Type = nameof(EducationViewModel),
                Status = "Success",
                Message = "Formação excluída com sucesso"
            };
        }

        /// <summary>
        /// Lista as certificações
        /// </summary>
        /// <returns>Lista de certificações</returns>
        public async Task<RequestViewModel<CertificationViewModel>> GetCertificationsAsync()
        {

            var certifications = await _curriculumRepository.GetCertificationsAsync(userId);
            var viewModels = certifications.Select(c => new CertificationViewModel(c)).ToList();

            return new RequestViewModel<CertificationViewModel>
            {
                Data = viewModels,
                Type = nameof(CertificationViewModel),
                Status = "Success",
                Message = $"Encontradas {viewModels.Count} certificações"
            };
        }

        /// <summary>
        /// Cria uma nova certificação
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<CertificationViewModel>> CreateCertificationAsync(CertificationViewModel certification)
        {
            if (certification == null)
            {
                throw new ArgumentNullException(nameof(certification));
            }



            if (certification.GuidID.HasValue && certification.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar uma certificação, não informe o ID");
            }

            var entity = certification.ToEntity();
            entity.UserID = userId;

            var result = await _curriculumRepository.CreateCertificationAsync(entity);
            var viewModel = new CertificationViewModel(result);

            return new RequestViewModel<CertificationViewModel>
            {
                Data = new List<CertificationViewModel> { viewModel },
                Type = nameof(CertificationViewModel),
                Status = "Success",
                Message = "Certificação criada com sucesso"
            };
        }

        /// <summary>
        /// Atualiza uma certificação existente
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<CertificationViewModel>> UpdateCertificationAsync(CertificationViewModel certification)
        {
            if (certification == null)
            {
                throw new ArgumentNullException(nameof(certification));
            }

            if (!certification.GuidID.HasValue || certification.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar uma certificação, informe o ID");
            }

            var certificationEntity = await _curriculumRepository.GetCertificationByIdAsync(certification.GuidID.Value);
            if (certificationEntity == null)
            {
                throw new ValidationException("Certificação não encontrada");
            }

            // Atualizar a entidade existente
            certificationEntity.Name = certification.Name;
            certificationEntity.Issuer = certification.Issuer;
            certificationEntity.IssueDate = certification.IssueDate;
            certificationEntity.ExpiryDate = certification.ExpiryDate;
            certificationEntity.CredentialId = certification.CredentialId;
            certificationEntity.CredentialUrl = certification.CredentialUrl;
            certificationEntity.Description = certification.Description;
            certificationEntity.DisplayOrder = certification.DisplayOrder;
            certificationEntity.DateUpdated = DateTime.UtcNow;

            var success = await _curriculumRepository.UpdateCertificationAsync(certificationEntity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar certificação");
            }

            var viewModel = new CertificationViewModel(certificationEntity);

            return new RequestViewModel<CertificationViewModel>
            {
                Data = new List<CertificationViewModel> { viewModel },
                Type = nameof(CertificationViewModel),
                Status = "Success",
                Message = "Certificação atualizada com sucesso"
            };
        }

        /// <summary>
        /// Remove uma certificação
        /// </summary>
        /// <param name="id">ID da certificação</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<CertificationViewModel>> DeleteCertificationAsync(Guid id)
        {
            var success = await _curriculumRepository.DeleteCertificationAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao excluir certificação");
            }

            return new RequestViewModel<CertificationViewModel>
            {
                Data = new List<CertificationViewModel>(),
                Type = nameof(CertificationViewModel),
                Status = "Success",
                Message = "Certificação excluída com sucesso"
            };
        }

        /// <summary>
        /// Lista os serviços
        /// </summary>
        /// <returns>Lista de serviços</returns>
        public async Task<RequestViewModel<ServiceViewModel>> GetServicesAsync()
        {

            var services = await _curriculumRepository.GetServicesAsync(userId);
            var viewModels = services.Select(s => new ServiceViewModel(s)).ToList();

            return new RequestViewModel<ServiceViewModel>
            {
                Data = viewModels,
                Type = nameof(ServiceViewModel),
                Status = "Success",
                Message = $"Encontrados {viewModels.Count} serviços"
            };
        }

        /// <summary>
        /// Cria um novo serviço
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ServiceViewModel>> CreateServiceAsync(ServiceViewModel service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }



            if (service.GuidID.HasValue && service.GuidID != Guid.Empty)
            {
                throw new ValidationException("Para criar um serviço, não informe o ID");
            }

            var entity = service.ToEntity();
            entity.UserID = userId;

            var result = await _curriculumRepository.CreateServiceAsync(entity);
            var viewModel = new ServiceViewModel(result);

            return new RequestViewModel<ServiceViewModel>
            {
                Data = new List<ServiceViewModel> { viewModel },
                Type = nameof(ServiceViewModel),
                Status = "Success",
                Message = "Serviço criado com sucesso"
            };
        }

        /// <summary>
        /// Atualiza um serviço existente
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ServiceViewModel>> UpdateServiceAsync(ServiceViewModel service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (!service.GuidID.HasValue || service.GuidID == Guid.Empty)
            {
                throw new ValidationException("Para atualizar um serviço, informe o ID");
            }

            var serviceEntity = await _curriculumRepository.GetServiceByIdAsync(service.GuidID.Value);
            if (serviceEntity == null)
            {
                throw new ValidationException("Serviço não encontrado");
            }

            // Atualizar a entidade existente
            serviceEntity.Title = service.Title;
            serviceEntity.Description = service.Description;
            serviceEntity.Icon = service.Icon;
            serviceEntity.Features = service.Features;
            serviceEntity.Price = service.Price;
            serviceEntity.Currency = service.Currency;
            serviceEntity.Duration = service.Duration;
            serviceEntity.DisplayOrder = service.DisplayOrder;
            serviceEntity.DateUpdated = DateTime.UtcNow;

            var success = await _curriculumRepository.UpdateServiceAsync(serviceEntity);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao atualizar serviço");
            }

            var viewModel = new ServiceViewModel(serviceEntity);

            return new RequestViewModel<ServiceViewModel>
            {
                Data = new List<ServiceViewModel> { viewModel },
                Type = nameof(ServiceViewModel),
                Status = "Success",
                Message = "Serviço atualizado com sucesso"
            };
        }

        /// <summary>
        /// Remove um serviço
        /// </summary>
        /// <param name="id">ID do serviço</param>
        /// <returns>Resultado da operação</returns>
        public async Task<RequestViewModel<ServiceViewModel>> DeleteServiceAsync(Guid id)
        {
            var success = await _curriculumRepository.DeleteServiceAsync(id);
            if (!success)
            {
                throw new InvalidOperationException("Erro ao excluir serviço");
            }

            return new RequestViewModel<ServiceViewModel>
            {
                Data = new List<ServiceViewModel>(),
                Type = nameof(ServiceViewModel),
                Status = "Success",
                Message = "Serviço excluído com sucesso"
            };
        }

        /// <summary>
        /// Obtém estatísticas do currículo
        /// </summary>
        /// <returns>Estatísticas do currículo</returns>
        public async Task<RequestViewModel<CurriculumStatsViewModel>> GetStatsAsync()
        {


            var skills = await GetSkillsAsync();
            var experiences = await GetExperiencesAsync();
            var education = await GetEducationAsync();
            var certifications = await GetCertificationsAsync();
            var services = await GetServicesAsync();

            var stats = new CurriculumStatsViewModel
            {
                TotalSkills = skills.Data.Count,
                TotalExperience = experiences.Data.Count,
                TotalEducation = education.Data.Count,
                TotalCertifications = certifications.Data.Count,
                TotalServices = services.Data.Count,
                SkillsByCategory = skills.Data.GroupBy(s => s.Category).ToDictionary(g => g.Key, g => g.Count()),
                AverageSkillLevel = skills.Data.Any() ? skills.Data.Average(s => s.Level) : 0
            };

            return new RequestViewModel<CurriculumStatsViewModel>
            {
                Data = new List<CurriculumStatsViewModel> { stats },
                Type = nameof(CurriculumStatsViewModel),
                Status = "Success",
                Message = "Estatísticas encontradas com sucesso"
            };
        }
    }
}