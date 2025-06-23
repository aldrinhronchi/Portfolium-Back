using System.ComponentModel.DataAnnotations;
using Portfolium_Back.Connections.Repositories.Interface;
using Portfolium_Back.Models;
using Portfolium_Back.Models.ViewModels;
using Portfolium_Back.Services.Interfaces;
using Portfolium_Back.Context;
using Portfolium_Back.Connections.Repositories;
using Portfolium_Back.Extensions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Portfolium_Back.Services
{
    /// <summary>
    /// Serviço para gerenciar projetos do portfólio
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly PortfoliumContext db;
        private readonly IRepository<Project> ProjectRepo;
        private readonly Guid userId = TokenHelper.GetSingleUserId();
        public ProjectService(PortfoliumContext db)
        {
            this.db = db;
            this.ProjectRepo = new Repository<Project>(db);
        }

        /// <summary>
        /// Buscar projetos com paginação e filtros
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> GetAllAsync(Int32 Pagina, Int32 RegistrosPorPagina,
            String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false)
        {
            RequestViewModel<ProjectViewModel> Requisicao;
            IQueryable<Project> _Projects = db.Projects.Include(i => i.User).Where(x => x.IsActive);

            // Em sistema single-user, filtramos automaticamente pelo usuário único
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            if (singleUser != null)
            {
                _Projects = _Projects.Where(p => p.UserID == singleUser.ID);
            }

            if (!String.IsNullOrWhiteSpace(CamposQuery))
            {
                String[] CamposArray = CamposQuery.Split(";|;");
                String[] ValoresArray = ValoresQuery.Split(";|;");
                if (CamposArray.Length == ValoresArray.Length)
                {
                    Dictionary<String, String> Filtros = new Dictionary<String, String>();
                    for (Int32 index = 0; index < CamposArray.Length; index++)
                    {
                        String? Campo = CamposArray[index];
                        String? Valor = ValoresArray[index];
                        if (!(String.IsNullOrWhiteSpace(Campo) && String.IsNullOrWhiteSpace(Valor)))
                        {
                            Filtros.Add(Campo, Valor);
                        }
                    }
                    IQueryable<Project> ProjectFiltrado = _Projects;
                    foreach (KeyValuePair<String, String> Filtro in Filtros)
                    {
                        ProjectFiltrado = TipografiaHelper.Filtrar(ProjectFiltrado, Filtro.Key, Filtro.Value);
                    }
                    _Projects = ProjectFiltrado;
                }
                else
                {
                    throw new ValidationException("Não foi possível filtrar!");
                }
            }

            if (!String.IsNullOrWhiteSpace(Ordenacao))
            {
                _Projects = TipografiaHelper.Ordenar(_Projects, Ordenacao, Ordem);
            }
            else
            {
                _Projects = TipografiaHelper.Ordenar(_Projects, "ID", Ordem);
            }

            Requisicao = await TipografiaHelper.FormatarRequisicaoParaViewModelAsync<Project, ProjectViewModel>(_Projects, Pagina, RegistrosPorPagina, new ProjectViewModel());

            return Requisicao;
        }

        /// <summary>
        /// Buscar projeto por ID
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> GetByIdAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid projectId))
            {
                throw new ValidationException("ID inválido!");
            }

            Project? _project = await ProjectRepo.GetAsync(x => x.GuidID == projectId && x.IsActive);
            if (_project == null)
            {
                throw new ValidationException("Projeto não encontrado!");
            }

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel> { new ProjectViewModel(_project) },
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projeto encontrado com sucesso"
            };
        }

        /// <summary>
        /// Buscar projeto por GUID
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> GetByGuidAsync(Guid guidId)
        {
            var project = await ProjectRepo.GetAsync(x => x.GuidID == guidId && x.IsActive);

            if (project == null)
            {
                throw new ValidationException("Projeto não encontrado!");
            }

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel> { new ProjectViewModel(project) },
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projeto encontrado com sucesso"
            };
        }

        /// <summary>
        /// Criar um novo projeto (legacy - mantido para compatibilidade)
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> CreateAsync(ProjectViewModel projectViewModel)
        {
            // Em modo single-user, usamos o usuário único automaticamente
            return await CreateAsync(projectViewModel, AppConstants.SINGLE_USER_ID);
        }

        /// <summary>
        /// Criar um novo projeto (single-user system)
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> CreateAsync(ProjectViewModel projectViewModel, Guid userId)
        {
            Validator.ValidateObject(projectViewModel, new ValidationContext(projectViewModel), true);

            IRepository<User> UserRepo = new Repository<User>(db);
            var user = await UserRepo.GetAsync(x => x.GuidID == userId);

            if (user == null)
            {
                throw new ValidationException("Usuário não encontrado!");
            }
            if (!user.IsActive)
            {
                throw new ValidationException("Usuário inválido!");
            }

            Project newProject = projectViewModel.ToEntity();
            newProject.UserID = user.ID;
            newProject.IsActive = true;
            newProject.DateCreated = TimeZoneManager.GetTimeNow();
            newProject.UserCreated = "Sistema";

            await ProjectRepo.CreateAsync(newProject);

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel> { new ProjectViewModel(newProject) },
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projeto criado com sucesso"
            };
        }

        /// <summary>
        /// Atualizar um projeto existente (single-user system)
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> UpdateAsync(ProjectViewModel projectViewModel)
        {
            Validator.ValidateObject(projectViewModel, new ValidationContext(projectViewModel), true);

            Project? _project = await ProjectRepo.GetAsync(x => x.GuidID == projectViewModel.GuidID);

            if (_project == null)
            {
                throw new ValidationException("Projeto não encontrado");
            }

            // Em sistema single-user, sempre usamos o usuário único
            IRepository<User> UserRepo = new Repository<User>(db);
            var user = await UserRepo.GetAsync(x => x.GuidID == userId);

            if (user == null)
            {
                throw new ValidationException("Usuário não encontrado!");
            }
            if (!user.IsActive)
            {
                throw new ValidationException("Usuário inválido!");
            }

            Project projectToUpdate = new Project
            {
                ID = _project.ID,
                GuidID = _project.GuidID,
                Name = projectViewModel.Name,
                Description = projectViewModel.Description,
                ShortDescription = projectViewModel.ShortDescription,
                Technologies = projectViewModel.Technologies,
                ProjectUrl = projectViewModel.ProjectUrl,
                DemoUrl = projectViewModel.DemoUrl,
                RepositoryUrl = projectViewModel.RepositoryUrl,
                MainImage = projectViewModel.MainImage,
                AdditionalImages = projectViewModel.AdditionalImages,
                Status = projectViewModel.Status,
                Category = projectViewModel.Category,
                DisplayOrder = projectViewModel.DisplayOrder,
                IsFeatured = projectViewModel.IsFeatured,
                StartDate = projectViewModel.StartDate,
                EndDate = projectViewModel.EndDate,
                UserID = user.ID,
                DateCreated = _project.DateCreated,
                DateUpdated = DateTime.UtcNow,
                IsActive = _project.IsActive,
                UserCreated = _project.UserCreated,
                UserUpdated = "Sistema"
            };

            await ProjectRepo.UpdateAsync(projectToUpdate);

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel> { projectViewModel },
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projeto atualizado com sucesso"
            };
        }

        /// <summary>
        /// Excluir projeto (soft delete)
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> DeleteAsync(String id)
        {
            if (!Guid.TryParse(id, out Guid projectId))
            {
                throw new ValidationException("ID inválido!");
            }

            Project? _project = await ProjectRepo.GetAsync(x => x.GuidID == projectId);
            if (_project == null)
            {
                throw new ValidationException("Projeto não encontrado");
            }

            _project.IsActive = false;
            _project.DateUpdated = DateTime.UtcNow;
            _project.UserUpdated = "Sistema";
            await ProjectRepo.UpdateAsync(_project);

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel>(),
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projeto excluído com sucesso"
            };
        }

        /// <summary>
        /// Buscar projetos em destaque
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> GetFeaturedAsync(int? limit = null)
        {            
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            
            IQueryable<Project> query = db.Projects.Where(p => p.IsFeatured && p.IsActive);
            if (singleUser != null)
            {
                query = query.Where(p => p.UserID == singleUser.ID);
            }

            var projects = await query.Take(limit ?? 10).ToListAsync();

            return new RequestViewModel<ProjectViewModel>
            {
                Data = projects.Select(p => new ProjectViewModel(p)).ToList(),
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = "Projetos em destaque encontrados com sucesso"
            };
        }

        /// <summary>
        /// Buscar projetos por categoria
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> GetByCategoryAsync(string category)
        {
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            
            IQueryable<Project> query = db.Projects.Where(p => p.Category == category && p.IsActive);
            if (singleUser != null)
            {
                query = query.Where(p => p.UserID == singleUser.ID);
            }

            var projects = await query.ToListAsync();

            return new RequestViewModel<ProjectViewModel>
            {
                Data = projects.Select(p => new ProjectViewModel(p)).ToList(),
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = $"Projetos da categoria '{category}' encontrados com sucesso"
            };
        }

        /// <summary>
        /// Buscar categorias únicas
        /// </summary>
        public async Task<RequestViewModel<string>> GetCategoriesAsync()
        {
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            
            IQueryable<Project> query = db.Projects.Where(p => p.IsActive);
            if (singleUser != null)
            {
                query = query.Where(p => p.UserID == singleUser.ID);
            }

            var categories = await query
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            return new RequestViewModel<string>
            {
                Data = categories,
                Type = "String",
                Status = "Success",
                Message = "Categorias encontradas com sucesso"
            };
        }

        /// <summary>
        /// Buscar tecnologias únicas
        /// </summary>
        public async Task<RequestViewModel<string>> GetTechnologiesAsync()
        {
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            
            IQueryable<Project> query = db.Projects.Where(p => p.IsActive);
            if (singleUser != null)
            {
                query = query.Where(p => p.UserID == singleUser.ID);
            }

            var allTechnologies = await query
                .Where(p => !string.IsNullOrEmpty(p.Technologies))
                .Select(p => p.Technologies)
                .ToListAsync();

            var technologies = allTechnologies
                .SelectMany(t => t.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(t => t.Trim())
                .Distinct()
                .ToList();

            return new RequestViewModel<string>
            {
                Data = technologies,
                Type = "String",
                Status = "Success",
                Message = "Tecnologias encontradas com sucesso"
            };
        }

        /// <summary>
        /// Obter estatísticas dos projetos
        /// </summary>
        public async Task<RequestViewModel<ProjectStatsViewModel>> GetStatsAsync()
        {
            var singleUser = await db.Users.FirstOrDefaultAsync(u => u.GuidID == userId);
            
            IQueryable<Project> query = db.Projects.Where(p => p.IsActive);
            if (singleUser != null)
            {
                query = query.Where(p => p.UserID == singleUser.ID);
            }

            var totalProjects = await query.CountAsync();
            var featuredProjects = await query.CountAsync(p => p.IsFeatured);
            var categoriesCount = await query
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category)
                .Distinct()
                .CountAsync();

            var stats = new ProjectStatsViewModel
            {
                TotalProjects = totalProjects,
                FeaturedProjects = featuredProjects,
                CategoriesCount = categoriesCount,
                LastUpdate = await query.MaxAsync(p => (DateTime?)p.DateUpdated) ?? await query.MaxAsync(p => (DateTime?)p.DateCreated)
            };

            return new RequestViewModel<ProjectStatsViewModel>
            {
                Data = new List<ProjectStatsViewModel> { stats },
                Type = nameof(ProjectStatsViewModel),
                Status = "Success",
                Message = "Estatísticas obtidas com sucesso"
            };
        }

        /// <summary>
        /// Ativar/Desativar projeto
        /// </summary>
        public async Task<RequestViewModel<ProjectViewModel>> ToggleStatusAsync(int id, string userInfo)
        {
            var project = await ProjectRepo.GetAsync(x => x.ID == id);
            if (project == null)
            {
                throw new ValidationException("Projeto não encontrado!");
            }

            project.IsActive = !project.IsActive;
            project.DateUpdated = DateTime.UtcNow;
            project.UserUpdated = userInfo;

            await ProjectRepo.UpdateAsync(project);

            return new RequestViewModel<ProjectViewModel>
            {
                Data = new List<ProjectViewModel> { new ProjectViewModel(project) },
                Type = nameof(ProjectViewModel),
                Status = "Success",
                Message = $"Status do projeto {(project.IsActive ? "ativado" : "desativado")} com sucesso"
            };
        }
    }
}