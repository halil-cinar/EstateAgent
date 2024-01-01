using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("PropertyMedia")]
    public class PropertyMediaEntity:EntityBase
    {
        public long PropertyId { get; set; }
        public int? SlideIndex { get; set; }

        public long MediaId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(MediaId))]
        public MediaEntity Media { get; set; }

        [ForeignKey(nameof(PropertyId))]
        public PropertyEntity Property { get; set;}
    }
}
