using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities.Enums
{
    public enum PropertySaleStatus
    {
        [Description("Kiralık")]
        Rent=1,
        [Description("Satılık")]
        Sale
    }
}
