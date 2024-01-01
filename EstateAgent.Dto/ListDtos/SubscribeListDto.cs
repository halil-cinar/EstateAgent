
using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class SubscribeListDto : ListDtoBase
    {
        public string Email { get; set; }
        public DateTime SubscribedDate { get; set; }


    }
}
