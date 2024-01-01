using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("AboutMedia")]
    public class AboutMediaEntity:EntityBase
    {
        public int? SlideIndex { get; set; }

        public long MediaId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(MediaId))]
        public MediaEntity Media { get; set; }


    }
}
