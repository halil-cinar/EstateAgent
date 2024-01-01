using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class HomeListDto:ListDtoBase
    {
        public int SalePropertyCount { get; set; }
        public int RentPropertyCount { get; set; }

        public int AgentCount { get; set; }
        public int BlogCount { get; set; }
    }
}
