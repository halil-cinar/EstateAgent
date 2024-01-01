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
    public interface IAboutMediaService
    {
        public Task<BussinessLayerResult<bool>> Add(AboutMediaDto aboutMedia);
        public Task<BussinessLayerResult<bool>> Update(AboutMediaDto aboutMedia);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<AboutMediaListDto>>> LoadMoreFilter(LoadMoreFilter<AboutMediaFilter> filter);
        public Task<BussinessLayerResult<AboutMediaListDto>> Get(long id);
        public Task<BussinessLayerResult<bool>> ChangePhoto(AboutMediaDto aboutMedia);
        public Task<BussinessLayerResult<bool>> Swap(AboutMediaDto aboutMedia);
        public Task<BussinessLayerResult<List<AboutMediaListDto>>> GetAll(AboutMediaFilter filter);



    }
}
