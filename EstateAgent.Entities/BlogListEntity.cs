using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("BlogListView")]
    public class BlogListEntity:EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
        public DateTime PostedDate { get; set; }

        public long MediaId { get; set; }
        public long UserId { get; set; }

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public long? UserProfilePhotoId { get; set; }
    }
}
