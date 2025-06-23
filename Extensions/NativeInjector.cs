using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using Portfolium_Back.Context;
using Portfolium_Back.Connections.Configurations;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Extensions.Middleware;
using Portfolium_Back.Services;
using Portfolium_Back.Services.Interfaces;
using Portfolium_Back.Connections.Repositories;
using Portfolium_Back.Connections.Repositories.Interface;

namespace Portfolium_Back.Extensions
{
    public class NativeInjector
    {
        public static void RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            #region Log

            Serilog.Core.Logger logger = new LoggerConfiguration()
          .ReadFrom.Configuration(configuration)
          .CreateLogger();

            services.AddSingleton(provider =>
                new LoggerFactory().AddSerilog(logger).CreateLogger("Portfolium-Back"));

            #endregion Log

            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ICurriculumService, CurriculumService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddTransient<ErrorHandlerService>();

            #endregion Services

            #region Repository

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            // Repositório genérico é instanciado diretamente nos serviços

            #endregion Repository

            #region Other

            ServiceLocator.IncluirServico(services.BuildServiceProvider());

            #endregion Other
        }

        public static void RegisterBuild(WebApplicationBuilder builder)
        {
            #region Context

            builder.Services.AddDbContext<PortfoliumContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("PandoraDB"))/*.EnableSensitiveDataLogging()*/;
            });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            #endregion Context

            #region Swagger

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration(builder.Configuration);

            #endregion Swagger

            #region JWT
            var JWTBuilderKey = builder.Configuration["Jwt:Key"] ?? "";
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTBuilderKey))
                };
            });

            #endregion JWT

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "Origins",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200",
                                                          "http://localhost:3000",
                                                          "https://localhost:4200",
                                                          "https://localhost:3000")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowCredentials();
                                  });
                
                // Política para desenvolvimento
                options.AddPolicy(name: "Development",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });

            #endregion CORS
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwaggerConfiguration(env);
            
            app.UseHttpsRedirection();
            app.AddMiddlewares();
            
            // Usar política de CORS baseada no ambiente
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
            }
            else
            {
                app.UseCors("Origins");
            }

            #region Auth

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion Auth
        }
    }

    public static class MiddlewareRegistrationExtension
    {
        public static void AddMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}