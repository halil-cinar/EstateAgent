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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EstateAgent.Business
{
    public class ContactInfoManager : ManagerBase<ContactInfoEntity,ContactInfoEntity>, IContactInfoService
    {
        public ContactInfoManager(BaseEntityValidator<ContactInfoEntity> validator, IMapper mapper, IEntityRepository<ContactInfoEntity> repository, IEntityRepository<ContactInfoEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        public async Task<BussinessLayerResult<bool>> Add(ContactInfoDto contactInfo)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new ContactInfoEntity
                {
                    Address= contactInfo.Address,
                    Email= contactInfo.Email,
                    Name= contactInfo.Name,
                    Phone= contactInfo.Phone,
                    FacebookUrl= contactInfo.FacebookUrl,
                    InstagramUrl= contactInfo.InstagramUrl,
                    LinkedinUrl= contactInfo.LinkedinUrl,
                    XUrl= contactInfo.XUrl,
                    LocationLatitude= contactInfo.LocationLatitude,
                    LocationLongitude= contactInfo.LocationLongitude,
                    
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
                        response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(ContactInfoDto contactInfo)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(contactInfo.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoUpdateItemNotFoundError, "");
                    return response;
                }
                entity.Address = contactInfo.Address;
                entity.Email = contactInfo.Email;
                entity.Name = contactInfo.Name;
                entity.Phone = contactInfo.Phone;

                entity.FacebookUrl = contactInfo.FacebookUrl;
                entity.InstagramUrl = contactInfo.InstagramUrl;
                entity.LinkedinUrl = contactInfo.LinkedinUrl;
                entity.XUrl = contactInfo.XUrl;
                entity.LocationLatitude = contactInfo.LocationLatitude;
                entity.LocationLongitude = contactInfo.LocationLongitude;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<ContactInfoListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<ContactInfoListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<ContactInfoListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<ContactInfoListDto>>> LoadMoreFilter(LoadMoreFilter<ContactInfoFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<ContactInfoListDto>>();
            try
            {
                List<ContactInfoEntity> dataList;
                List<ContactInfoListDto> result = new List<ContactInfoListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        //(filter.Filter.Title == null || filter.Filter.Title == "" || x.Title.Contains(filter.Filter.Title))
                        //&& (filter.Filter.Content == null || filter.Filter.Content == "" || x.Content.Contains(filter.Filter.Content))
                        true
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
                    result.Add(Mapper.Map<ContactInfoListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<ContactInfoListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.ContactInfoContactInfoGetAllExceptionError, ex.Message);
            }
            return response;
        }

    }
}
