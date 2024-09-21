using SMS.Application.Services.UserConfigurations.Dto;
using SMS.Common.Responses;

namespace SMS.Application.Interfaces.Identity
{
    public interface IUserConfigurationService
    {
        Task<Response<IEnumerable<UserConfigurationDto>>> GetUserConfigurations(UserConfigurationRequestDto request);

        Task<Response> UpdateUserConfigurations(List<UpdateUserConfigurationDto> request);

        Task<ExportNetworkFilesDto> ExportNetworkSampleFiles();

        Task<ExportNetworkFilesDto> ExportLocationSampleFile();

        Task SaveUserConfiguration(Guid userId);
    }
}
