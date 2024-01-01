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
    public interface IRoleMethodService
    {
        public Task<BussinessLayerResult<bool>> Add(RoleMethodDto about);
        public Task<BussinessLayerResult<bool>> Update(RoleMethodDto about);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<RoleMethodListDto>>> LoadMoreFilter(LoadMoreFilter<RoleMethodFilter> filter);
        public Task<BussinessLayerResult<RoleMethodListDto>> Get(long id);
        public Task<BussinessLayerResult<List<RoleMethodListDto>>> GetAll(RoleMethodFilter filter);
    }
}
