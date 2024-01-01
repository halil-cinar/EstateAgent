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
    public interface IMessageService
    {
        public Task<BussinessLayerResult<bool>> Send(MessageDto message);
        //public Task<BussinessLayerResult<bool>> Recieve(MessageDto message);

        //public Task<BussinessLayerResult<bool>> Update(MessageDto message);
        //public Task<BussinessLayerResult<bool>> Remove(long id);
        //public Task<BussinessLayerResult<GenericLoadMoreDto<MessageListDto>>> LoadMoreFilter(LoadMoreFilter<MessageFilter> filter);
        //public Task<BussinessLayerResult<MessageListDto>> Get(long id);
    }
}
