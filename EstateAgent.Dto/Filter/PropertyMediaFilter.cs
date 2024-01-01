using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class PropertyMediaFilter
    {
        public long? PropertyId { get; set; }
        public int? SlideIndex { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

       
      

        
      
    }
}
