using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Session")]
    public class SessionEntity:EntityBase
    {
        public long UserId { get; set; }
        public long IdentityId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [ForeignKey("IdentityId")]
        public IdentityEntity Identity { get; set; }


    }
}
