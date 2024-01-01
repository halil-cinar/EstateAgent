using EstateAgent.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities
{
    [Table("SystemSettings")]
    public class SystemSettingsEntity:EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
