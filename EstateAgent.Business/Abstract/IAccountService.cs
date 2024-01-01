using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Business.Abstract
{
    public interface IAccountService
    {
        public Task<BussinessLayerResult<SessionListDto>> Login(IdentityDto identity);
        public Task<BussinessLayerResult<SessionListDto>> GetSession();
        public Task<BussinessLayerResult<List<MethodTypes>>> AuthorizationControl(long? sessionId);
        public Task<BussinessLayerResult<SessionListDto>> Logout();

    }
}
