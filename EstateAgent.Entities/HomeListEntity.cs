using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("HomeListView")]
    public class HomeListEntity:EntityBase
    {
        public int SalePropertyCount { get; set; }
        public int RentPropertyCount { get; set; }

        public int AgentCount { get; set; }
        public int BlogCount { get; set; }

    }
}
