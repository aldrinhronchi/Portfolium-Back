using System.ComponentModel.DataAnnotations;

namespace Portfolium_Back.Models.ViewModels
{
    /// <summary>
    /// ViewModel unificada para operações com projetos
    /// </summary>
    public class ProjectViewModel : IViewModel<Project, ProjectViewModel>
    {
        // Propriedades de identificação
        //public Int32? ID { get; set; }
        public Guid? GuidID { get; set; }

        // Propriedades do projeto
        [Required(ErrorMessage = "Nome do projeto é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public String Name { get; set; }

        [StringLength(2000, ErrorMessage = "Descrição deve ter no máximo 2000 caracteres")]
        public String? Description { get; set; }

        [StringLength(500, ErrorMessage = "Descrição curta deve ter no máximo 500 caracteres")]
        public String? ShortDescription { get; set; }

        [StringLength(1000, ErrorMessage = "Tecnologias deve ter no máximo 1000 caracteres")]
        public String? Technologies { get; set; }

        // URLs do projeto
        [StringLength(500, ErrorMessage = "URL do projeto deve ter no máximo 500 caracteres")]
        public String? ProjectUrl { get; set; }

        [StringLength(500, ErrorMessage = "URL da demo deve ter no máximo 500 caracteres")]
        public String? DemoUrl { get; set; }

        [StringLength(500, ErrorMessage = "URL do repositório deve ter no máximo 500 caracteres")]
        public String? RepositoryUrl { get; set; }

        // Imagens
        [StringLength(500, ErrorMessage = "URL da imagem principal deve ter no máximo 500 caracteres")]
        public String? MainImage { get; set; }

        [StringLength(2000, ErrorMessage = "Imagens adicionais devem ter no máximo 2000 caracteres")]
        public String? AdditionalImages { get; set; }

        // Configurações do projeto
        [StringLength(50, ErrorMessage = "Status deve ter no máximo 50 caracteres")]
        public String? Status { get; set; }

        [StringLength(100, ErrorMessage = "Categoria deve ter no máximo 100 caracteres")]
        public String? Category { get; set; }

        public Int32 DisplayOrder { get; set; } = 0;
        public Boolean IsFeatured { get; set; } = false;

        // Datas
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Relacionamentos
        public String? UserID { get; set; }
        public String? UserName { get; set; }

        // Propriedades de auditoria (somente para respostas)
        public Boolean? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public String? UserCreated { get; set; }
        public String? UserUpdated { get; set; }

        // Construtores
        public ProjectViewModel() { }

        /// <summary>
        /// Construtor para criar ViewModel a partir da entidade Project
        /// </summary>
        /// <param name="project">Entidade Project</param>
        public ProjectViewModel(Project project)
        {
            //ID = project.ID;
            GuidID = project.GuidID;
            Name = project.Name;
            Description = project.Description;
            ShortDescription = project.ShortDescription;
            Technologies = project.Technologies;
            ProjectUrl = project.ProjectUrl;
            DemoUrl = project.DemoUrl;
            RepositoryUrl = project.RepositoryUrl;
            MainImage = project.MainImage;
            AdditionalImages = project.AdditionalImages;
            Status = project.Status;
            Category = project.Category;
            DisplayOrder = project.DisplayOrder;
            IsFeatured = project.IsFeatured;
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            UserID = project.User?.GuidID.ToString();
            UserName = project.User?.Name;
            IsActive = project.IsActive;
            DateCreated = project.DateCreated;
            DateUpdated = project.DateUpdated;
            UserCreated = project.UserCreated;
            UserUpdated = project.UserUpdated;
        }

        /// <summary>
        /// Converte ViewModel para entidade Project
        /// </summary>
        /// <returns>Entidade Project</returns>
        public Project ToEntity()
        {
            return new Project
            {
                //ID = ID ?? 0,
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Description = Description,
                ShortDescription = ShortDescription,
                Technologies = Technologies,
                ProjectUrl = ProjectUrl,
                DemoUrl = DemoUrl,
                RepositoryUrl = RepositoryUrl,
                MainImage = MainImage,
                AdditionalImages = AdditionalImages,
                Status = Status,
                Category = Category,
                DisplayOrder = DisplayOrder,
                IsFeatured = IsFeatured,
                StartDate = StartDate,
                EndDate = EndDate,
                UserID = null,
            };
        }

        public ProjectViewModel ToViewModel(Project entity)
        {
            return new ProjectViewModel(entity);
        }
    }

    /// <summary>
    /// ViewModel para filtros de listagem de projetos
    /// </summary>
    public class ProjectFilterViewModel
    {
        public String? Name { get; set; }
        public String? Category { get; set; }
        public String? Status { get; set; }
        public Boolean? IsFeatured { get; set; }
        public Boolean? IsActive { get; set; }
        public Int32? UserID { get; set; }
        public Int32 Page { get; set; } = 1;
        public Int32 PageSize { get; set; } = 10;
        public String? OrderBy { get; set; } = "DisplayOrder";
        public String? OrderDirection { get; set; } = "ASC";
    }

    /// <summary>
    /// ViewModel para estatísticas de projetos (single-user system)
    /// </summary>
    public class ProjectStatsViewModel
    {
        public Int32 TotalProjects { get; set; }
        public Int32 FeaturedProjects { get; set; }
        public Int32 CategoriesCount { get; set; }
        public DateTime? LastUpdate { get; set; }
        
        // Legacy properties (mantidas para compatibilidade)
        public Int32 Total { get => TotalProjects; set => TotalProjects = value; }
        public Int32 Active { get => TotalProjects; set => TotalProjects = value; }
        public Int32 Featured { get => FeaturedProjects; set => FeaturedProjects = value; }
        public Dictionary<String, Int32> ByStatus { get; set; } = new();
        public Dictionary<String, Int32> ByCategory { get; set; } = new();
    }
} 