using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.ListDtos
{
    public class MediaListDto:ListDtoBase
    {
        public string MediaName { get; set; }
        public string MediaType { get; set; }

        public byte[] Media { get; set; }

       


    }
}
