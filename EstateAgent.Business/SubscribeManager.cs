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

namespace EstateAgent.Business
{
    public class SubscribeManager : ManagerBase<SubscribeEntity,SubscribeEntity>, ISubscribeService
    {
        private ISystemSettingService _settingService;


        public SubscribeManager(BaseEntityValidator<SubscribeEntity> validator, IMapper mapper, IEntityRepository<SubscribeEntity> repository, IEntityRepository<SubscribeEntity> listEntityRepository, ISystemSettingService settingService) : base(validator, mapper, repository, listEntityRepository)
        {
            _settingService = settingService;
        }

        public async Task<BussinessLayerResult<bool>> Add(SubscribeDto subscribe)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new SubscribeEntity
                {
                    Email= subscribe.Email,
                    SubscribedDate=DateTime.Now,

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
                        response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(SubscribeDto subscribe)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(subscribe.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeUpdateItemNotFoundError, "");
                    return response;
                }
                entity.Email = subscribe.Email;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SubscribeListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<SubscribeListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<SubscribeListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<SubscribeListDto>>> LoadMoreFilter(LoadMoreFilter<SubscribeFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<SubscribeListDto>>();
            try
            {
                List<SubscribeEntity> dataList;
                List<SubscribeListDto> result = new List<SubscribeListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Email == null || filter.Filter.Email == "" || x.Email.Contains(filter.Filter.Email))
                        &&(x.IsDeleted==false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x=>x.IsDeleted==false);
                }

                var firstIndex = filter.PageCount * filter.ContentCount;
                var endIndex = firstIndex + filter.ContentCount;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<SubscribeListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<SubscribeListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<Dictionary<long,bool>>>SendEmail(SubscribeFilter filter,string title,string message,bool isHtml=false)
        {
            var response = new BussinessLayerResult<Dictionary<long, bool>>();
            try
            {
                response.Result = new Dictionary<long, bool>();
                var smtpResult = await _settingService.GetSmtpValues();
                if (smtpResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(smtpResult.ErrorMessages);
                    return response;
                }

                var mailSender = new MailSender(smtpResult.Result);

                List<SubscribeEntity> dataList;
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Email == null || filter.Email == "" || x.Email.Contains(filter.Email))
                        && (x.IsDeleted == false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }
                

                foreach (var entity in dataList)
                {
                    var result=  mailSender.SendEmail(title, message, isHtml, entity.Email);
                    response.Result.Add(entity.Id, result);
                }

            }catch(Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.SubscribeSubscribeSendExceptionError, ex.Message);
            }
            return response;
        }

    }
}
