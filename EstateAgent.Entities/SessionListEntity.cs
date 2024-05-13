using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("SessionListView")]
    public class SessionListEntity:EntityBase
    {
        public long UserId { get; set; }
        public long IdentityId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public long? UserProfilePhotoId { get; set; }

        public string IdentityUserName { get; set; }

      
    }
}
