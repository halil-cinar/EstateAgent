using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Blog")]
    public class BlogEntity:EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
        public DateTime PostedDate { get; set; }

        public long MediaId { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(MediaId))]
        public MediaEntity Media { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }


    }
}
