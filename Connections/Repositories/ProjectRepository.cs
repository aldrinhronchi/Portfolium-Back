using Microsoft.EntityFrameworkCore;
using Portfolium_Back.Context;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Connections.Repositories
{
    /// <summary>
    /// Repositório para gerenciar projetos do portfólio
    /// </summary>
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private new readonly PortfoliumContext _context;

        public ProjectRepository(PortfoliumContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Buscar projetos com paginação e filtros
        /// </summary>
        public async Task<RequestViewModel<Project>> GetProjectsPagedAsync(ProjectFilterViewModel filters)
        {
            var query = _context.Projects
                .Include(p => p.User)
                .Where(p => p.IsActive)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(p => p.Name.Contains(filters.Name));
            }

            if (!string.IsNullOrEmpty(filters.Category))
            {
                query = query.Where(p => p.Category == filters.Category);
            }

            if (!string.IsNullOrEmpty(filters.Status))
            {
                query = query.Where(p => p.Status == filters.Status);
            }

            if (filters.IsFeatured.HasValue)
            {
                query = query.Where(p => p.IsFeatured == filters.IsFeatured.Value);
            }

            if (filters.UserID.HasValue)
            {
                query = query.Where(p => p.UserID == filters.UserID.Value);
            }

            if (filters.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == filters.IsActive.Value);
            }

            // Contar total
            var totalCount = await query.CountAsync();

            // Aplicar ordenação
            switch (filters.OrderBy?.ToLower())
            {
                case "name":
                    query = filters.OrderDirection?.ToUpper() == "DESC" 
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name);
                    break;
                case "datecreated":
                    query = filters.OrderDirection?.ToUpper() == "DESC" 
                        ? query.OrderByDescending(p => p.DateCreated)
                        : query.OrderBy(p => p.DateCreated);
                    break;
                case "category":
                    query = filters.OrderDirection?.ToUpper() == "DESC" 
                        ? query.OrderByDescending(p => p.Category)
                        : query.OrderBy(p => p.Category);
                    break;
                default: // DisplayOrder
                    query = filters.OrderDirection?.ToUpper() == "DESC" 
                        ? query.OrderByDescending(p => p.DisplayOrder)
                        : query.OrderBy(p => p.DisplayOrder);
                    break;
            }

            // Aplicar paginação
            var skipCount = (filters.Page - 1) * filters.PageSize;
            var projectEntities = await query
                .Skip(skipCount)
                .Take(filters.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / filters.PageSize);

            return new RequestViewModel<Project>
            {
                Data = projectEntities,
                Page = filters.Page,
                PageSize = filters.PageSize,
                PageCount = totalPages,
                Type = "Project",
                Status = "Success",
                Message = $"Encontrados {projectEntities.Count} projetos"
            };
        }

        /// <summary>
        /// Buscar projetos em destaque
        /// </summary>
        public async Task<List<Project>> GetFeaturedProjectsAsync(int? limit = null)
        {
            var query = _context.Projects
                .Include(p => p.User)
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.DateCreated);

            if (limit.HasValue)
            {
                return await query.Take(limit.Value).ToListAsync();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Buscar projetos por categoria
        /// </summary>
        public async Task<List<Project>> GetProjectsByCategoryAsync(string category)
        {
            return await _context.Projects
                .Include(p => p.User)
                .Where(p => p.IsActive && p.Category == category)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.DateCreated)
                .ToListAsync();
        }

        /// <summary>
        /// Buscar projetos por usuário
        /// </summary>
        public async Task<List<Project>> GetProjectsByUserAsync(int userId)
        {
            return await _context.Projects
                .Include(p => p.User)
                .Where(p => p.IsActive && p.UserID == userId)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.DateCreated)
                .ToListAsync();
        }

        /// <summary>
        /// Buscar projeto por GUID
        /// </summary>
        public async Task<Project?> GetProjectByGuidAsync(Guid guidId)
        {
            return await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.GuidID == guidId && p.IsActive);
        }

        /// <summary>
        /// Buscar categorias únicas de projetos
        /// </summary>
        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Projects
                .Where(p => p.IsActive && !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        /// <summary>
        /// Buscar tecnologias únicas utilizadas nos projetos
        /// </summary>
        public async Task<List<string>> GetTechnologiesAsync()
        {
            var allTechnologies = await _context.Projects
                .Where(p => p.IsActive && !string.IsNullOrEmpty(p.Technologies))
                .Select(p => p.Technologies!)
                .ToListAsync();

            var uniqueTechnologies = new HashSet<string>();

            foreach (var tech in allTechnologies)
            {
                // Tenta primeiro como JSON, senão como CSV
                try
                {
                    var techArray = System.Text.Json.JsonSerializer.Deserialize<string[]>(tech);
                    if (techArray != null)
                    {
                        foreach (var t in techArray)
                        {
                            if (!string.IsNullOrWhiteSpace(t))
                                uniqueTechnologies.Add(t.Trim());
                        }
                    }
                }
                catch
                {
                    // Se falhar como JSON, trata como CSV
                    var techs = tech.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var t in techs)
                    {
                        if (!string.IsNullOrWhiteSpace(t))
                            uniqueTechnologies.Add(t.Trim());
                    }
                }
            }

            return uniqueTechnologies.OrderBy(t => t).ToList();
        }

        /// <summary>
        /// Contar projetos por status
        /// </summary>
        public async Task<Dictionary<string, int>> GetProjectCountByStatusAsync()
        {
            return await _context.Projects
                .Where(p => p.IsActive && !string.IsNullOrEmpty(p.Status))
                .GroupBy(p => p.Status!)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
} 