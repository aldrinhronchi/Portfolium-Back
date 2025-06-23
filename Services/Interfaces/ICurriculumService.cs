using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    /// <summary>
    /// Interface para serviços de currículo
    /// </summary>
    public interface ICurriculumService
    {
        /// <summary>
        /// Obtém o currículo completo
        /// </summary>
        /// <returns>Dados completos do currículo</returns>
        Task<RequestViewModel<CurriculumViewModel>> GetCurriculumAsync();

        /// <summary>
        /// Obtém as informações pessoais
        /// </summary>
        /// <returns>Informações pessoais</returns>
        Task<RequestViewModel<PersonalInfoViewModel>> GetPersonalInfoAsync();

        /// <summary>
        /// Cria ou atualiza as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<PersonalInfoViewModel>> CreatePersonalInfoAsync(PersonalInfoViewModel personalInfo);

        /// <summary>
        /// Cria ou atualiza as informações pessoais
        /// </summary>
        /// <param name="personalInfo">Dados das informações pessoais</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<PersonalInfoViewModel>> UpdatePersonalInfoAsync(PersonalInfoViewModel personalInfo);

        /// <summary>
        /// Lista as habilidades
        /// </summary>
        /// <param name="category">Categoria específica</param>
        /// <returns>Lista de habilidades</returns>
        Task<RequestViewModel<SkillViewModel>> GetSkillsAsync(string? category = null);

        /// <summary>
        /// Cria ou atualiza uma habilidade
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<SkillViewModel>> CreateSkillAsync(SkillViewModel skill);

        /// <summary>
        /// Cria ou atualiza uma habilidade
        /// </summary>
        /// <param name="skill">Dados da habilidade</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<SkillViewModel>> UpdateSkillAsync(SkillViewModel skill);

        /// <summary>
        /// Remove uma habilidade
        /// </summary>
        /// <param name="id">ID da habilidade</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<SkillViewModel>> DeleteSkillAsync(Guid id);

        /// <summary>
        /// Lista as experiências profissionais
        /// </summary>
        /// <returns>Lista de experiências</returns>
        Task<RequestViewModel<ExperienceViewModel>> GetExperiencesAsync();

        /// <summary>
        /// Cria ou atualiza uma experiência
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
            Task<RequestViewModel<ExperienceViewModel>> CreateExperienceAsync(ExperienceViewModel experience);

        /// <summary>
        /// Cria ou atualiza uma experiência
        /// </summary>
        /// <param name="experience">Dados da experiência</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ExperienceViewModel>> UpdateExperienceAsync(ExperienceViewModel experience);

        /// <summary>
        /// Remove uma experiência
        /// </summary>
        /// <param name="id">ID da experiência</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ExperienceViewModel>> DeleteExperienceAsync(Guid id);

        /// <summary>
        /// Lista a educação
        /// </summary>
        /// <returns>Lista de educação</returns>
        Task<RequestViewModel<EducationViewModel>> GetEducationAsync();

        /// <summary>
        /// Cria ou atualiza uma educação
        /// </summary>
        /// <param name="education">Dados da educação</param>       
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<EducationViewModel>> CreateEducationAsync(EducationViewModel education);

        /// <summary>
        /// Cria ou atualiza uma educação
        /// </summary>
        /// <param name="education">Dados da educação</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<EducationViewModel>> UpdateEducationAsync(EducationViewModel education);

        /// <summary>
        /// Remove uma educação
        /// </summary>
        /// <param name="id">ID da educação</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<EducationViewModel>> DeleteEducationAsync(Guid id);

        /// <summary>
        /// Lista as certificações
        /// </summary>
        /// <returns>Lista de certificações</returns>
        Task<RequestViewModel<CertificationViewModel>> GetCertificationsAsync();

        /// <summary>
        /// Cria ou atualiza uma certificação
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<CertificationViewModel>> CreateCertificationAsync(CertificationViewModel certification);

        /// <summary>
        /// Cria ou atualiza uma certificação
        /// </summary>
        /// <param name="certification">Dados da certificação</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<CertificationViewModel>> UpdateCertificationAsync(CertificationViewModel certification);

        /// <summary>
        /// Remove uma certificação
        /// </summary>
        /// <param name="id">ID da certificação</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<CertificationViewModel>> DeleteCertificationAsync(Guid id);

        /// <summary>
        /// Lista os serviços
        /// </summary>
        /// <returns>Lista de serviços</returns>
        Task<RequestViewModel<ServiceViewModel>> GetServicesAsync();

        /// <summary>
        /// Cria ou atualiza um serviço
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ServiceViewModel>> CreateServiceAsync(ServiceViewModel service);

        /// <summary>
        /// Cria ou atualiza um serviço
        /// </summary>
        /// <param name="service">Dados do serviço</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ServiceViewModel>> UpdateServiceAsync(ServiceViewModel service);

        /// <summary>
        /// Remove um serviço
        /// </summary>
        /// <param name="id">ID do serviço</param>
        /// <returns>Resultado da operação</returns>
        Task<RequestViewModel<ServiceViewModel>> DeleteServiceAsync(Guid id);

        /// <summary>
        /// Obtém estatísticas do currículo
        /// </summary>
        /// <returns>Estatísticas do currículo</returns>
        Task<RequestViewModel<CurriculumStatsViewModel>> GetStatsAsync();
    }
} 