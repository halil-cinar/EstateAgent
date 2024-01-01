using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Filter;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business.Abstract
{
    public interface IIdentityService
    {

        public Task<BussinessLayerResult<long?>> Add(IdentityDto identity);
        public Task<BussinessLayerResult<long?>> ChangePassword(IdentityDto identity);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<IdentityListDto>> Get(long id);
        public Task<BussinessLayerResult<IdentityListDto>> CheckPassword(IdentityDto identity);
        public Task<BussinessLayerResult<IdentityDto>> ResetPassword(long userId);

    }
}
