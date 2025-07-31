using DocCheck.Bitrix;
using DocCheck.Components;
using DocCheck.Components.Account;
using DocCheck.Data;
using DocCheck.OData;
using DocCheck.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace DocCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Configuration.GetValue<bool>("IsUseSeq", defaultValue: false))
            {
                Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

                builder.Host.UseSerilog();

                builder.Services.AddOpenTelemetry()
                    .ConfigureResource(r => r.AddService("DockCheck Service"))
                    .WithTracing(t =>
                    {
                        t.AddSource("DocCheck.Source");
                        t.AddAspNetCoreInstrumentation();
                        t.AddHttpClientInstrumentation();
                        t.AddSqlClientInstrumentation();
                        t.AddConsoleExporter();
                        t.AddOtlpExporter(e =>
                        {
                            e.Endpoint = new Uri($"{builder.Configuration["OpenTelemetry:OtlpExporter"]}/traces");
                            e.Protocol = OtlpExportProtocol.HttpProtobuf;
                        });
                    });
            }

            // Add services to the container.
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

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddQuickGridEntityFrameworkAdapter();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

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

            builder.Services.AddScoped<ODataService>();
            builder.Services.AddScoped<BitrixService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<DocCheckRepository>();
            //builder.Services.AddScoped(typeof(Repository<>));
            builder.Services.AddScoped<IDocCheckService, DocCheckService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<ISaleDocService, SaleDocService>();
            builder.Services.AddScoped<ICorrectionDocService, CorrectionDocService>();


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
                app.UseMigrationsEndPoint();
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
