﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;  // Added this for IConfiguration
using Microsoft.Extensions.DependencyInjection;  // Added this for AddDbContextCheck
using SMS.Application.Interfaces;
using SMS.Application.Interfaces.Identity;
using SMS.Domain.Entities;
using SMS.Infrastructure.Data;
using SMS.Infrastructure.Repositories.Base;
using SMS.Presistence.Repositories;
using SMS.IdentityService.Services;

namespace SMS.Presistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPlanningPortalPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PlanningPortalConnection"),
                      b =>
                      {
                          b.CommandTimeout(300);
                          b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                      });
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            // Ensure that you have the HealthChecks EF Core package installed
            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            services.ResolveRepositories();
            //ResolveServices(services);
            return services;
        }

        public static void ResolveRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlanningPortalRepository<EntityType>, PlanningPortalRepository<EntityType>>();
            services.AddScoped<IPlanningPortalRepository<UserConfiguration>, PlanningPortalRepository<UserConfiguration>>();
            services.AddScoped<IPlanningPortalRepository<ConfigurationSchema>, PlanningPortalRepository<ConfigurationSchema>>();
            services.AddScoped<IPlanningPortalRepository<ApplicationUser>, PlanningPortalRepository<ApplicationUser>>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IApplicationDbContext), typeof(ApplicationDbContext));
            services.AddScoped<IIdentityService, SMS.IdentityService.Services.IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        //public static void ResolveServices(this IServiceCollection services)
        //{
            
        //}
    }
}