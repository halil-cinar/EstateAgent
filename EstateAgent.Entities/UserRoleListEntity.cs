using EstateAgent.Entities.Abstract;
using EstateAgent.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("UserRoleListView")]
    public class UserRoleListEntity:EntityBase
    {
        public long UserId { get; set; }
        public RoleTypes Role { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public long? UserProfilePhotoId { get; set; }
    }
}
