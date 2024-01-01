using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("User")]
    public class UserEntity:EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long? ProfilePhotoId { get; set; }

        [ForeignKey(nameof(ProfilePhotoId))]
        public MediaEntity ProfilePhoto { get; set; }




    }
}
