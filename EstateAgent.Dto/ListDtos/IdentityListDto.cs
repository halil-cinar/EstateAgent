using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class IdentityListDto : ListDtoBase
    {
        public long UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string UserName { get; set; }

        public bool IsValid { get; set; }
        public DateTime? ExpiryDate { get; set; }




    }
}
