using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using SMS.Application.Interfaces.Identity;
using SMS.Application.Interfaces;
using SMS.Application.Services.UserConfigurations.Dto;
using System.Collections.ObjectModel;
using SMS.Domain.Entities;
using SMS.Common.Constants;
using SMS.Common.Responses;
using Microsoft.EntityFrameworkCore;
using SMS.Common.Utilities;

namespace SMS.Application.Services.UserConfigurations
{
    public class UserConfigurationService : IUserConfigurationService
    {
        private readonly ILogger<UserConfigurationService> _logger;
        private readonly IPlanningPortalRepository<UserConfiguration> _userConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IPlanningPortalRepository<ConfigurationSchema> _configurationSchemaRepository;

        public UserConfigurationService(ILogger<UserConfigurationService> logger,
                                            IPlanningPortalRepository<UserConfiguration> userConfigurationRepository,
                                            IMapper mapper,
                                            IPlanningPortalRepository<ConfigurationSchema> configurationSchemaRepository)
        {
            _logger = logger;
            _userConfigurationRepository = userConfigurationRepository;
            _mapper = mapper;
            _configurationSchemaRepository = configurationSchemaRepository;
        }

        public async Task<ExportNetworkFilesDto> ExportNetworkSampleFiles()
        {
            _logger.LogInformation($"{nameof(ExportNetworkSampleFiles)} {ApplicationLogsConstants.MethodRunning}");
            ExportNetworkFilesDto result = new() { Name = CsvConstants.SampleNetworkFileName };
           // result.MemoryStream = await _blobStorageService.GetDataStream(CsvConstants.SampleNetworkFileName);
            _logger.LogInformation($"{nameof(ExportNetworkSampleFiles)} {ApplicationLogsConstants.MethodExecuted}");
            return result;
        }

        public async Task<ExportNetworkFilesDto> ExportLocationSampleFile()
        {
            _logger.LogInformation($"{nameof(ExportLocationSampleFile)} {ApplicationLogsConstants.MethodRunning}");
            ExportNetworkFilesDto result = new() { Name = CsvConstants.SampleLocationFileName };
            _logger.LogInformation($"{nameof(ExportLocationSampleFile)} {ApplicationLogsConstants.MethodExecuted}");
            return result;
        }

        public async Task<Response<IEnumerable<UserConfigurationDto>>> GetUserConfigurations(UserConfigurationRequestDto request)
        {
            _logger.LogInformation($"{nameof(GetUserConfigurations)} {ApplicationLogsConstants.MethodRunning}");
            var userConfigurations = await _userConfigurationRepository.TableNoTracking.TagWith("GetUserConfigurations")
                                            .Where(x => x.ConfigurationSchema.EntityTypeId == (int)request.EntityType)
                                            .OrderBy(x => x.DisplayOrder)
                                            .Include(x => x.ConfigurationSchema).ThenInclude(x => x.EntityType)
                                            .ProjectTo<UserConfigurationDto>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            _logger.LogInformation($"{nameof(GetUserConfigurations)} {ApplicationLogsConstants.MethodExecuted}");
            return new Response<IEnumerable<UserConfigurationDto>>() { Successful = true, Result = userConfigurations };
        }

        public async Task<Response> UpdateUserConfigurations(List<UpdateUserConfigurationDto> request)
        {
            _logger.LogInformation($"{nameof(UpdateUserConfigurations)} {ApplicationLogsConstants.MethodRunning}");
            var savedConfigurations = await _userConfigurationRepository.TableNoTracking.TagWith("UpdateUserConfigurations")
                                              .Where(x => request.Select(x => x.Id).Contains(x.Id))
                                              .ToListAsync();
            _logger.LogInformation($"{nameof(UpdateUserConfigurations)} {ApplicationLogsConstants.MethodExecuted}");
            _logger.LogInformation($"{nameof(UpdateUserConfigurations)} {nameof(UserConfiguration)} {ApplicationLogsConstants.EntryUpdated}");

            return await AlterPropertiesAndUpdateConfigurations(request, savedConfigurations);
        }

        public async Task SaveUserConfiguration(Guid userId)
        {
            //List to of SchemaNames to be false in IsFilterable and IsSortable
            ReadOnlyCollection<string> readonlyListOfSchemaNames = ReadOnlyListUtility.ReadOnlyListOfSchemaNames();

            //Will be eventually changed to get data from users table
            var newUserConfigurations = new List<UserConfiguration>();

            //Get configuration schemas list
            var configurationSchemas = await _configurationSchemaRepository.TableNoTracking.TagWith("SaveUserConfiguration").OrderBy(x => x.EntityTypeId).ToListAsync();
            int displayOrder = 1;
            var entityType = configurationSchemas.FirstOrDefault()?.EntityTypeId;
            //Create new user configurations
            foreach (var configurationSchema in configurationSchemas)
            {
                AddNewUserConfiguration(userId, readonlyListOfSchemaNames, newUserConfigurations, displayOrder, configurationSchema);
                if (entityType == configurationSchema.EntityTypeId)
                    displayOrder++;
                else if (entityType != configurationSchema.EntityTypeId)
                {
                    entityType = configurationSchema.EntityTypeId;
                    displayOrder = 1;
                }
            }

            //Get user configurations already saved in database
            var userConfigurations = await _userConfigurationRepository.TableNoTracking.TagWith("SaveUserConfiguration").ToListAsync();

            //Only get lists of user configurations that does'nt exist in database against each user
            var userConfigurationsToSeed = newUserConfigurations.Where(x => !(userConfigurations.Select(x => x.UserId).Contains(x.UserId) && userConfigurations.Select(x => x.ConfigurationSchemaId).Contains(x.ConfigurationSchemaId))).ToList();
            await _userConfigurationRepository.Add(userConfigurationsToSeed);
        }

        private static void AddNewUserConfiguration(Guid userId, ReadOnlyCollection<string> readonlyListOfSchemaNames, List<UserConfiguration> newUserConfigurations, int displayOrder, ConfigurationSchema? configurationSchema)
        {
            newUserConfigurations.Add(
                        new UserConfiguration
                        {
                            DisplayName = configurationSchema.SchemaName,
                            IsShown = configurationSchema.SchemaName != nameof(BaseAuditEntity.CreatedAt),
                            IsFilterable = !readonlyListOfSchemaNames.Contains(configurationSchema.SchemaName),
                            IsSortable = !readonlyListOfSchemaNames.Contains(configurationSchema.SchemaName),
                            DisplayOrder = displayOrder,
                            UserId = userId,
                            ConfigurationSchemaId = configurationSchema.Id,
                        }
                    );
        }

        private async Task<Response> AlterPropertiesAndUpdateConfigurations(List<UpdateUserConfigurationDto> updatedConfigurationsData, List<UserConfiguration> savedConfigurations)
        {
            _logger.LogInformation($"{nameof(AlterPropertiesAndUpdateConfigurations)} {ApplicationLogsConstants.MethodRunning}");
            if (!(updatedConfigurationsData.Any() || savedConfigurations.Any())) return new Response<bool>() { Successful = false, Message = ApplicationConstants.NoConfigurationsFoundToUpdate };
            foreach (var savedConfiguration in savedConfigurations)
            {
                savedConfiguration.IsShown = updatedConfigurationsData.SingleOrDefault(x => x.Id! == savedConfiguration.Id)?.IsShown ?? false;
                savedConfiguration.IsFilterable = updatedConfigurationsData.SingleOrDefault(x => x.Id! == savedConfiguration.Id)?.IsFilterable ?? false;
                savedConfiguration.IsSortable = updatedConfigurationsData.SingleOrDefault(x => x.Id! == savedConfiguration.Id)?.IsSortable ?? false;
                savedConfiguration.DisplayName = updatedConfigurationsData.SingleOrDefault(x => x.Id! == savedConfiguration.Id)?.DisplayName ?? string.Empty;
                savedConfiguration.DisplayOrder = updatedConfigurationsData.SingleOrDefault(x => x.Id! == savedConfiguration.Id)?.DisplayOrder ?? 0;
            }
            await _userConfigurationRepository.Update(savedConfigurations);
            _logger.LogInformation($"{nameof(AlterPropertiesAndUpdateConfigurations)} {ApplicationLogsConstants.MethodExecuted}");
            return new Response<bool>() { Successful = true };
        }

      
    }
}
