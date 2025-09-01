using DocCheck.Application;
using DocCheck.Components;
using DocCheck.Components.Account;
using DocCheck.Data;
using DocCheck.Infrastructure.Bitrix;
using DocCheck.Infrastructure.OData;
using DocCheck.Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using Bitrix = DocCheck.Infrastructure.Bitrix;
using OData = DocCheck.Infrastructure.OData;

namespace DocCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Services.AddSerilog();

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var odataConfig = builder.Configuration.GetSection(nameof(OData)).Get<ODataConfig>()
                ?? throw new InvalidOperationException("OData Config Not Found");
            var authenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{odataConfig.UserName}:{odataConfig.Password}"));
            builder.Services.AddHttpClient<ODataClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(odataConfig.BaseAddress);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationString);
            });

            var bitrixConfig = builder.Configuration.GetSection(nameof(Bitrix)).Get<BitrixConfig>()
                ?? throw new InvalidOperationException("Bitrix Config Not Found");
            builder.Services.AddHttpClient<BitrixClient>(HttpClient =>
            {
                HttpClient.BaseAddress = new Uri(bitrixConfig.BaseAddress);
            });

            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<BitrixService>();

            builder.Services.AddScoped<IODataService, ODataService>();

            builder.Services.AddScoped<ISaleDocService, SaleDocService>();

            builder.Services.AddScoped<ISaleDocLogService, SaleDocLogService>();

            builder.Services.AddHostedService<RabbitMqConsumer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
