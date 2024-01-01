using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class IdentityDto : DtoBase
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        
        public string UserName { get; set; }

        public bool IsValid { get; set; }
        public DateTime? ExpiryDate { get; set; }




    }
}
