using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("ContactInfo")]
    public class ContactInfoEntity:EntityBase
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string? XUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? LinkedinUrl { get; set; }

        public string LocationLongitude { get; set; }
        public string LocationLatitude { get; set; }


    }
}
