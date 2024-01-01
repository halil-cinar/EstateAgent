using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("IdentityEnt")]
    public class IdentityEntity:EntityBase
    {
        public long UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string UserName { get; set; }

        public bool IsValid { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

    }
}
