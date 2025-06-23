using System.ComponentModel.DataAnnotations;

namespace Portfolium_Back.Models.ViewModels
{
    /// <summary>
    /// ViewModel para informações pessoais
    /// </summary>
    public class PersonalInfoViewModel : IViewModel<PersonalInfo, PersonalInfoViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }

        public int YearsExperience { get; set; }
        public int ProjectsCompleted { get; set; }
        public int HappyClients { get; set; }
        public int Certifications { get; set; }

        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }

        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public PersonalInfoViewModel() { }

        public PersonalInfoViewModel(PersonalInfo personalInfo)
        {
            GuidID = personalInfo.GuidID;
            Name = personalInfo.Name;
            Title = personalInfo.Title;
            Description = personalInfo.Description;
            Location = personalInfo.Location;
            Phone = personalInfo.Phone;
            Email = personalInfo.Email;
            ProfileImage = personalInfo.ProfileImage;
            YearsExperience = personalInfo.YearsExperience;
            ProjectsCompleted = personalInfo.ProjectsCompleted;
            HappyClients = personalInfo.HappyClients;
            Certifications = personalInfo.Certifications;
            LinkedInUrl = personalInfo.LinkedInUrl;
            GitHubUrl = personalInfo.GitHubUrl;
            PortfolioUrl = personalInfo.PortfolioUrl;
            IsActive = personalInfo.IsActive;
            DateCreated = personalInfo.DateCreated;
            DateUpdated = personalInfo.DateUpdated;
        }
        public PersonalInfo ToEntity()
        {
            return new PersonalInfo
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Title = Title,
                Description = Description,
                Location = Location,
                Phone = Phone,
                Email = Email,
                ProfileImage = ProfileImage,
                YearsExperience = YearsExperience,
                ProjectsCompleted = ProjectsCompleted,
                HappyClients = HappyClients,
                Certifications = Certifications,
                LinkedInUrl = LinkedInUrl,
                GitHubUrl = GitHubUrl,
                PortfolioUrl = PortfolioUrl,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public PersonalInfoViewModel ToViewModel(PersonalInfo entity)
        {
            return new PersonalInfoViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para habilidades
    /// </summary>
    public class SkillViewModel : IViewModel<Skill, SkillViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Level { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public SkillViewModel() { }

        public SkillViewModel(Skill skill)
        {
            GuidID = skill.GuidID;
            Name = skill.Name;
            Level = skill.Level;
            Category = skill.Category;
            DisplayOrder = skill.DisplayOrder;
            Icon = skill.Icon;
            Color = skill.Color;
            IsActive = skill.IsActive;
            DateCreated = skill.DateCreated;
            DateUpdated = skill.DateUpdated;
        }
        public Skill ToEntity()
        {
            return new Skill
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Level = Level,
                Category = Category,
                DisplayOrder = DisplayOrder,
                Icon = Icon,
                Color = Color,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public SkillViewModel ToViewModel(Skill entity)
        {
            return new SkillViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para experiências
    /// </summary>
    public class ExperienceViewModel : IViewModel<Experience, ExperienceViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Company { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Responsibilities { get; set; } // JSON array
        public string? Technologies { get; set; } // JSON array
        public string? Achievements { get; set; } // JSON array
        public bool IsCurrentJob { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public ExperienceViewModel() { }

        public ExperienceViewModel(Experience experience)
        {
            GuidID = experience.GuidID;
            Title = experience.Title;
            Company = experience.Company;
            StartDate = experience.StartDate;
            EndDate = experience.EndDate;
            Location = experience.Location;
            Description = experience.Description;
            Responsibilities = experience.Responsibilities;
            Technologies = experience.Technologies;
            Achievements = experience.Achievements;
            IsCurrentJob = experience.IsCurrentJob;
            DisplayOrder = experience.DisplayOrder;
            IsActive = experience.IsActive;
            DateCreated = experience.DateCreated;
            DateUpdated = experience.DateUpdated;
        }
        public Experience ToEntity()
        {
            return new Experience
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Title = Title,
                Company = Company,
                StartDate = StartDate,
                EndDate = EndDate,
                Location = Location,
                Description = Description,
                Responsibilities = Responsibilities,
                Technologies = Technologies,
                Achievements = Achievements,
                IsCurrentJob = IsCurrentJob,
                DisplayOrder = DisplayOrder,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public ExperienceViewModel ToViewModel(Experience entity)
        {
            return new ExperienceViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para educação
    /// </summary>
    public class EducationViewModel : IViewModel<Education, EducationViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public string Institution { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Grade { get; set; }
        public string? Achievements { get; set; } // JSON array
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public EducationViewModel() { }

        public EducationViewModel(Education education)
        {
            GuidID = education.GuidID;
            Degree = education.Degree;
            Institution = education.Institution;
            StartDate = education.StartDate;
            EndDate = education.EndDate;
            Location = education.Location;
            Description = education.Description;
            Grade = education.Grade;
            Achievements = education.Achievements;
            DisplayOrder = education.DisplayOrder;
            IsActive = education.IsActive;
            DateCreated = education.DateCreated;
            DateUpdated = education.DateUpdated;
        }
        public Education ToEntity()
        {
            return new Education
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Degree = Degree,
                Institution = Institution,
                StartDate = StartDate,
                EndDate = EndDate,
                Location = Location,
                Description = Description,
                Grade = Grade,
                Achievements = Achievements,
                DisplayOrder = DisplayOrder,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public EducationViewModel ToViewModel(Education entity)
        {
            return new EducationViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para certificações
    /// </summary>
    public class CertificationViewModel : IViewModel<Certification, CertificationViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public CertificationViewModel() { }

        public CertificationViewModel(Certification certification)
        {
            GuidID = certification.GuidID;
            Name = certification.Name;
            Issuer = certification.Issuer;
            IssueDate = certification.IssueDate;
            ExpiryDate = certification.ExpiryDate;
            CredentialId = certification.CredentialId;
            CredentialUrl = certification.CredentialUrl;
            Description = certification.Description;
            DisplayOrder = certification.DisplayOrder;
            IsActive = certification.IsActive;
            DateCreated = certification.DateCreated;
            DateUpdated = certification.DateUpdated;
        }
        public Certification ToEntity()
        {
            return new Certification
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Issuer = Issuer,
                IssueDate = IssueDate,
                ExpiryDate = ExpiryDate,
                CredentialId = CredentialId,
                CredentialUrl = CredentialUrl,
                Description = Description,
                DisplayOrder = DisplayOrder,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public CertificationViewModel ToViewModel(Certification entity)
        {
            return new CertificationViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para serviços
    /// </summary>
    public class ServiceViewModel : IViewModel<Service, ServiceViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Features { get; set; } // JSON array
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public string? Duration { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public ServiceViewModel() { }

        public ServiceViewModel(Service service)
        {
            GuidID = service.GuidID;
            Title = service.Title;
            Description = service.Description;
            Icon = service.Icon;
            Features = service.Features;
            Price = service.Price;
            Currency = service.Currency;
            Duration = service.Duration;
            DisplayOrder = service.DisplayOrder;
            IsActive = service.IsActive;
            DateCreated = service.DateCreated;
            DateUpdated = service.DateUpdated;
        }
        public Service ToEntity()
        {
            return new Service
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Title = Title,
                Description = Description,
                Icon = Icon,
                Features = Features,
                Price = Price,
                Currency = Currency,
                Duration = Duration,
                DisplayOrder = DisplayOrder,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public ServiceViewModel ToViewModel(Service entity)
        {
            return new ServiceViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para mensagens de contato
    /// </summary>
    public class ContactMessageViewModel : IViewModel<ContactMessage, ContactMessageViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseAt { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public ContactMessageViewModel() { }

        public ContactMessageViewModel(ContactMessage contactMessage)
        {
            GuidID = contactMessage.GuidID;
            Name = contactMessage.Name;
            Email = contactMessage.Email;
            Subject = contactMessage.Subject;
            Message = contactMessage.Message;
            IsRead = contactMessage.IsRead;
            ReadAt = contactMessage.ReadAt;
            Response = contactMessage.Response;
            ResponseAt = contactMessage.ResponseAt;
            IpAddress = contactMessage.IpAddress;
            UserAgent = contactMessage.UserAgent;
            IsActive = contactMessage.IsActive;
            DateCreated = contactMessage.DateCreated;
            DateUpdated = contactMessage.DateUpdated;
        }
        public ContactMessage ToEntity()
        {
            return new ContactMessage
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Email = Email,
                Subject = Subject,
                Message = Message,
                IsRead = IsRead,
                ReadAt = ReadAt,
                Response = Response,
                ResponseAt = ResponseAt,
                IpAddress = IpAddress,
                UserAgent = UserAgent,
                IsActive = IsActive,
                DateCreated = DateCreated ?? DateTime.UtcNow,
                DateUpdated = DateUpdated ?? DateTime.UtcNow
            };
        }
        public ContactMessageViewModel ToViewModel(ContactMessage entity)
        {
            return new ContactMessageViewModel(entity);
        }

      
    }

    /// <summary>
    /// ViewModel para dados completos do currículo
    /// </summary>
    public class CurriculumViewModel 
    {
        public PersonalInfoViewModel? PersonalInfo { get; set; }
        public List<SkillViewModel> Skills { get; set; } = new List<SkillViewModel>();
        public List<ExperienceViewModel> Experiences { get; set; } = new List<ExperienceViewModel>();
        public List<EducationViewModel> Education { get; set; } = new List<EducationViewModel>();
        public List<CertificationViewModel> Certifications { get; set; } = new List<CertificationViewModel>();
        public List<ServiceViewModel> Services { get; set; } = new List<ServiceViewModel>();
       
    }

    /// <summary>
    /// ViewModel para estatísticas do currículo
    /// </summary>
    public class CurriculumStatsViewModel
    {
        public int TotalSkills { get; set; }
        public double AverageSkillLevel { get; set; }
        public int TotalExperience { get; set; } // meses
        public int TotalEducation { get; set; }
        public int TotalCertifications { get; set; }
        public int TotalServices { get; set; }
        public Dictionary<string, int> SkillsByCategory { get; set; } = new Dictionary<string, int>();

    }
} 