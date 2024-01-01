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
    public class UserRoleManager : ManagerBase<UserRoleEntity,UserRoleListEntity>, IUserRoleService
    {
        public UserRoleManager(BaseEntityValidator<UserRoleEntity> validator, IMapper mapper, IEntityRepository<UserRoleEntity> repository, IEntityRepository<UserRoleListEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<bool>> Add(UserRoleDto userRole)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new UserRoleEntity
                {
                    UserId= userRole.UserId,
                    Role=userRole.Role,

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
                        response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(UserRoleDto userRole)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(userRole.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleUpdateItemNotFoundError, "");
                    return response;
                }
                entity.UserId = userRole.UserId;
                entity.Role = userRole.Role;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<UserRoleListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<UserRoleListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<UserRoleListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>> LoadMoreFilter(LoadMoreFilter<UserRoleFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>();
            try
            {
                List<UserRoleListEntity> dataList;
                List<UserRoleListDto> result = new List<UserRoleListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.UserId == null ||x.UserId==filter.Filter.UserId)
                        && (filter.Filter.Role == null ||x.Role==filter.Filter.Role)

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
                    result.Add(Mapper.Map<UserRoleListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<UserRoleListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<OptionDto<long>>>> GetOptionList(UserRoleFilter filter)
        {
            var response = new BussinessLayerResult<List<OptionDto<long>>>();
            try
            {
                var dataList =(filter!=null)
                    ?ListEntityRepository.GetAll(x =>
                        (
                        (filter.UserId == null || x.UserId == filter.UserId)
                        && (filter.Role == null || x.Role == filter.Role)
                        &&(x.IsDeleted==false)
                        ))
                    :ListEntityRepository.GetAll(x=>x.IsDeleted==false);
                var list=new List<OptionDto<long>>();

                foreach (var item in dataList)
                {
                    list.Add(new OptionDto<long>
                    {
                        Text=item.UserName+"-"+item.UserSurname+"-"+item.UserPhoneNumber+"-"+item.UserEmail ,
                        Value=item.UserId
                    });
                }
                response.Result = list;
            }catch(Exception ex)
            {
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleOptionListExceptionError, ex.Message);

            }
            return response;
        }

        public async Task<BussinessLayerResult<List<UserRoleListDto>>> GetAll(UserRoleFilter filter)
        {
            var response = new BussinessLayerResult<List<UserRoleListDto>>();
            try
            {
                List<UserRoleListEntity> dataList;
                List<UserRoleListDto> result = new List<UserRoleListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.UserId == null || x.UserId == filter.UserId)
                        && (filter.Role == null || x.Role == filter.Role)

                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll();
                }

               
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<UserRoleListDto>(dataList[i]));
                }

                response.Result = result;




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.UserRoleUserRoleGetAllExceptionError, ex.Message);
            }
            return response;
        }




    }
}
