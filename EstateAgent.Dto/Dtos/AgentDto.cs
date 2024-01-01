using EstateAgent.Dto.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class AgentDto:DtoBase
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile File { get; set; }




    }
}
