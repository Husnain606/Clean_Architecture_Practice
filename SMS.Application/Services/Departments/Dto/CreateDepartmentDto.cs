namespace SMS.Application.Services.Departments.Dto
{
    /* L – Liskov Substitution Principal (LSP):
    * The child  CreateDepartmentDTO can completely replace its parent DepartmentDTO
    * it provides all the functionalities of DepartmentDTO*/
    public class CreateDepartmentDto : DepartmentDto
    {
        public Guid Id { get; set; }
    }
}
