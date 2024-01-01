using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Media")]
    public class MediaEntity:EntityBase
    {
        public string MediaName { get; set; }
        public string MediaType { get; set; }
        public string MediaUrl { get; set; }

    }
}
