using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using WeeklyReview.Client.Services;
using WeeklyReview.Database.Converters;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Setup
{
    public static class Configurations
    {
        public static void AddConfigurationProviders(this WebApplicationBuilder builder)
        {
            if(builder.Environment.IsDevelopment())
                Console.WriteLine("Runnig in Development");
            if (builder.Environment.IsProduction())
                Console.WriteLine("Runnig in Production");
        }

        public static void AddDatabases(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<WeeklyReviewApiDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("WeeklyReview"));
            });
            builder.Services.AddScoped<WeeklyReviewDbContext, WeeklyReviewApiDbContext>();
        }

        public static void AddAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
        }

        public static void AddCors(this WebApplicationBuilder builder, string allowedOrigins)
        {
            builder.Services.AddCors(options => options
                .AddPolicy(name: allowedOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }));
        }

        public static void AddEndpoints(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddControllersWithViews(options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonColorConverter());
                })
                .AddOData(options => options.EnableQueryFeatures(null)
                .EnableQueryFeatures(null));
            builder.Services.AddRazorPages();
        }
    }
}
