using IdentityService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SMS.Application;
using SMS.Application.Mapping;
using SMS.Domain.Entities;
using SMS.Infrastructure.Data;
using SMS.Presistence;
using System.Text;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Serilog for logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Log/Log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Host.UseSerilog();

        // Add configuration for DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")));

        // Add Identity services
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Add Application Services
        //builder.Services.ResolveServices(); // Ensure this method is defined correctly
        builder.Services.AddApplication(); // Ensure this extension method is defined
        builder.Services.AddPlanningPortalPersistence(builder.Configuration);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        JwtSettings _jwtSettings = new JwtSettings();
        // Configure JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // To prevent issues with slight clock drift

            };
        });

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();  // Developer Exception Page for detailed error information in development mode
        }

        app.UseSwagger();  // Enable Swagger middleware
        app.UseSwaggerUI(c =>  // Configure Swagger UI
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        app.UseHttpsRedirection();  // Enforce HTTPS

        app.UseRouting();  // Enable routing

        app.UseAuthentication();  // Enable authentication middleware
        app.UseAuthorization();   // Enable authorization middleware

        app.UseEndpoints(endpoints =>  // Map controllers to endpoints
        {
            endpoints.MapControllers();
        });

        app.Run();  // Start the application

    }
}