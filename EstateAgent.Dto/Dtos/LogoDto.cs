﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class LogoDto:SystemSettingsDto
    {
        public IFormFile File { get; set; }
    }
}
