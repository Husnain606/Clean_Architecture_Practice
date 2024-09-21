namespace SMS.Application.Services.Students.Dto
{
    public class GroupedStudentsDTO
    {
        public Guid DepartmentId { get; set; }
        public List<StudentDto> Students { get; set; }
    }
}
