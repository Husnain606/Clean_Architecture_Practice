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
using SMS.Application.Services.Account.Dto;
using SMS.Application.Services.Account.Validator;
using SMS.Application.Modules;
using SMS.Application.Interfaces.Email;
using SMS.Application.Services.Email;
using SMS.Application.Interfaces.Excel;
using SMS.Application.Services.Excel;

namespace SMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var AccountMapper = ApplicationModule.AccountMapper();
            var DepartmentMapper = ApplicationModule.DepartmentMapper();
            var StudentMapper = ApplicationModule.StudentMapper();
            var TeacherMapper = ApplicationModule.TeacherMapper();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(AccountMapper);
                mc.AddProfile(DepartmentMapper);
                mc.AddProfile(StudentMapper);
                mc.AddProfile(TeacherMapper);

            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddValidations();
            ResolveServices(services);
            return services;
        }

        private static void AddValidations(this IServiceCollection services)
        {
          //egister FluentValidation (Uncomment and add actual validators)
            services.AddTransient<IValidator<CreateStudentDto>, StudentValidator>();
            services.AddTransient<IValidator<CreateDepartmentDto>, DepartmentValidator>();
            services.AddTransient<IValidator<CreateTeacherDto>, TeacherValidator>();
            services.AddTransient<IValidator<LoginResponseDto>, LoginResponseValidator>();
            services.AddTransient<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        }

        private static IServiceCollection ResolveServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService,RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserClaimService, UserClaimService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IExcelService, ExcelService>();
            return services;
        }

    }
}
