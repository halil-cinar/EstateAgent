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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class SessionManager : ManagerBase<SessionEntity, SessionListEntity>, ISessionService
    {
        public SessionManager(BaseEntityValidator<SessionEntity> validator, IMapper mapper, IEntityRepository<SessionEntity> repository, IEntityRepository<SessionListEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<long?>> Add(SessionDto session)
        {
            var response = new BussinessLayerResult<long?>();

            try
            {
                var entity = new SessionEntity
                {
                    CreateTime= DateTime.Now,
                    ExpiryDate= session.ExpiryDate,
                    IdentityId= session.IdentityId,
                    IpAddress= session.IpAddress,
                    IsActive= session.IsActive,
                    UserId= session.UserId,
                    


                    IsDeletable = true,
                    IsDeleted = false
                };
                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Add(entity);
                    response.Result = entity.Id;
                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(SessionDto session)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(session.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionUpdateItemNotFoundError, "");
                    return response;
                }
                
                entity.ExpiryDate = session.ExpiryDate;
                entity.IdentityId = session.IdentityId;
                entity.IpAddress = session.IpAddress;
                entity.IsActive = session.IsActive;
                entity.UserId = session.UserId;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<SessionListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<SessionListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<SessionListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<SessionListDto>>> LoadMoreFilter(LoadMoreFilter<SessionFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<SessionListDto>>();
            try
            {
                List<SessionListEntity> dataList;
                List<SessionListDto> result = new List<SessionListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.IpAddress == null || filter.Filter.IpAddress == "" || x.IpAddress.Contains(filter.Filter.IpAddress))
                        && (filter.Filter.IdentityId == null || filter.Filter.IdentityId == x.IdentityId)
                        && (filter.Filter.UserId == null || filter.Filter.UserId == x.UserId)
                        && (filter.Filter.IsActive == null || (filter.Filter.IsActive==true &&  filter.Filter.IsActive == x.IsActive && x.ExpiryDate<DateTime.Now)||(filter.Filter.IsActive==false &&  filter.Filter.IsActive == x.IsActive))
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
                    result.Add(Mapper.Map<SessionListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<SessionListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionGetAllExceptionError, ex.Message);
            }
            return response;
        }


        public async Task<BussinessLayerResult<List<SessionListDto>>> GetAll(SessionFilter filter)
        {
            var response = new BussinessLayerResult<List<SessionListDto>>();
            try
            {
                List<SessionListEntity> dataList;
                List<SessionListDto> result = new List<SessionListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.IpAddress == null || filter.IpAddress == "" || x.IpAddress.Contains(filter.IpAddress))
                        && (filter.IdentityId == null || filter.IdentityId == x.IdentityId)
                        && (filter.UserId == null || filter.UserId == x.UserId)
                        && (filter.IsActive == null || (filter.IsActive == true && filter.IsActive == x.IsActive && x.ExpiryDate > DateTime.Now) || (filter.IsActive == false && filter.IsActive == x.IsActive))
                        && (x.IsDeleted == false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }


                for (int i = 0; i < dataList.Count; i++)
                {
                    
                    result.Add(Mapper.Map<SessionListDto>(dataList[i]));
                }


                response.Result = result;




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.SessionSessionGetAllExceptionError, ex.Message);
            }
            return response;
        }
    }
}
