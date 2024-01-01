using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Agent")]
    public class AgentEntity:EntityBase
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long MediaId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [ForeignKey(nameof(MediaId))]
        public MediaEntity Media { get; set; }


    }
}
