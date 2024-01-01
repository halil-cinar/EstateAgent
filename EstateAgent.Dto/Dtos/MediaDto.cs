using EstateAgent.Dto.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class MediaDto:DtoBase
    {
        public IFormFile File { get; set; }

    }
}
