using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Services.Interfaces
{
    /// <summary>
    /// Interface para serviço de projetos do portfólio
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Buscar projetos com paginação e filtros
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        /// <summary>
        /// Buscar projeto por ID
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> GetByIdAsync(String id);

        /// <summary>
        /// Buscar projeto por GUID
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> GetByGuidAsync(Guid guidId);

        /// <summary>
        /// Criar um novo projeto
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> CreateAsync(ProjectViewModel projectViewModel);

        /// <summary>
        /// Criar um novo projeto 
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> CreateAsync(ProjectViewModel projectViewModel, Guid userId);

        /// <summary>
        /// Atualizar um projeto existente
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> UpdateAsync(ProjectViewModel projectViewModel);

        /// <summary>
        /// Excluir projeto (soft delete)
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> DeleteAsync(String id);

        /// <summary>
        /// Buscar projetos em destaque
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> GetFeaturedAsync(int? limit = null);

        /// <summary>
        /// Buscar projetos por categoria
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> GetByCategoryAsync(string category);

        /// <summary>
        /// Buscar categorias únicas
        /// </summary>
        Task<RequestViewModel<string>> GetCategoriesAsync();

        /// <summary>
        /// Buscar tecnologias únicas
        /// </summary>
        Task<RequestViewModel<string>> GetTechnologiesAsync();

        /// <summary>
        /// Obter estatísticas dos projetos
        /// </summary>
        Task<RequestViewModel<ProjectStatsViewModel>> GetStatsAsync();

        /// <summary>
        /// Ativar/Desativar projeto
        /// </summary>
        Task<RequestViewModel<ProjectViewModel>> ToggleStatusAsync(int id, string userInfo);
    }
} 