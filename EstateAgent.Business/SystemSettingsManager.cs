using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.Core.ExtensionsMethods;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities;
using EstateAgent.Entities.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EstateAgent.Business
{
    public class SystemSettingsManager : ManagerBase<SystemSettingsEntity,SystemSettingsEntity>, ISystemSettingService
    {
        private IMediaService _mediaService;
        public SystemSettingsManager(BaseEntityValidator<SystemSettingsEntity> validator, IMapper mapper, IEntityRepository<SystemSettingsEntity> repository, IEntityRepository<SystemSettingsEntity> listEntityRepository, IMediaService mediaService) : base(validator, mapper, repository, listEntityRepository)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<bool>> Add(SystemSettingsDto systemSettings)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new SystemSettingsEntity
                {
                    Key=systemSettings.Key,
                    Name=systemSettings.Name,
                    Value=systemSettings.Value,

                    IsDeletable = true,
                    IsDeleted = false
                };
                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Add(entity);
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsAddExceptionError, ex.Message);
            }

            return response;
        }

        public async Task<BussinessLayerResult<bool>> ChangeLogo(LogoDto logo)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity= Repository.GetById(logo.Id);
                var mediaResult = (string.IsNullOrEmpty(entity.Value))
                    ? await _mediaService.Add(new MediaDto { File = logo.File })
                    : await _mediaService.Update(new MediaDto { File = logo.File,Id=Convert.ToInt64(entity.Value) });
                if(mediaResult.ResultStatus==Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages);
                    return response;
                }

                entity.Value = mediaResult.Result.ToString();
                response.Result = true;
                Repository.Update(entity);
            }catch(Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsChangeLogoExceptionError,ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Remove(long id)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    Repository.Update(entity);
                    response.Result = true;

                }
                else
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(SystemSettingsDto systemSettings)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(systemSettings.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsUpdateItemNotFoundError, "");
                    return response;
                }
               entity. Key = systemSettings.Key;
               entity. Name = systemSettings.Name;
               entity. Value = systemSettings.Value;

                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Update(entity);
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SystemSettingsListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<SystemSettingsListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<SystemSettingsListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<SystemSettingsListDto>> Get(string key)
        {
            var response = new BussinessLayerResult<SystemSettingsListDto>();
            try
            {
                var entity = ListEntityRepository.Get(x=>x.Key==key);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<SystemSettingsListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<SmtpValues>> GetSmtpValues()
        {
            var response = new BussinessLayerResult<SmtpValues>();
            try
            {
                var result=new SmtpValues
                {
                    SmtpServer = ListEntityRepository.Get(x => x.Key == "smtpServer").Value,
                    SmtpPort = Convert.ToInt32(ListEntityRepository.Get(x => x.Key == "smtpPort").Value),
                    SmtpUsername = ListEntityRepository.Get(x => x.Key == "smtpDisplayName").Value,
                    SmtpPassword = ListEntityRepository.Get(x => x.Key == "smtpPassword").Value,
                    SmtpDisplayName = ListEntityRepository.Get(x => x.Key == "smtpDisplayName").Value,
                    SmtpEnableSsl = Convert.ToBoolean(Convert.ToInt32(ListEntityRepository.Get(x => x.Key == "smtpEnableSsl").Value)),
                    SmtpDisplayAddress=ListEntityRepository.Get(x=>x.Key=="smtpDisplayAddress").Value
                    
                };
                response.Result = result;

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetExceptionError, ex.Message);
            }
            return response;

        }



        public async Task<BussinessLayerResult<GenericLoadMoreDto<SystemSettingsListDto>>> LoadMoreFilter(LoadMoreFilter<SystemSettingsFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<SystemSettingsListDto>>();
            try
            {
                List<SystemSettingsEntity> dataList;
                List<SystemSettingsListDto> result = new List<SystemSettingsListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Key == null || filter.Filter.Key == "" || x.Key.Contains(filter.Filter.Key))
                        && (filter.Filter.Name == null || filter.Filter.Name == "" || x.Name.Contains(filter.Filter.Name))

                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll();
                }

                var firstIndex = filter.PageCount * filter.ContentCount;
                var endIndex = firstIndex + filter.ContentCount;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<SystemSettingsListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<SystemSettingsListDto>
                {
                    Values = result,
                    ContentCount = filter.ContentCount,
                    PageCount = filter.PageCount,
                    NextPage = endIndex < dataList.Count,
                    PrevPage = firstIndex > 0,
                    TotalContentCount = dataList.Count,
                    TotalPageCount = dataList.Count / filter.ContentCount
                };




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SystemSettingsSystemSettingsGetAllExceptionError, ex.Message);
            }
            return response;
        }

    }
}
