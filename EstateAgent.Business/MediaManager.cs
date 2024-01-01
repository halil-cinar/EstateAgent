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
    public class MediaManager : ManagerBase<MediaEntity,MediaEntity>, IMediaService
    {
        public MediaManager(BaseEntityValidator<MediaEntity> validator, IMapper mapper, IEntityRepository<MediaEntity> repository, IEntityRepository<MediaEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
        }

        string path = "wwwroot/Medias";

        public async Task<BussinessLayerResult<long?>> Add(MediaDto media)
        {
            var response = new BussinessLayerResult<long?>();
            try
            {
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileName=Guid.NewGuid().ToString()+Path.GetExtension(media.File.FileName);
                var filePath=Path.Combine(path, fileName);

                using(var fs = new FileStream(filePath,FileMode.Create))
                {
                    await media.File.CopyToAsync(fs);
                }

                var entity = new MediaEntity
                {
                    MediaName=media.File.FileName,
                    MediaType=media.File.ContentType,
                    MediaUrl=filePath,
                   
                    
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
                        response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaDeleteItemNotFoundError, "");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaDeleteExceptionError,
                                  ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<long?>> Update(MediaDto media)
        {
            var response = new BussinessLayerResult<long?>();
            try
            {
                var entity = Repository.GetById(media.Id);
                if (entity == null)
                {
                    response.Result = null;
                    response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaUpdateItemNotFoundError, "");
                    return response;
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                

                using (var fs = new FileStream(entity.MediaUrl, FileMode.Truncate))
                {
                    await media.File.CopyToAsync(fs);
                }
                entity.MediaType = media.File.ContentType;
                entity.MediaName = media.File.FileName;

                var validationResult = Validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    Repository.Update(entity);
                    response.Result = entity.Id;
                }
                else
                {
                    response.Result = null;
                    foreach (var err in validationResult.Errors)
                    {
                        response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaUpdateValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<MediaListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<MediaListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaGetItemNotFoundError, "");

                }
                else
                {

                    var dto = new MediaListDto
                    {
                        Id= id,
                        MediaName = entity.MediaName,
                        MediaType = entity.MediaType,
                        Media = await File.ReadAllBytesAsync(entity.MediaUrl)
                    };
                    response.Result = dto;

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaGetExceptionError, ex.Message);
            }
            return response;

        }

        //public async Task<BussinessLayerResult<GenericLoadMoreDto<MediaListDto>>> LoadMoreFilter(LoadMoreFilter<MediaFilter> filter)
        //{
        //    var response = new BussinessLayerResult<GenericLoadMoreDto<MediaListDto>>();
        //    try
        //    {
        //        List<MediaEntity> dataList;
        //        List<MediaListDto> result = new List<MediaListDto>();
        //        if (filter.Filter != null)
        //        {
        //            dataList = ListEntityRepository.GetAll(x =>
        //                (
        //                (filter.Filter.MediaType == null || filter.Filter.MediaType == "" || x.MediaType.Contains(filter.Filter.MediaType))
        //                && (filter.Filter.MediaUrl == null || filter.Filter.MediaUrl == "" || x.MediaUrl.Contains(filter.Filter.MediaUrl))

        //                ));
        //        }
        //        else
        //        {
        //            dataList = ListEntityRepository.GetAll();
        //        }

        //        var firstIndex = filter.PageCount * filter.ContentCount;
        //        var endIndex = firstIndex + filter.ContentCount;

        //        for (int i = firstIndex; i < endIndex; i++)
        //        {
        //            if (i >= dataList.Count)
        //            {
        //                break;
        //            }
        //            result.Add(Mapper.Map<MediaListDto>(dataList[i]));
        //        }

        //        response.Result = new GenericLoadMoreDto<MediaListDto>
        //        {
        //            Values = result,
        //            ContentCount = filter.ContentCount,
        //            PageCount = filter.PageCount,
        //            NextPage = endIndex < dataList.Count,
        //            PrevPage = firstIndex > 0,
        //            TotalContentCount = dataList.Count,
        //            TotalPageCount = dataList.Count / filter.ContentCount
        //        };




        //    }
        //    catch (Exception ex)
        //    {
        //        response.Result = null;
        //        response.AddError(Dto.Enums.ErrorMessageCode.MediaMediaGetAllExceptionError, ex.Message);
        //    }
        //    return response;
        //}

    }
}
