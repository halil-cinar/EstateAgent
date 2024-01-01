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
    public interface IContactInfoService
    {

        public Task<BussinessLayerResult<bool>> Add(ContactInfoDto contactInfo);
        public Task<BussinessLayerResult<bool>> Update(ContactInfoDto contactInfo);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<ContactInfoListDto>>> LoadMoreFilter(LoadMoreFilter<ContactInfoFilter> filter);
        public Task<BussinessLayerResult<ContactInfoListDto>> Get(long id);
    }
}
