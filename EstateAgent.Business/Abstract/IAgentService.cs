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
    public interface IAgentService
    {

        public Task<BussinessLayerResult<bool>> Add(AgentDto agent);
        public Task<BussinessLayerResult<bool>> Update(AgentDto agent);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AgentListDto>>> LoadMoreFilter(LoadMoreFilter<AgentFilter> filter);
        public Task<BussinessLayerResult<AgentListDto>> Get(long id);
        public Task<BussinessLayerResult<bool>> ChangeUser(long agentId, long userId);
        public Task<BussinessLayerResult<bool>> ChangePhoto(AgentDto agent);
        public Task<BussinessLayerResult<List<OptionDto<long>>>> GetOptionList(AgentFilter filter);

    }
}
