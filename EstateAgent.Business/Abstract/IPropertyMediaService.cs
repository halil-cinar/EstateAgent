using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.LoadMoreDtos;
using EstateAgent.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business.Abstract
{
    public interface IPropertyMediaService
    {

        public Task<BussinessLayerResult<bool>> Add(PropertyMediaDto propertyMedia);
        public Task<BussinessLayerResult<bool>> Update(PropertyMediaDto propertyMedia);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PropertyMediaListDto>>> LoadMoreFilter(LoadMoreFilter<PropertyMediaFilter> filter);
        public Task<BussinessLayerResult<PropertyMediaListDto>> Get(long id);
        public Task<BussinessLayerResult<bool>> ChangePhoto(PropertyMediaDto propertyMedia);
        public Task<BussinessLayerResult<bool>> Swap(PropertyMediaDto propertyMedia);
        public Task<BussinessLayerResult<List<PropertyMediaListDto>>> GetAll(PropertyMediaFilter filter);
    }
}
