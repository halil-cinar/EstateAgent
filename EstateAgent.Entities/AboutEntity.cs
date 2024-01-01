using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("About")]
    public class AboutEntity:EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        

    }
}
