using AutoMapper;
using EstateAgent.Business.Abstract;
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
using System.Transactions;
using System.Xml.Linq;

namespace EstateAgent.Business
{
    public class AgentManager : ManagerBase<AgentEntity,AgentListEntity>, IAgentService
    {
        private IMediaService _mediaService;
        private IPropertyService _propertyService;

        public AgentManager(BaseEntityValidator<AgentEntity> validator, IMapper mapper, IEntityRepository<AgentEntity> repository, IEntityRepository<AgentListEntity> listEntityRepository, IMediaService mediaService, IPropertyService propertyService) : base(validator, mapper, repository, listEntityRepository)
        {
            _mediaService = mediaService;
            _propertyService = propertyService;
        }

        public async Task<BussinessLayerResult<bool>> Add(AgentDto agent)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {

                var mediaResult = await _mediaService.Add(new MediaDto { File = agent.File });

                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                var entity = new AgentEntity
                {
                    Description= agent.Description,
                    Email= agent.Email,
                    Name= agent.Name,
                    PhoneNumber= agent.PhoneNumber,
                    UserId= agent.UserId,
                    MediaId=(long)mediaResult.Result,


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
                        response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentAddExceptionError, ex.Message);
            }

            return response;
        }

        public async Task<BussinessLayerResult<bool>> Remove(long id)
        {
            var response = new BussinessLayerResult<bool>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Repository.GetById(id);
                    if (entity != null)
                    {
                        var properties = await _propertyService.GetAll(new PropertyFilter
                        {
                            AgentId = entity.Id
                        });
                        if(properties.ResultStatus==Dto.Enums.ResultStatus.Error)
                        {
                            scope.Dispose();
                            response.ErrorMessages.AddRange(properties.ErrorMessages);
                            return response;
                        }
                        foreach (var item in properties.Result)
                        {
                            var result= await _propertyService.Remove(item.Id);
                            if (result.ResultStatus == Dto.Enums.ResultStatus.Error)
                            {
                                scope.Dispose();
                                response.ErrorMessages.AddRange(result.ErrorMessages);
                                return response;
                            }
                        }

                        entity.IsDeleted = true;
                        Repository.Update(entity);
                        response.Result = true;
                        scope.Complete();
                    }
                    else
                    {
                        scope.Dispose();
                        response.Result = false;
                        response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentDeleteItemNotFoundError, "item is not found");
                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();    
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentDeleteExceptionError, ex.Message);
                }
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(AgentDto agent)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(agent.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentUpdateItemNotFoundError, "");
                    return response;
                }
                
                entity.Description = agent.Description;
                entity.Email = agent.Email;
                entity.Name = agent.Name;
                entity.PhoneNumber = agent.PhoneNumber;
                //entity.UserId = agent.UserId;
                

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
                        response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> ChangePhoto(AgentDto agent)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(agent.Id);

                var mediaREsult = (entity.MediaId == null || entity.MediaId <= 0)
                ? await _mediaService.Add(new MediaDto { File = agent.File })
                : await _mediaService.Update(new MediaDto { File = agent.File ,Id=entity.MediaId });

                if (mediaREsult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaREsult.ErrorMessages.ToList());
                    return response;
                }

                entity.MediaId = (long)mediaREsult.Result;
                response.Result = true;

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutChangePhotoExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> ChangeUser(long agentId,long userId)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(agentId);

                entity.UserId= userId;
                Repository.Update(entity);
                
                response.Result = true;

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutChangeUserExceptionError, ex.Message);
            }
            return response;
        }



        public async Task<BussinessLayerResult<AgentListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<AgentListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<AgentListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<AgentListDto>>> LoadMoreFilter(LoadMoreFilter<AgentFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<AgentListDto>>();
            try
            {
                List<AgentListEntity> dataList;
                List<AgentListDto> result = new List<AgentListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.PhoneNumber == null || filter.Filter.PhoneNumber == "" || x.PhoneNumber.Contains(filter.Filter.PhoneNumber))
                        && (filter.Filter.Email == null || filter.Filter.Email == "" || x.Email.Contains(filter.Filter.Email))
                        && (filter.Filter.Name == null || filter.Filter.Name == "" || x.Name.Contains(filter.Filter.Name))
                        && (filter.Filter.UserId == null || x.UserId == filter.Filter.UserId)
                        && (x.IsDeleted ==false)


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
                    result.Add(Mapper.Map<AgentListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<AgentListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<OptionDto<long>>>> GetOptionList(AgentFilter filter)
        {
            var response = new BussinessLayerResult<List<OptionDto<long>>>();
            try
            {
                var dataList = (filter != null)
                    ? ListEntityRepository.GetAll(x =>
                        (
                        (filter.PhoneNumber == null || filter.PhoneNumber == "" || x.PhoneNumber.Contains(filter.PhoneNumber))
                        && (filter.Email == null || filter.Email == "" || x.Email.Contains(filter.Email))
                        && (filter.Name == null || filter.Name == "" || x.Name.Contains(filter.Name))
                        && (filter.UserId == null || x.UserId == filter.UserId)
                        && (x.IsDeleted == false)
                        ))
                    : ListEntityRepository.GetAll(x => x.IsDeleted == false);
                var list = new List<OptionDto<long>>();

                foreach (var item in dataList)
                {
                    list.Add(new OptionDto<long>
                    {
                        Text = item.Name+"-"+item.Email+"-"+item.PhoneNumber,
                        Value = item.Id
                    });
                }
                response.Result = list;
            }
            catch (Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.AgentAgentOptionListExceptionError, ex.Message);

            }
            return response;
        }



    }
}
