using Portfolium_Back.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolium_Back.Models
{
    /// <summary>
    /// Modelo para informações pessoais do currículo
    /// </summary>
    [Table("PersonalInfo")]
    public class PersonalInfo : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? ProfileImage { get; set; }

        public int YearsExperience { get; set; }
        public int ProjectsCompleted { get; set; }
        public int HappyClients { get; set; }
        public int Certifications { get; set; }

        [StringLength(255)]
        public string? LinkedInUrl { get; set; }

        [StringLength(255)]
        public string? GitHubUrl { get; set; }

        [StringLength(255)]
        public string? PortfolioUrl { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para habilidades técnicas
    /// </summary>
    public class Skill : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Level { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }

        [StringLength(7)]
        public string? Color { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para experiências profissionais
    /// </summary>
    public class Experience : Entity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Company { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? Responsibilities { get; set; } // JSON array
        public string? Technologies { get; set; } // JSON array
        public string? Achievements { get; set; } // JSON array

        public bool IsCurrentJob { get; set; }
        public int DisplayOrder { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para educação
    /// </summary>
    [Table("Education")]
    public class Education : Entity
    {
        [Required]
        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Institution { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(10)]
        public string? Grade { get; set; }

        public string? Achievements { get; set; } // JSON array

        public int DisplayOrder { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para certificações
    /// </summary>
    public class Certification : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Issuer { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [StringLength(100)]
        public string? CredentialId { get; set; }

        [StringLength(255)]
        public string? CredentialUrl { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para serviços oferecidos
    /// </summary>
    public class Service : Entity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Icon { get; set; }

        public string? Features { get; set; } // JSON array

        public decimal? Price { get; set; }

        [StringLength(3)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? Duration { get; set; }

        public int DisplayOrder { get; set; }

        // Relacionamento com usuário
        public Guid UserID { get; set; }
    }

    /// <summary>
    /// Modelo para mensagens de contato
    /// </summary>
    public class ContactMessage : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseAt { get; set; }

        [StringLength(15)]
        public string? IpAddress { get; set; }

        [StringLength(200)]
        public string? UserAgent { get; set; }
    }
} 