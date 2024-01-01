using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class UserListDto : ListDtoBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long ProfilePhotoId { get; set; }
        public DateTime BirthDate { get; set; }







    }
}
