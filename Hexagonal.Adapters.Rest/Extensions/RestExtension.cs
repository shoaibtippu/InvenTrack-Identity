using Hexagonal.Adapters.Rest.Filters;
using Hexagonal.Application.Common.Exceptions.Common;
using InvenTrack.Application.Ports.In.User;
using InvenTrack.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace InvenTrack.Adapters.Rest.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring REST services.
    /// </summary>
    public static class RestExtension
    {
        /// <summary>
        /// Adds application services for REST.
        /// </summary>
        public static void AddRestApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<AutoMapperResult>();
            services.AddScoped<IUserService, UserService>();
        }

        /// <summary>
        /// Configures services for REST.
        /// </summary>
        public static IServiceCollection ConfigureRestServices(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });
            services.AddEndpointsApiExplorer();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            return services;
        }

        
        /// <summary>
        /// Uses Swagger services.
        /// </summary>
        public static void UseSwaggerServices(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HEX API V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.EnableDeepLinking();
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });
            app.MapControllers();
        }
        

        /// <summary>
        /// Adds Swagger services.
        /// </summary>
        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "InvenTrack API",
                    Description = "MV Backend Api Endpoints"
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                {
                    s.IncludeXmlComments(xmlPath);
                }

                s.OperationFilter<SecurityRequirementsOperationFilter>();
                //s.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "InvenTrack API",
                //    Description = "MV Backend Api Endpoints"
                //});
                //var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                //if (System.IO.File.Exists(xmlPath))
                //{
                //    s.IncludeXmlComments(xmlPath);
                //}
                //s.OperationFilter<SecurityRequirementsOperationFilter>();
                //s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme.",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});
            });
        }
    }
}
