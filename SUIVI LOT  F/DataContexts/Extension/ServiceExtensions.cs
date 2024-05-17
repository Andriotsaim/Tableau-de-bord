using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SUIVI.Helpers;
using SUIVI.Repository;
using SUIVI.Repository.InterfaceRepository;
using SUIVI.Services;
using SUIVI.Services.IService;
using System.Text.Json.Serialization;

namespace SUIVI.DataContexts.Extension
{
    /// <summary>
    /// Other configuration for program.cs."
    /// </summary>
    public static class ServiceExtensions
    {
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30000);
            });
            services.AddHttpContextAccessor();
            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            services.TryAddScoped<IRedergroupRepository,RedergroupRepository>();
            services.TryAddScoped<ISuiviRepository, SuiviRepository>();
            services.TryAddScoped<IAccountRepository, AccountRepository>();
            services.TryAddScoped<IPreviewRepository, PreviewRepository>();
            services.TryAddScoped<ITransfertRepository, TransfertRepository>();

            services.AddTransient<ISuiviService, SuiviService>();
            services.AddTransient<IPreviewService, PreviewService>();
            services.AddTransient<ITransfertService, TransfertService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.Secure = CookieSecurePolicy.Always; // Include the 'secure' directive
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(120);
                options.ExcludedHosts.Add("dashboardsaimltd.mu");
                options.ExcludedHosts.Add("www.dashboardsaimltd.mu");
            });

            return services;
        }
        //public static void ConfigureRederContext(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<RedergroupContext>(options =>
        //    options.UseMySql(configuration.GetConnectionString(SD.Reder), ServerVersion.AutoDetect(configuration.GetConnectionString(SD.Reder)), b => b.MigrationsAssembly("SUIVI")));
        //}
    }
}
