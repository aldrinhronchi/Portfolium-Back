using Portfolium_Back.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Portfolium_Back.Models
{
    /// <summary>
    /// Modelo representando um projeto do portfólio
    /// </summary>
    public class Project : Entity
    {
        // GuidID já está herdado de Entity

        /// <summary>
        /// Nome/Título do projeto
        /// </summary>
        [Required]
        [StringLength(200)]
        public String Name { get; set; }

        /// <summary>
        /// Descrição detalhada do projeto
        /// </summary>
        [StringLength(2000)]
        public String? Description { get; set; }

        /// <summary>
        /// Descrição curta para exibição em cards
        /// </summary>
        [StringLength(500)]
        public String? ShortDescription { get; set; }

        /// <summary>
        /// Tecnologias utilizadas no projeto (JSON array ou string separada por vírgulas)
        /// </summary>
        [StringLength(1000)]
        public String? Technologies { get; set; }

        /// <summary>
        /// Link para o projeto (GitHub, site, etc.)
        /// </summary>
        [StringLength(500)]
        public String? ProjectUrl { get; set; }

        /// <summary>
        /// Link para demonstração/preview do projeto
        /// </summary>
        [StringLength(500)]
        public String? DemoUrl { get; set; }

        /// <summary>
        /// Link para repositório do código
        /// </summary>
        [StringLength(500)]
        public String? RepositoryUrl { get; set; }

        /// <summary>
        /// Imagem principal do projeto (URL ou path)
        /// </summary>
        [StringLength(500)]
        public String? MainImage { get; set; }

        /// <summary>
        /// Imagens adicionais do projeto (JSON array ou string separada por vírgulas)
        /// </summary>
        [StringLength(2000)]
        public String? AdditionalImages { get; set; }

        /// <summary>
        /// Status do projeto (Em Desenvolvimento, Concluído, Arquivado, etc.)
        /// </summary>
        [StringLength(50)]
        public String? Status { get; set; }

        /// <summary>
        /// Categoria do projeto (Web, Mobile, Desktop, etc.)
        /// </summary>
        [StringLength(100)]
        public String? Category { get; set; }

        /// <summary>
        /// Prioridade para ordenação na exibição
        /// </summary>
        public Int32 DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indica se o projeto deve ser destacado na página inicial
        /// </summary>
        public Boolean IsFeatured { get; set; } = false;

        /// <summary>
        /// Data de início do projeto
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Data de conclusão do projeto
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ID do usuário proprietário do projeto
        /// </summary>
        public Int32? UserID { get; set; }

        /// <summary>
        /// Referência de navegação para o usuário
        /// </summary>
        public virtual User? User { get; set; }
    }
} 