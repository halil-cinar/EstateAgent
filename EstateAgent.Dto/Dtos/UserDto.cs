using EstateAgent.Dto.Abstract;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class UserDto:DtoBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public RoleTypes Role { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public IFormFile? ProfilePhoto { get; set; }




    }
}
