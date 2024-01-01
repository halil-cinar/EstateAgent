using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class IdentityFilter
    {
        public long? UserId { get; set; }
       
        public string? UserName { get; set; }

        public bool? IsValid { get; set; }
        

        


    }
}
