using SMS.Application.Services.Students.Dto;

namespace SMS.Application.Interfaces.Students
{
    public interface IStudentGetMethods
    {
        /* 	I  - Interface Segregation Principle (ISP):
         * Create IStudentGetMethods interface rather then
         * put all the joins and groups method declaration
         * in single interface IStudentServices*/
        Task<List<StudentDto>> InnerJoin();
        Task<List<StudentDto>> GetLeftOuterJoinFields();
        Task<List<StudentDto>> GetRightOuterJoinFields();
        Task<List<StudentDto>> GetLeftInnerJoinFields();
        Task<List<StudentDto>> GetRightInnerJoinFields();
        Task<List<StudentDto>> GetGroupJoinFields();
        Task<List<GroupedStudentsDTO>> GroupByDepartment();
    }
}
