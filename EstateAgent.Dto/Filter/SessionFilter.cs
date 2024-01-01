using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Filter
{
    public class SessionFilter
    {
        public long? UserId { get; set; }
        public long? IdentityId { get; set; }
        public string? IpAddress { get; set; }
        public bool? IsActive { get; set; }
    }
}
