using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.LoadMoreDtos
{
    public class GenericLoadMoreDto<T>:BaseLoadMoreDto
        where T : ListDtoBase,new()
    {
        public List<T> Values { get; set; }
    }
}
