using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class MessageListDto:ListDtoBase
    {
        public string FullName { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime SendTime { get; set; }
        public long? SendUserId { get; set; }
        public string ChatKey { get; set; }
    }
}
