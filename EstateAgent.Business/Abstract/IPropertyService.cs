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
    public interface IPropertyService
    {

        public Task<BussinessLayerResult<bool>> Add(PropertyDto property);
        public Task<BussinessLayerResult<bool>> Update(PropertyDto property);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<PropertyListDto>>> LoadMoreFilter(LoadMoreFilter<PropertyFilter> filter);
        public Task<BussinessLayerResult<PropertyListDto>> Get(long id);
        public Task<BussinessLayerResult<List<PropertyListDto>>> GetAll(PropertyFilter filter);

    }
}
