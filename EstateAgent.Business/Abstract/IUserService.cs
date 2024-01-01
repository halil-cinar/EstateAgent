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
    public interface IUserService
    {

        public Task<BussinessLayerResult<long?>> Add(UserDto user);
        public Task<BussinessLayerResult<long?>> Update(UserDto user);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<UserListDto>>> LoadMoreFilter(LoadMoreFilter<UserFilter> filter);
        public Task<BussinessLayerResult<UserListDto>> Get(long id);
        public Task<BussinessLayerResult<long?>> ChangePhoto(UserDto user);
        public Task<BussinessLayerResult<long?>> ChangeIdentity(UserDto user);
        public Task<BussinessLayerResult<List<UserListDto>>> GetAll(UserFilter filter);
        public Task<BussinessLayerResult<long?>> ResetPassword(long id);
    }
}
