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
    public interface IAboutService
    {

        public Task<BussinessLayerResult<bool>> Add(AboutDto about);
        public Task<BussinessLayerResult<bool>> Update(AboutDto about);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AboutListDto>>> LoadMoreFilter(LoadMoreFilter<AboutFilter> filter);
        public Task<BussinessLayerResult<AboutListDto>> Get(long id);
    }
}
