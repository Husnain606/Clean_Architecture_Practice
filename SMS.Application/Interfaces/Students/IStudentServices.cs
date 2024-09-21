﻿using SMS.Application.Services.Students.Dto;
using SMS.Common.ViewModels;

namespace SMS.Application.Interfaces.Students
{
    public interface IStudentService
    {
        Task<ResponseModel> GetStudentListAsync();
        Task<ResponseModel> GetStudentDetailsByIdAsync(Guid StudentID);
        Task<ResponseModel> UpdateStudentAsync(CreateStudentDto StudentModel);
        Task<ResponseModel> CreateStudentAsync(CreateStudentDto StudentModel);
        Task<ResponseModel> DeleteStudentAsync(Guid StudentID);
        Task<List<StudentDto>> GetStudentDetailsByAgeG13Async(int age);    // 2.	O  - Open Closed Principal (OCP):
                                                                           // it is open for extension that it can get the student
                                                                           // by some parameter chechk  now it is depend upon service
                                                                           // that in parameter through which filter it can process

    }
}