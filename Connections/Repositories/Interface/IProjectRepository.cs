using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Connections.Repositories.Interface
{
    /// <summary>
    /// Interface para repositório de projetos com métodos específicos
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Buscar projetos com paginação e filtros
        /// </summary>
        Task<RequestViewModel<Project>> GetProjectsPagedAsync(ProjectFilterViewModel filters);

        /// <summary>
        /// Buscar projetos em destaque
        /// </summary>
        Task<List<Project>> GetFeaturedProjectsAsync(int? limit = null);

        /// <summary>
        /// Buscar projetos por categoria
        /// </summary>
        Task<List<Project>> GetProjectsByCategoryAsync(string category);

        /// <summary>
        /// Buscar projetos por usuário
        /// </summary>
        Task<List<Project>> GetProjectsByUserAsync(int userId);

        /// <summary>
        /// Buscar projeto por GUID
        /// </summary>
        Task<Project?> GetProjectByGuidAsync(Guid guidId);

        /// <summary>
        /// Buscar categorias únicas de projetos
        /// </summary>
        Task<List<string>> GetCategoriesAsync();

        /// <summary>
        /// Buscar tecnologias únicas utilizadas nos projetos
        /// </summary>
        Task<List<string>> GetTechnologiesAsync();

        /// <summary>
        /// Contar projetos por status
        /// </summary>
        Task<Dictionary<string, int>> GetProjectCountByStatusAsync();
    }
} 