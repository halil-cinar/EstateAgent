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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business
{
    public class BlogManager : ManagerBase<BlogEntity,BlogListEntity>, IBlogService
    {
        private IMediaService _mediaService;
        public BlogManager(BaseEntityValidator<BlogEntity> validator, IMapper mapper, IEntityRepository<BlogEntity> repository, IEntityRepository<BlogListEntity> listEntityRepository, IMediaService mediaService) : base(validator, mapper, repository, listEntityRepository)
        {
            _mediaService = mediaService;
        }

        public async Task<BussinessLayerResult<bool>> Add(BlogDto blog)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var mediaResult = await _mediaService.Add(new MediaDto { File = blog.File });

                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                var entity = new BlogEntity
                {
                    Content = blog.Content,
                    Title = blog.Title,
                    Description=blog.Description,
                    PostedDate=DateTime.Now,
                    UserId=blog.UserId,
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
                        response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogAddExceptionError, ex.Message);
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
                    response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(BlogDto blog)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {

                var entity = Repository.GetById(blog.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogUpdateItemNotFoundError, "");
                    return response;
                }
               
                entity.Content = blog.Content;
                entity.Title = blog.Title;
                entity.Description = blog.Description;
                entity.PostedDate = DateTime.Now;
                
                

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
                        response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogUpdateValidationError, err.ErrorMessage);
                    }


                }




            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogUpdateExceptionError, ex.Message);
            }
            return response;
        }


        public async Task<BussinessLayerResult<bool>> ChangePhoto(BlogDto blog)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {

                var entity = Repository.GetById(blog.Id);
                if (entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogUpdateItemNotFoundError, "");
                    return response;
                }

                var mediaResult = (entity.MediaId == null)
                    ? await _mediaService.Add(new MediaDto { File = blog.File })
                    : await _mediaService.Update(new MediaDto { File = blog.File, Id = entity.MediaId });


                if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                    return response;
                }

                entity.MediaId = (long) mediaResult.Result;

                Repository.Update(entity);

                response.Result = true;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogUpdateExceptionError, ex.Message);
            }
            return response;
        }



        public async Task<BussinessLayerResult<BlogListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<BlogListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<BlogListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>> LoadMoreFilter(LoadMoreFilter<BlogFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>();
            try
            {
                List<BlogListEntity> dataList;
                List<BlogListDto> result = new List<BlogListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Title == null || filter.Filter.Title == "" || x.Title.Contains(filter.Filter.Title))
                        && (filter.Filter.UserId == null || x.UserId==filter.Filter.UserId)
                        &&(x.IsDeleted==false)
                        ));
                    if(filter.Filter.Order!= null )
                    {
                        dataList=((filter.Filter.Order.PostedDateDirection==true)
                            ?dataList.OrderBy(x=>x.PostedDate)
                            :dataList.OrderByDescending(x => x.PostedDate)
                            
                            ).ToList();
                    }
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
                    result.Add(Mapper.Map<BlogListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<BlogListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.BlogBlogGetAllExceptionError, ex.Message);
            }
            return response;
        }

    }
}
