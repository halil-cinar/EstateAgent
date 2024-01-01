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
    public interface ISessionService
    {
        public Task<BussinessLayerResult<long?>> Add(SessionDto session);
        public Task<BussinessLayerResult<bool>> Update(SessionDto session);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<SessionListDto>>> LoadMoreFilter(LoadMoreFilter<SessionFilter> filter);
        public Task<BussinessLayerResult<SessionListDto>> Get(long id);
        public Task<BussinessLayerResult<List<SessionListDto>>> GetAll(SessionFilter filter);

    }
}
