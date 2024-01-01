using AutoMapper;
using EstateAgent.Business.Abstract;
using EstateAgent.DataAccess;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities;
using EstateAgent.Entities.Enums;
using EstateAgent.Entities.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EstateAgent.Business
{
    public class PropertyManager : ManagerBase<PropertyEntity,PropertyListEntity>, IPropertyService
    {
        private IPropertyMediaService _propertyMediaService;
        private ISubscribeService _subscribeService;
        public PropertyManager(BaseEntityValidator<PropertyEntity> validator, IMapper mapper, IEntityRepository<PropertyEntity> repository, IEntityRepository<PropertyListEntity> listEntityRepository, IPropertyMediaService propertyMediaService, ISubscribeService subscribeService) : base(validator, mapper, repository, listEntityRepository)
        {
            _propertyMediaService = propertyMediaService;
            _subscribeService = subscribeService;
        }

        public async Task<BussinessLayerResult<bool>> Add(PropertyDto property)
        {
            var response = new BussinessLayerResult<bool>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = new PropertyEntity
                    {
                        Address = property.Address,
                        AgentId = property.AgentId,
                        Details = property.Details,
                        KitchenCount = property.KitchenCount,
                        LivingRoomCount = property.LivingRoomCount,
                        ParkingCount = property.ParkingCount,
                        BedRoomCount = property.BedRoomCount,
                        PropertySaleStatus = property.PropertySaleStatus,
                        Price = property.Price,
                        LocationLatitude = property.LocationLatitude,
                        LocationLongitude = property.LocationLongitude,
                        Title = property.Title,

                        IsDeletable = true,
                        IsDeleted = false
                    };
                    var validationResult = Validator.Validate(entity);
                    if (validationResult.IsValid)
                    {
                        Repository.Add(entity);

                        foreach (var file in property.Files)
                        {
                            var mediaResult = await _propertyMediaService.Add(new PropertyMediaDto
                            {
                                Description = "",
                                Title = "",
                                SlideIndex = 0,
                                PropertyId = entity.Id,
                                File = file

                            });
                            if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Error)
                            {
                                scope.Dispose();
                                response.ErrorMessages.AddRange(mediaResult.ErrorMessages.ToList());
                                return response;
                            }
                        }
                         _subscribeService.SendEmail(
                            filter:null,
                            title: "Yeni Emlak İlanı",
                            message: "<!DOCTYPE html> <html lang=\"tr\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Yeni Emlak İlanı</title> <style> body { font-family: Arial, sans-serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f4f4f4; } .container { max-width: 600px; margin: 20px auto; background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); } h2 { color: #333; } p { color: #666; } .property-details { margin-top: 20px; border-top: 1px solid #ccc; padding-top: 20px; } .cta-button { display: inline-block; padding: 10px 20px; text-decoration: none; background-color: #3498db; color: #fff; border-radius: 3px; } </style> </head> <body> <div class=\"container\"> <h2>Yeni Emlak İlanı</h2> <p>Merhaba Değerli Müşterimiz,</p> <p>Portföyümüze yeni eklediğimiz bir emlak ilanını sizinle paylaşmak isteriz.</p> <div class=\"property-details\"> <h3>Emlak Detayları:</h3> <p><strong>Adres:</strong> "+entity.Address+"</p> <p><strong>Fiyat:</strong> ₺"+entity.Price+ "</p> <p><strong>Yatak Odası Sayısı:</strong>" + entity.BedRoomCount + " </p>  <p><strong>Oturma Odası Sayısı:</strong>"+entity.LivingRoomCount+ " </p>  <p><strong>Park Sayısı:</strong>"+entity.ParkingCount+ " </p>  <p><strong>Mutfak Sayısı:</strong>"+entity.KitchenCount+" </p>  </div> <p>Emlak ile ilgileniyorsanız, lütfen bizimle iletişime geçmekten çekinmeyin.</p> </div> </body> </html>",
                            isHtml: true
                            );
                        scope.Complete();
                       
                        response.Result = true;
                    }
                    else
                    {
                        scope.Dispose();
                        response.Result = false;
                        foreach (var err in validationResult.Errors)
                        {
                            response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyAddValidationError, err.ErrorMessage);
                        }

                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyAddExceptionError, ex.Message);
                }
                
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
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyDeleteItemNotFoundError, "item is not found");
                }

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyDeleteExceptionError, ex.Message);
            }
            return response;
        }

        public async Task<BussinessLayerResult<bool>> Update(PropertyDto property)
        {
            var response = new BussinessLayerResult<bool>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var entity = Repository.GetById(property.Id);
                    if (entity == null)
                    {
                        response.Result = false;
                        response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyUpdateItemNotFoundError, "");
                        return response;
                    }

                    entity.Address = property.Address;
                    entity.AgentId = property.AgentId;
                    entity.Details = property.Details;
                    entity.KitchenCount = property.KitchenCount;
                    entity.LivingRoomCount = property.LivingRoomCount;
                    entity.ParkingCount = property.ParkingCount;
                    entity.BedRoomCount = property.BedRoomCount;
                    entity.PropertySaleStatus = property.PropertySaleStatus;
                    entity.Price = property.Price;
                    entity.LocationLatitude = property.LocationLatitude;
                    entity.LocationLongitude = property.LocationLongitude;
                    entity.Title = property.Title;

                    var validationResult = Validator.Validate(entity);
                    if (validationResult.IsValid)
                    {
                        Repository.Update(entity);

                        

                        scope.Complete();
                        response.Result = true;
                    }
                    else
                    {
                        scope.Dispose();
                        response.Result = false;
                        foreach (var err in validationResult.Errors)
                        {
                            response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyUpdateValidationError, err.ErrorMessage);
                        }

                    }

                }
                catch (Exception ex)
                {
                    response.Result = false;
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyUpdateExceptionError, ex.Message);
                }
            }
            return response;
        }

        

        public async Task<BussinessLayerResult<PropertyListDto>> Get(long id)
        {
            var response = new BussinessLayerResult<PropertyListDto>();
            try
            {
                var entity = ListEntityRepository.GetById(id);
                if (entity == null)
                {
                    response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyGetItemNotFoundError, "item is not found");

                }
                else
                {
                    response.Result = Mapper.Map<PropertyListDto>(entity);
                    var mediaResult = await _propertyMediaService.GetAll(new PropertyMediaFilter
                    {
                        PropertyId = entity.Id,
                    });
                    if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Success)
                    {
                        response.Result.PropertyMediaLists = mediaResult.Result;
                    }
                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyGetExceptionError, ex.Message);
            }
            return response;

        }

        public async Task<BussinessLayerResult<GenericLoadMoreDto<PropertyListDto>>> LoadMoreFilter(LoadMoreFilter<PropertyFilter> filter)
        {
            var response = new BussinessLayerResult<GenericLoadMoreDto<PropertyListDto>>();
            try
            {
                List<PropertyListEntity> dataList;
                List<PropertyListDto> result = new List<PropertyListDto>();
                if (filter.Filter != null)
                {
                    string[] searchKeys = null;
                    if (filter?.Filter?.Search != null && filter.Filter.Search != "")
                    {
                        searchKeys = filter.Filter.Search.Split(" ");
                    }
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Filter.Title == null || filter.Filter.Title == "" || x.Title.Contains(filter.Filter.Title))
                        && (filter.Filter.Address == null || filter.Filter.Address == "" || x.Address.Contains(filter.Filter.Address))
                        && (filter.Filter.AgentId == null || filter.Filter.AgentId == x.AgentId)
                        && (filter.Filter.PropertySaleStatus == null || filter.Filter.PropertySaleStatus == x.PropertySaleStatus)
                        && (filter.Filter.MinPrice == null || filter.Filter.MinPrice <= x.Price)
                        && (filter.Filter.MaxPrice == null || filter.Filter.MaxPrice == x.Price)
                        && (filter.Filter.MaxParkingCount == null || filter.Filter.MaxParkingCount >= x.ParkingCount)
                        && (filter.Filter.MinParkingCount == null || filter.Filter.MinParkingCount <= x.ParkingCount)
                        && (filter.Filter.MaxKitchenCount == null || filter.Filter.MaxKitchenCount >= x.KitchenCount)
                        && (filter.Filter.MinKitchenCount == null || filter.Filter.MinKitchenCount <= x.KitchenCount)
                        && (filter.Filter.MaxBedRoomCount == null || filter.Filter.MaxBedRoomCount >= x.BedRoomCount)
                        && (filter.Filter.MinBedRoomCount == null || filter.Filter.MinBedRoomCount <= x.BedRoomCount)
                        && (filter.Filter.MaxLivingRoomCount == null || filter.Filter.MaxLivingRoomCount >= x.LivingRoomCount)
                        && (filter.Filter.MinLivingRoomCount == null || filter.Filter.MinLivingRoomCount <= x.LivingRoomCount)

                        && (filter.Filter.Search == null || filter.Filter.Search == ""
                            || x.Title.Contains(filter.Filter.Search)
                            || x.Details.Contains(filter.Filter.Search) == true
                            || x.Address.Contains(filter.Filter.Search)
                        )
                        && (x.IsDeleted == false)

                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }

                var firstIndex = filter.PageCount * filter.ContentCount;
                var endIndex = firstIndex + filter.ContentCount;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<PropertyListDto>(dataList[i]));
                }
                foreach (var item in result)
                {
                    var mediaResult = await _propertyMediaService.GetAll(new PropertyMediaFilter
                    {
                        PropertyId = item.Id,
                    });
                    if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Success)
                    {
                        item.PropertyMediaLists = mediaResult.Result;
                    }

                }

                response.Result = new GenericLoadMoreDto<PropertyListDto>
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
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyGetAllExceptionError, ex.Message);
            }
            return response;
        }
        public async Task<BussinessLayerResult<List<PropertyListDto>>> GetAll(PropertyFilter filter)
        {
            var response = new BussinessLayerResult<List<PropertyListDto>>();
            try
            {
                List<PropertyListEntity> dataList;
                List<PropertyListDto> result = new List<PropertyListDto>();
                if (filter != null)
                {
                    string[] searchKeys = null;
                    if (filter?.Search != null && filter.Search != "")
                    {
                        searchKeys = filter.Search.Split(" ");
                    }
                    dataList = ListEntityRepository.GetAll(x =>
                        (
                        (filter.Title == null || filter.Title == "" || x.Title.Contains(filter.Title))
                        && (filter.Address == null || filter.Address == "" || x.Address.Contains(filter.Address))
                        && (filter.AgentId == null || filter.AgentId == x.AgentId)
                        && (filter.PropertySaleStatus == null || filter.PropertySaleStatus == x.PropertySaleStatus)
                        && (filter.MinPrice == null || filter.MinPrice <= x.Price)
                        && (filter.MaxPrice == null || filter.MaxPrice == x.Price)
                        && (filter.MaxParkingCount == null || filter.MaxParkingCount >= x.ParkingCount)
                        && (filter.MinParkingCount == null || filter.MinParkingCount <= x.ParkingCount)
                        && (filter.MaxKitchenCount == null || filter.MaxKitchenCount >= x.KitchenCount)
                        && (filter.MinKitchenCount == null || filter.MinKitchenCount <= x.KitchenCount)
                        && (filter.MaxBedRoomCount == null || filter.MaxBedRoomCount >= x.BedRoomCount)
                        && (filter.MinBedRoomCount == null || filter.MinBedRoomCount <= x.BedRoomCount)
                        && (filter.MaxLivingRoomCount == null || filter.MaxLivingRoomCount >= x.LivingRoomCount)
                        && (filter.MinLivingRoomCount == null || filter.MinLivingRoomCount <= x.LivingRoomCount)

                        && (filter.Search == null || filter.Search == ""
                            || x.Title.Contains(filter.Search)
                            || x.Details.Contains(filter.Search) == true
                            || x.Address.Contains(filter.Search)
                        )
                        && (x.IsDeleted == false)

                        ));
                }
                else
                {
                    dataList = ListEntityRepository.GetAll(x => x.IsDeleted == false);
                }

                var firstIndex = 0;
                var endIndex = dataList.Count;

                for (int i = firstIndex; i < endIndex; i++)
                {
                    if (i >= dataList.Count)
                    {
                        break;
                    }
                    result.Add(Mapper.Map<PropertyListDto>(dataList[i]));
                }
                foreach (var item in result)
                {
                    var mediaResult = await _propertyMediaService.GetAll(new PropertyMediaFilter
                    {
                        PropertyId = item.Id,
                    });
                    if (mediaResult.ResultStatus == Dto.Enums.ResultStatus.Success)
                    {
                        item.PropertyMediaLists = mediaResult.Result;
                    }

                }

                response.Result = result;




            }
            catch (Exception ex)
            {
                response.Result = null;
                response.AddError(Dto.Enums.ErrorMessageCode.PropertyPropertyGetAllExceptionError, ex.Message);
            }
            return response;
        }


    }
}
