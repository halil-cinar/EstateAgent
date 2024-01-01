using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities.Enums
{
    public enum RoleTypes
    {
        [Description("Admin")]
        Admin,
        [Description("User")]
        User,
        [Description("Agent")]
        Agent,
        [Description("Guest")]
        Guest
    }
}
