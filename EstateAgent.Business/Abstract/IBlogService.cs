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
    public interface IBlogService
    {

        public Task<BussinessLayerResult<bool>> Add(BlogDto blog);
        public Task<BussinessLayerResult<bool>> Update(BlogDto blog);
        public Task<BussinessLayerResult<bool>> Remove(long id);
        public Task<BussinessLayerResult<GenericLoadMoreDto<BlogListDto>>> LoadMoreFilter(LoadMoreFilter<BlogFilter> filter);
        public Task<BussinessLayerResult<BlogListDto>> Get(long id);
        public Task<BussinessLayerResult<bool>> ChangePhoto(BlogDto blog);
    }
}
