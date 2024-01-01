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
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class PropertyMediaManager : ManagerBase<PropertyMediaEntity,PropertyMediaEntity>, IPropertyMediaService
    {
        private IMediaService _mediaService;
        public PropertyMediaManager(BaseEntityValidator<PropertyMediaEntity> validator, IMapper mapper, IEntityRepository<PropertyMediaEntity> repository, IEntityRepository<PropertyMediaEntity> listEntityRepository, IMediaService mediaService) : base(validator, mapper, repository, listEntityRepository)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<bool>> Add(PropertyMediaDto propertyMedia)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var mediaResult = await _mediaService.Add(new MediaDto { File = propertyMedia.File });

                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                var entity = new PropertyMediaEntity
                {
                    Description= propertyMedia.Description,
                    PropertyId= propertyMedia.PropertyId,
                    SlideIndex= propertyMedia.SlideIndex,
                    Title = propertyMedia.Title,
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
                        response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(PropertyMediaDto propertyMedia)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {

                var entity = Repository.GetById(propertyMedia.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaUpdateItemNotFoundError, "");
                    return response;
                }
                
                entity.Description = propertyMedia.Description;
                entity.PropertyId = propertyMedia.PropertyId;
                entity.SlideIndex = propertyMedia.SlideIndex;
                entity.Title = propertyMedia.Title;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<PropertyMediaListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<PropertyMediaListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<PropertyMediaListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<PropertyMediaListDto>>> LoadMoreFilter(LoadMoreFilter<PropertyMediaFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<PropertyMediaListDto>>();
            try
            {
                List<PropertyMediaEntity> dataList;
                List<PropertyMediaListDto> result = new List<PropertyMediaListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Title == null || filter.Filter.Title == "" || x.Title.Contains(filter.Filter.Title))
                        && (filter.Filter.Description == null || filter.Filter.Description == "" || x.Description.Contains(filter.Filter.Description))
                        && (filter.Filter.PropertyId == null || filter.Filter.PropertyId == x.PropertyId)
                        && (filter.Filter.SlideIndex == null || filter.Filter.SlideIndex == x.SlideIndex)
                        &&(x.IsDeleted==false)

                        )).OrderBy(x=>x.SlideIndex).ToList();
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
                    result.Add(Mapper.Map<PropertyMediaListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<PropertyMediaListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaGetAllExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> ChangePhoto(PropertyMediaDto propertyMedia)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {

                var entity = Repository.GetById(propertyMedia.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaUpdateItemNotFoundError, "");
                    return response;
                }
                var mediaResult = (entity.MediaId != null)
                    ? await _mediaService.Update(new MediaDto { File = propertyMedia.File, Id = entity.MediaId })
                    : await _mediaService.Add(new MediaDto { File = propertyMedia.File});
                    


                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                entity.MediaId = (long)mediaResult.Result;

                Repository.Update(entity);



            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaChangePhotoExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Swap(PropertyMediaDto propertyMedia)
        {
            var response = new BussinessLayerResult<bool>();
            
            try
            {

                var entity = Repository.GetById(propertyMedia.Id);
                var allEntities=Repository.GetAll(x=>x.IsDeleted==false&&x.PropertyId==propertyMedia.PropertyId&&x.SlideIndex!=null);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaUpdateItemNotFoundError, "");
                    return response;
                }

                
                if (propertyMedia.SlideIndex < entity.SlideIndex)
                {
                    foreach (var item in allEntities.Where(x => x.SlideIndex >= propertyMedia.SlideIndex).ToList())
                    {
                        item.SlideIndex += 1;
                        Repository.Update(item);
                    }
                }
                if(propertyMedia.SlideIndex > entity.SlideIndex)
                {
                    foreach (var item in allEntities.Where(x => x.SlideIndex >= entity.SlideIndex&&x.SlideIndex<=propertyMedia.SlideIndex).ToList())
                    {
                        item.SlideIndex -= 1;
                        Repository.Update(item);
                    }
                }
                entity.SlideIndex=propertyMedia.SlideIndex;
                Repository.Update(entity);



            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaChangePhotoExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<PropertyMediaListDto>>> GetAll(PropertyMediaFilter filter)
        {
            var response = new BussinessLayerResult<List<PropertyMediaListDto>>();
            try
            {
                List<PropertyMediaEntity> dataList;
                List<PropertyMediaListDto> result = new List<PropertyMediaListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Title == null || filter.Title == "" || x.Title.Contains(filter.Title))
                        && (filter.Description == null || filter.Description == "" || x.Description.Contains(filter.Description))
                        && (filter.PropertyId == null || filter.PropertyId == x.PropertyId)
                        && (filter.SlideIndex == null || filter.SlideIndex == x.SlideIndex)
                        && (x.IsDeleted == false)

                        )).OrderBy(x => x.SlideIndex).ToList();
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x=>x.IsDeleted==false);
                }

               

                for (int i = 0; i < dataList.Count; i++)
                {
                    
                    result.Add(Mapper.Map<PropertyMediaListDto>(dataList[i]));
                }

                
                response.Result= result;




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyMediaPropertyMediaGetAllExceptionError, ex.Message);
            }
            return response;
        }





    }
}
