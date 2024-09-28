using Database.Persitance;
using Functions.Persitance;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Configuration;

public static class Configurations
{
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WeeklyReviewApiDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("WeeklyReview"));
        });
        services.AddScoped<WeeklyReviewDbContext, WeeklyReviewApiDbContext>();
    }
}
