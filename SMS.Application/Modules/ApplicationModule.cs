using AutoMapper;
using SMS.Application.Mapping;

namespace SMS.Application.Modules
{
    public static class ApplicationModule
    {
        public static Profile AccountMapper()
        {
            return new AccountMapper();
        }

        public static Profile DepartmentMapper()
        {
            return new DepartmentMapper();
        }
        public static Profile StudentMapper()
        {
            return new StudentMapper();
        }
        public static Profile TeacherMapper()
        {
            return new TeacherMapper();
        }
    }
    }
