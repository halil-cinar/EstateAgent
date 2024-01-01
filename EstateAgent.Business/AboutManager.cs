using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities;
using EstateAgent.Entities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace EstateAgent.Business
{
    public class AboutManager : ManagerBase<AboutEntity,AboutEntity>, IAboutService
    {
     
        public AboutManager(BaseEntityValidator<AboutEntity> validator, IMapper mapper, IEntityRepository<AboutEntity> repository, IEntityRepository<AboutEntity> listEntityRepository) : base(validator, mapper, repository, listEntityRepository)
        {
          
        }

        public async Task<BussinessLayerResult<bool>> Add(AboutDto about)
        {
            var response = new BussinessLayerResult<bool>();

            try
            {
                var entity = new AboutEntity
                {
                    Content = about.Content,
                    Title = about.Title,
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
                        response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutAddValidationError, err.ErrorMessage);
                    }


                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutAddExceptionError, ex.Message);
            }
            
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Remove(long id)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity=Repository.GetById(id);
                if(entity!=null)
                {
                    entity.IsDeleted = true;
                    Repository.Update(entity);
                    response.Result= true;

                }
                else
                {
                    response.Result= false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(AboutDto about)
        {
            var response = new BussinessLayerResult<bool>();
            try
            {
                var entity = Repository.GetById(about.Id);
                if(entity == null)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutUpdateItemNotFoundError, "");
                    return response;
                }
                entity.Title = about.Title;
                entity.Content = about.Content;

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
                        response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutUpdateValidationError, err.ErrorMessage);
                    }


                }

               


            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutUpdateExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<AboutListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<AboutListDto>();
            try
            {
                var entity=ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result=Mapper.Map<AboutListDto>(entity);
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutGetExceptionError, ex.Message);
            }
            return response;
            
        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>> LoadMoreFilter(LoadMoreFilter<AboutFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>();
            try
            {
                List<AboutEntity> dataList;
                List<AboutListDto> result= new List<AboutListDto>();
                if (filter.Filter != null)
                {
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Title == null || filter.Filter.Title == "" || x.Title.Contains(filter.Filter.Title))
                        && (filter.Filter.Content == null || filter.Filter.Content == "" || x.Content.Contains(filter.Filter.Content))

                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll();
                }

                var firstIndex = filter.PageCount * filter.ContentCount;
                var endIndex= firstIndex+ filter.ContentCount;
                
                for(int i=firstIndex; i<endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<AboutListDto>(dataList[i]));
                }

                response.Result = new GenericLoadMoreDto<AboutListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.AboutAboutGetAllExceptionError, ex.Message);
            }
            return response;
        }
 
    

    }
}
