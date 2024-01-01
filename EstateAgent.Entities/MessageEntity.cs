using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("Message")]
    public class MessageEntity:EntityBase
    {
        public string FullName { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime SendTime { get; set; }
        public long? SendUserId { get; set; }
        public long? AnsweredMessageId { get; set; }
        public string ChatKey { get; set; }

        [ForeignKey(nameof(SendUserId))]
        public UserEntity User { get; set; }

        [ForeignKey(nameof(AnsweredMessageId))]
        public MessageEntity AnsweredMessage { get; set; }

    }
}
