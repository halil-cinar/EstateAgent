using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class BlogFilter
    {
        public string? Title { get; set; }
      
        public long? UserId { get; set; }

        public Order? Order { get; set; }



    }

    public class Order
    {
        public bool? PostedDateDirection { get; set; }
    }
}
