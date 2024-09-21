// SMS.Application/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using SMS.Application.Interfaces.Departments;
using SMS.Application.Interfaces.Students;
using SMS.Application.Services.Departments;
using SMS.Application.Services.Departments.Dto;
using SMS.Application.Services.Departments.Validators;
using SMS.Application.Services.Students;
using SMS.Application.Services.Students.Dto;
using SMS.Application.Services.Students.Validators;
using FluentValidation;
using SMS.Application.Services.Teachers.Dto;
using SMS.Application.Services.Teachers.Validators;
using SMS.Application.Interfaces.Teachers;
using SMS.Application.Services.Teachers;
using SMS.Application.Interfaces.Accounts;
using SMS.Application.Services.Account;
using AutoMapper;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Services.UserConfigurations.Dto;
using SMS.Application.Services.UserConfigurations;
using SMS.Application.Services.Account.Dto;
using SMS.Application.Services.Account.Validator;

namespace SMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
              
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddValidations();
            ResolveServices(services);
            return services;
        }

        private static void AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<UpdateUserConfigurationDto>, UpdateUserConfigurationDtoValidator>();
            //egister FluentValidation (Uncomment and add actual validators)
            services.AddTransient<IValidator<CreateStudentDto>, StudentValidator>();
            services.AddTransient<IValidator<CreateDepartmentDto>, DepartmentValidator>();
            services.AddTransient<IValidator<CreateTeacherDto>, TeacherValidator>();
            services.AddTransient<IValidator<LoginResponseDto>, LoginResponseValidator>();
        }

        private static IServiceCollection ResolveServices(this IServiceCollection services)
        {
            services.AddScoped<IUserConfigurationService, UserConfigurationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            return services;
        }

    }
}
