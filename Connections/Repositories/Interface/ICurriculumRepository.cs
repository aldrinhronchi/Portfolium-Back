using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Repositories.Interface
{
    public interface ICurriculumRepository
    {
        // PersonalInfo
        Task<PersonalInfo?> GetPersonalInfoAsync(Guid userId);
        Task<PersonalInfo> CreatePersonalInfoAsync(PersonalInfo personalInfo);
        Task<bool> UpdatePersonalInfoAsync(PersonalInfo personalInfo);

        // Skills
        Task<List<Skill>> GetSkillsAsync(Guid userId, string? category = null);
        Task<Skill?> GetSkillByIdAsync(Guid skillId);
        Task<Skill> CreateSkillAsync(Skill skill);
        Task<bool> UpdateSkillAsync(Skill skill);
        Task<bool> DeleteSkillAsync(Guid skillId);

        // Experiences
        Task<List<Experience>> GetExperiencesAsync(Guid userId);
        Task<Experience?> GetExperienceByIdAsync(Guid experienceId);
        Task<Experience> CreateExperienceAsync(Experience experience);
        Task<bool> UpdateExperienceAsync(Experience experience);
        Task<bool> DeleteExperienceAsync(Guid experienceId);

        // Education
        Task<List<Education>> GetEducationAsync(Guid userId);
        Task<Education?> GetEducationByIdAsync(Guid educationId);
        Task<Education> CreateEducationAsync(Education education);
        Task<bool> UpdateEducationAsync(Education education);
        Task<bool> DeleteEducationAsync(Guid educationId);

        // Certifications
        Task<List<Certification>> GetCertificationsAsync(Guid userId);
        Task<Certification?> GetCertificationByIdAsync(Guid certificationId);
        Task<Certification> CreateCertificationAsync(Certification certification);
        Task<bool> UpdateCertificationAsync(Certification certification);
        Task<bool> DeleteCertificationAsync(Guid certificationId);

        // Services
        Task<List<Service>> GetServicesAsync(Guid userId);
        Task<Service?> GetServiceByIdAsync(Guid serviceId);
        Task<Service> CreateServiceAsync(Service service);
        Task<bool> UpdateServiceAsync(Service service);
        Task<bool> DeleteServiceAsync(Guid serviceId);
    }
} 