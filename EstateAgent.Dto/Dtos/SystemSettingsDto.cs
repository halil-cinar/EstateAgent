﻿using EstateAgent.Dto.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Dtos
{
    public class SystemSettingsDto:DtoBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
