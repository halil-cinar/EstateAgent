using EstateAgent.Dto.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class BlogDto:DtoBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
        public DateTime PostedDate { get; set; }

        public IFormFile File { get; set; }
        public long UserId { get; set; }

        public BlogDto()
        {
            PostedDate= DateTime.Now;
        }


    }
}
