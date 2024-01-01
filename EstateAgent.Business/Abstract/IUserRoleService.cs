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
    public interface IUserRoleService
    {

        public Task<BussinessLayerResult<bool>> Add(UserRoleDto userRole);
        public Task<BussinessLayerResult<bool>> Update(UserRoleDto userRole);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<UserRoleListDto>>> LoadMoreFilter(LoadMoreFilter<UserRoleFilter> filter);
        public Task<BussinessLayerResult<UserRoleListDto>> Get(long id);
        public Task<BussinessLayerResult<List<OptionDto<long>>>> GetOptionList(UserRoleFilter filter);
        public Task<BussinessLayerResult<List<UserRoleListDto>>> GetAll(UserRoleFilter filter);

    }
}
