using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Validators;
using EstateAgent.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateAgent.Dto.ListDtos;

namespace EstateAgent.Business
{
    public class AboutMediaManager : ManagerBase<AboutMediaEntity,AboutMediaEntity>, IAboutMediaService
    {
        private IMediaService _mediaService;
        public AboutMediaManager(BaseEntityValidator<AboutMediaEntity> validator, IMapper mapper, IEntityRepository<AboutMediaEntity> repository, IEntityRepository<AboutMediaEntity> listEntityRepository, IMediaService mediaManager) : base(validator, mapper, repository, listEntityRepository)
        {
            _mediaService = mediaManager;
        }

        public async Task<BussinessLayerResult<bool>> Add(AboutMediaDto aboutMedia)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {

                var mediaResult = await _mediaService.Add(new MediaDto { File = aboutMedia.File });

                if(mediaResult.ResultStatus==Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }


                var entity = new AboutMediaEntity
                {
                    Description= aboutMedia.Description,
                    MediaId=(long)mediaResult.Result,
                    SlideIndex=aboutMedia.SlideIndex,
                    Title = aboutMedia.Title,

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
                        response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(AboutMediaDto aboutMedia)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(aboutMedia.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaUpdateItemNotFoundError, "");
                    return response;
                }

                var mediaResult = await _mediaService.Update(new MediaDto { File = aboutMedia.File,Id=entity.MediaId });

                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                entity.Title = aboutMedia.Title;
                entity.Description = aboutMedia.Description;    
                
                entity.SlideIndex = aboutMedia.SlideIndex;
                
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
                        response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaUpdateValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<AboutMediaListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<AboutMediaListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<AboutMediaListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<AboutMediaListDto>>> LoadMoreFilter(LoadMoreFilter<AboutMediaFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<AboutMediaListDto>>();
            try
            {
                List<AboutMediaEntity> dataList;
                List<AboutMediaListDto> result = new List<AboutMediaListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.SlideIndex == null  || x.SlideIndex==filter.Filter.SlideIndex)

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
                    result.Add(Mapper.Map<AboutMediaListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<AboutMediaListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaGetAllExceptionError, ex.Message);
            }
            return response;
        }


        public async Task<BussinessLayerResult<bool>> ChangePhoto(AboutMediaDto aboutMedia)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {

                var entity = Repository.GetById(aboutMedia.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaUpdateItemNotFoundError, "");
                    return response;
                }
                var mediaResult = (entity.MediaId != null)
                    ? await _mediaService.Update(new MediaDto { File = aboutMedia.File, Id = entity.MediaId })
                    : await _mediaService.Add(new MediaDto { File = aboutMedia.File });



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
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaChangePhotoExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Swap(AboutMediaDto aboutMedia)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {

                var entity = Repository.GetById(aboutMedia.Id);
                var allEntities = Repository.GetAll(x => x.IsDeleted == false && x.SlideIndex != null);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaUpdateItemNotFoundError, "");
                    return response;
                }


                if (aboutMedia.SlideIndex < entity.SlideIndex)
                {
                    foreach (var item in allEntities.Where(x => x.SlideIndex >= aboutMedia.SlideIndex).ToList())
                    {
                        item.SlideIndex += 1;
                        Repository.Update(item);
                    }
                }
                if (aboutMedia.SlideIndex > entity.SlideIndex)
                {
                    foreach (var item in allEntities.Where(x => x.SlideIndex >= entity.SlideIndex && x.SlideIndex <= aboutMedia.SlideIndex).ToList())
                    {
                        item.SlideIndex -= 1;
                        Repository.Update(item);
                    }
                }
                entity.SlideIndex = aboutMedia.SlideIndex;
                Repository.Update(entity);



            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaChangePhotoExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<List<AboutMediaListDto>>> GetAll(AboutMediaFilter filter)
        {
            var response = new BussinessLayerResult<List<AboutMediaListDto>>();
            try
            {
                List<AboutMediaEntity> dataList;
                List<AboutMediaListDto> result = new List<AboutMediaListDto>();
                if (filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        //(filter.Title == null || filter.Title == "" || x.Title.Contains(filter.Title))
                        //&& (filter.Description == null || filter.Description == "" || x.Description.Contains(filter.Description))
                        //&& (filter.PropertyId == null || filter.PropertyId == x.PropertyId)
                         (filter.SlideIndex == null || filter.SlideIndex == x.SlideIndex)
                        && (x.IsDeleted == false)

                        )).OrderBy(x => x.SlideIndex).ToList();
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false).OrderBy(x => x.SlideIndex).ToList();
                }



                for (int i = 0; i < dataList.Count; i++)
                {

                    result.Add(Mapper.Map<AboutMediaListDto>(dataList[i]));
                }


                response.Result = result;




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutMediaAboutMediaGetAllExceptionError, ex.Message);
            }
            return response;
        }


    }
}
