using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Portfolium_Back.Connections.Configurations
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            String environmentName = configuration["Environment"] ?? "Development";
            String version = configuration["ApiVersion"] ?? "v1.0";

            return services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Portfolium API",
                    Version = version,
                    Description = $"API do Portfolium - Sistema de Portfólio Pessoal ({environmentName})",
                    Contact = new OpenApiContact
                    {
                        Name = "Terore Technology",
                        Email = "contato@teroretechnology.com",
                        Url = new Uri("https://teroretechnology.com/contato")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Agrupar endpoints por controller em vez de por método HTTP
                option.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"] });

                // Ordenação de endpoints por controller e depois por caminho
                option.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");

                String xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                String xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (File.Exists(xmlPath))
                {
                    option.IncludeXmlComments(xmlPath);
                }

                // Personalização de esquemas de autenticação
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT com Bearer: `Bearer {token}`",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme = "oauth2",
                            Name ="Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<String>()
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = ""
            });

            String wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath);
            }

            String indexPath = Path.Combine(wwwrootPath, "index.html");
            String templatePath = Path.Combine(wwwrootPath, "templates", "index.html");

            if (File.Exists(templatePath) && !File.Exists(indexPath))
            {
                File.Copy(templatePath, indexPath);
            }
            else if (!File.Exists(indexPath))
            {
                String htmlContent = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>Portfolium API</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 20px; background: #f5f5f5; }
        .container { max-width: 800px; margin: 0 auto; background: white; padding: 40px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
        h1 { color: #2c3e50; margin-bottom: 10px; }
        .subtitle { color: #7f8c8d; margin-bottom: 30px; }
        a { color: #3498db; text-decoration: none; padding: 10px 20px; background: #ecf0f1; border-radius: 5px; display: inline-block; }
        a:hover { background: #3498db; color: white; text-decoration: none; }
        .features { margin-top: 30px; }
        .feature { margin: 10px 0; padding: 10px; background: #f8f9fa; border-left: 4px solid #3498db; }
    </style>
</head>
<body>
    <div class=""container"">
        <h1>🎯 Portfolium API</h1>
        <p class=""subtitle"">Sistema de Portfólio Pessoal - API REST</p>
        
        <a href=""/docs"">📚 Acessar Documentação Swagger</a>
        
        <div class=""features"">
            <div class=""feature"">
                <strong>🔐 Autenticação:</strong> JWT Bearer Token
            </div>
            <div class=""feature"">
                <strong>📄 Paginação:</strong> Suporte completo a filtros e ordenação
            </div>
            <div class=""feature"">
                <strong>🛡️ Tratamento de Erros:</strong> Middleware global de exceções
            </div>
            <div class=""feature"">
                <strong>📊 Logs:</strong> Serilog com rotação diária
            </div>
        </div>
    </div>
</body>
</html>";

                File.WriteAllText(indexPath, htmlContent);
            }
            
            return app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            })
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/docs/v1/swagger.json", "Portfolium API v1");
                options.RoutePrefix = "docs";
                options.DocumentTitle = "Documentação Portfolium API";
                options.EnableDeepLinking();
                options.DisplayRequestDuration();

                if (env.IsDevelopment())
                {
                    options.EnableTryItOutByDefault();
                    options.DefaultModelsExpandDepth(2);
                }
                else
                {
                    options.DefaultModelsExpandDepth(0); 
                }

                // Personalização da UI
                options.InjectStylesheet("/swagger-ui/custom.css");
            });
        }
    }
} 