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
    public interface ISubscribeService
    {

        public Task<BussinessLayerResult<bool>> Add(SubscribeDto subscribe);
        public Task<BussinessLayerResult<bool>> Update(SubscribeDto subscribe);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<SubscribeListDto>>> LoadMoreFilter(LoadMoreFilter<SubscribeFilter> filter);
        public Task<BussinessLayerResult<SubscribeListDto>> Get(long id);
        public Task<BussinessLayerResult<Dictionary<long, bool>>> SendEmail(SubscribeFilter filter, string title, string message, bool isHtml = false);

    }
}
