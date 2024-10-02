using SMS.Application.Services.Students.Dto;

namespace SMS.Application.Interfaces.Students
{
    public interface IStudentGetMethods
    {
        /* 	I  - Interface Segregation Principle (ISP):
         * Create IStudentGetMethods interface rather then
         * put all the joins and groups method declaration
         * in single interface IStudentServices*/
        Task<List<StudentRequestDto>> InnerJoin();
        Task<List<StudentRequestDto>> GetLeftOuterJoinFields();
        Task<List<StudentRequestDto>> GetRightOuterJoinFields();
        Task<List<StudentRequestDto>> GetLeftInnerJoinFields();
        Task<List<StudentRequestDto>> GetRightInnerJoinFields();
        Task<List<StudentRequestDto>> GetGroupJoinFields();
        Task<List<GroupedStudentsDTO>> GroupByDepartment();
    }
}
