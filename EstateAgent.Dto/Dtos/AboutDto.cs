using EstateAgent.Dto.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class AboutDto:DtoBase
    {
        public string Title { get; set; }
        public string Content { get; set; }



    }
}
