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
    public interface IMediaService
    {

        public Task<BussinessLayerResult<long?>> Add(MediaDto media);
        public Task<BussinessLayerResult<long?>> Update(MediaDto media);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        //public Task<BussinessLayerResult<GenericLoadMoreDto<MediaListDto>>> LoadMoreFilter(LoadMoreFilter<MediaFilter> filter);
        public Task<BussinessLayerResult<MediaListDto>> Get(long id);
        
    }
}
