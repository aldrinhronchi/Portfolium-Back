using Microsoft.EntityFrameworkCore;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Context;
using Portfolium_Back.Models;

namespace Portfolium_Back.Connections.Repositories
{
    public class CurriculumRepository : ICurriculumRepository
    {
        private readonly PortfoliumContext _context;

        public CurriculumRepository(PortfoliumContext context)
        {
            _context = context;
        }

        #region PersonalInfo

        public async Task<PersonalInfo?> GetPersonalInfoAsync(Guid userId)
        {
            return await _context.PersonalInfos
                .Where(p => p.UserID == userId && p.IsActive)
                .OrderByDescending(p => p.DateUpdated)
                .FirstOrDefaultAsync();
        }

        public async Task<PersonalInfo> CreatePersonalInfoAsync(PersonalInfo personalInfo)
        {
            personalInfo.GuidID = Guid.NewGuid();
            personalInfo.DateCreated = DateTime.UtcNow;
            personalInfo.DateUpdated = DateTime.UtcNow;
            personalInfo.IsActive = true;

            _context.PersonalInfos.Add(personalInfo);
            await _context.SaveChangesAsync();

            return personalInfo;
        }

        public async Task<bool> UpdatePersonalInfoAsync(PersonalInfo personalInfo)
        {
            personalInfo.DateUpdated = DateTime.UtcNow;

            _context.PersonalInfos.Update(personalInfo);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        #endregion

        #region Skills

        public async Task<List<Skill>> GetSkillsAsync(Guid userId, string? category = null)
        {
            var query = _context.Skills
                .Where(s => s.UserID == userId && s.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            return await query
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            skill.GuidID = Guid.NewGuid();
            skill.DateCreated = DateTime.UtcNow;
            skill.DateUpdated = DateTime.UtcNow;
            skill.IsActive = true;

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        public async Task<bool> UpdateSkillAsync(Skill skill)
        {
            skill.DateUpdated = DateTime.UtcNow;

            _context.Skills.Update(skill);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Skill?> GetSkillByIdAsync(Guid skillId)
        {
            return await _context.Skills
                .Where(s => s.GuidID == skillId && s.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteSkillAsync(Guid skillId)
        {
            var skill = await _context.Skills.FindAsync(skillId);
            if (skill == null) return false;

            skill.IsActive = false;
            skill.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        #endregion

        #region Experiences

        public async Task<List<Experience>> GetExperiencesAsync(Guid userId)
        {
            return await _context.Experiences
                .Where(e => e.UserID == userId && e.IsActive)
                .OrderBy(e => e.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Experience> CreateExperienceAsync(Experience experience)
        {
            experience.GuidID = Guid.NewGuid();
            experience.DateCreated = DateTime.UtcNow;
            experience.DateUpdated = DateTime.UtcNow;
            experience.IsActive = true;

            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();

            return experience;
        }

        public async Task<bool> UpdateExperienceAsync(Experience experience)
        {
            experience.DateUpdated = DateTime.UtcNow;

            _context.Experiences.Update(experience);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Experience?> GetExperienceByIdAsync(Guid experienceId)
        {
            return await _context.Experiences
                .Where(e => e.GuidID == experienceId && e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteExperienceAsync(Guid experienceId)
        {
            var experience = await _context.Experiences.FindAsync(experienceId);
            if (experience == null) return false;

            experience.IsActive = false;
            experience.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        #endregion

        #region Education

        public async Task<List<Education>> GetEducationAsync(Guid userId)
        {
            return await _context.Educations
                .Where(e => e.UserID == userId && e.IsActive)
                .OrderBy(e => e.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Education> CreateEducationAsync(Education education)
        {
            education.GuidID = Guid.NewGuid();
            education.DateCreated = DateTime.UtcNow;
            education.DateUpdated = DateTime.UtcNow;
            education.IsActive = true;

            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return education;
        }

        public async Task<bool> UpdateEducationAsync(Education education)
        {
            education.DateUpdated = DateTime.UtcNow;

            _context.Educations.Update(education);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Education?> GetEducationByIdAsync(Guid educationId)
        {
            return await _context.Educations
                .Where(e => e.GuidID == educationId && e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteEducationAsync(Guid educationId)
        {
            var education = await _context.Educations.FindAsync(educationId);
            if (education == null) return false;

            education.IsActive = false;
            education.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        #endregion

        #region Certifications

        public async Task<List<Certification>> GetCertificationsAsync(Guid userId)
        {
            return await _context.Certifications
                .Where(c => c.UserID == userId && c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Certification> CreateCertificationAsync(Certification certification)
        {
            certification.GuidID = Guid.NewGuid();
            certification.DateCreated = DateTime.UtcNow;
            certification.DateUpdated = DateTime.UtcNow;
            certification.IsActive = true;

            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();

            return certification;
        }

        public async Task<bool> UpdateCertificationAsync(Certification certification)
        {
            certification.DateUpdated = DateTime.UtcNow;

            _context.Certifications.Update(certification);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Certification?> GetCertificationByIdAsync(Guid certificationId)
        {
            return await _context.Certifications
                .Where(c => c.GuidID == certificationId && c.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCertificationAsync(Guid certificationId)
        {
            var certification = await _context.Certifications.FindAsync(certificationId);
            if (certification == null) return false;

            certification.IsActive = false;
            certification.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        #endregion

        #region Services

        public async Task<List<Service>> GetServicesAsync(Guid userId)
        {
            return await _context.Services
                .Where(s => s.UserID == userId && s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            service.GuidID = Guid.NewGuid();
            service.DateCreated = DateTime.UtcNow;
            service.DateUpdated = DateTime.UtcNow;
            service.IsActive = true;

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<bool> UpdateServiceAsync(Service service)
        {
            service.DateUpdated = DateTime.UtcNow;

            _context.Services.Update(service);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Service?> GetServiceByIdAsync(Guid serviceId)
        {
            return await _context.Services
                .Where(s => s.GuidID == serviceId && s.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteServiceAsync(Guid serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null) return false;

            service.IsActive = false;
            service.DateUpdated = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        #endregion
    }
} 