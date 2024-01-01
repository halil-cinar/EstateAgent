using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Subscribe")]
    public class SubscribeEntity:EntityBase
    {
        public string Email { get; set; }
        public DateTime SubscribedDate { get; set; }


    }
}
