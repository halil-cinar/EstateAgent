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

namespace EstateAgent.Business
{
    public class RoleMethodManager : ManagerBase<RoleMethodEntity, RoleMethodEntity>, IRoleMethodService
    {
        public RoleMethodManager(BaseEntityValidator<RoleMethodEntity> validator, IMapper mapper, IEntityRepository<RoleMethodEntity> repository, IEntityRepository<RoleMethodEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<bool>> Add(RoleMethodDto roleMethod)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new RoleMethodEntity
                {
                    Role=roleMethod.Role,
                    Method=roleMethod.Method,

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
                        response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(RoleMethodDto roleMethod)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(roleMethod.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodUpdateItemNotFoundError, "");
                    return response;
                }
                entity.Role=roleMethod.Role;
                entity.Method = roleMethod.Method;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<RoleMethodListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<RoleMethodListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<RoleMethodListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>> LoadMoreFilter(LoadMoreFilter<RoleMethodFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>();
            try
            {
                List<RoleMethodEntity> dataList;
                List<RoleMethodListDto> result = new List<RoleMethodListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Method == null || filter.Filter.Method == x.Method)
                        &&(filter.Filter.Role == null || filter.Filter.Role == x.Role)
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
                    result.Add(Mapper.Map<RoleMethodListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<RoleMethodListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<RoleMethodListDto>>> GetAll(RoleMethodFilter filter)
        {
            var response = new BussinessLayerResult<List<RoleMethodListDto>>();
            try
            {
                List<RoleMethodEntity> dataList;
                List<RoleMethodListDto> result = new List<RoleMethodListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Method == null || filter.Method == x.Method)
                        && (filter.Role == null || filter.Role == x.Role)
                        && (x.IsDeleted == false)
                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }

               
                for (int i = 0; i < dataList.Count; i++)
                {
                    
                    result.Add(Mapper.Map<RoleMethodListDto>(dataList[i]));
                }

                response.Result = result;



            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.RoleMethodRoleMethodGetAllExceptionError, ex.Message);
            }
            return response;
        }


    }
}
