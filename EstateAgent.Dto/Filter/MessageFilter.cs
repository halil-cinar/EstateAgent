using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Filter
{
    public class MessageFilter
    {
        
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public long? SendUserId { get; set; }
        public string? ChatKey { get; set; }
    }
}
