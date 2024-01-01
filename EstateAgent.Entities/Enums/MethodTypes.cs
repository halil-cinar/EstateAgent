using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Entities.Enums
{
    public enum MethodTypes
    {
        [Description("none")]
        None = 0,

        [Description("method used for adding a about")]
        AboutAdd,
        [Description("method used for about deletion")]
        AboutRemove,
        [Description("method used to update the about")]
        AboutUpdate,
        [Description("method used for about listing")]
        AboutGetAll,
        [Description("method used for about listing")]
        AboutLoadMoreFilter,
        [Description("method used for about listing")]
        AboutGetOptionList,
        [Description("method used for about get")]
        AboutGet,

        [Description("method used for adding a aboutmedia")]
        AboutMediaAdd,
        [Description("method used for aboutmedia deletion")]
        AboutMediaRemove,
        [Description("method used to update the aboutmedia")]
        AboutMediaUpdate,
        [Description("method used for aboutmedia listing")]
        AboutMediaGetAll,
        [Description("method used for aboutmedia listing")]
        AboutMediaLoadMoreFilter,
        [Description("method used for aboutmedia listing")]
        AboutMediaGetOptionList,
        [Description("method used for aboutmedia get")]
        AboutMediaGet,

        [Description("method used for adding a account")]
        AccountAdd,
        [Description("method used for account deletion")]
        AccountRemove,
        [Description("method used to update the account")]
        AccountUpdate,
        [Description("method used for account listing")]
        AccountGetAll,
        [Description("method used for account listing")]
        AccountLoadMoreFilter,
        [Description("method used for account listing")]
        AccountGetOptionList,
        [Description("method used for account get")]
        AccountGet,

        [Description("method used for adding a agent")]
        AgentAdd,
        [Description("method used for agent deletion")]
        AgentRemove,
        [Description("method used to update the agent")]
        AgentUpdate,
        [Description("method used for agent listing")]
        AgentGetAll,
        [Description("method used for agent listing")]
        AgentLoadMoreFilter,
        [Description("method used for agent listing")]
        AgentGetOptionList,
        [Description("method used for agent get")]
        AgentGet,

        [Description("method used for adding a blog")]
        BlogAdd,
        [Description("method used for blog deletion")]
        BlogRemove,
        [Description("method used to update the blog")]
        BlogUpdate,
        [Description("method used for blog listing")]
        BlogGetAll,
        [Description("method used for blog listing")]
        BlogLoadMoreFilter,
        [Description("method used for blog listing")]
        BlogGetOptionList,
        [Description("method used for blog get")]
        BlogGet,

        [Description("method used for adding a contactinfo")]
        ContactInfoAdd,
        [Description("method used for contactinfo deletion")]
        ContactInfoRemove,
        [Description("method used to update the contactinfo")]
        ContactInfoUpdate,
        [Description("method used for contactinfo listing")]
        ContactInfoGetAll,
        [Description("method used for contactinfo listing")]
        ContactInfoLoadMoreFilter,
        [Description("method used for contactinfo listing")]
        ContactInfoGetOptionList,
        [Description("method used for contactinfo get")]
        ContactInfoGet,

        [Description("method used for adding a identity")]
        IdentityAdd,
        [Description("method used for identity deletion")]
        IdentityRemove,
        [Description("method used to update the identity")]
        IdentityUpdate,
        [Description("method used for identity listing")]
        IdentityGetAll,
        [Description("method used for identity listing")]
        IdentityLoadMoreFilter,
        [Description("method used for identity listing")]
        IdentityGetOptionList,
        [Description("method used for identity get")]
        IdentityGet,

        [Description("method used for adding a media")]
        MediaAdd,
        [Description("method used for media deletion")]
        MediaRemove,
        [Description("method used to update the media")]
        MediaUpdate,
        [Description("method used for media listing")]
        MediaGetAll,
        [Description("method used for media listing")]
        MediaLoadMoreFilter,
        [Description("method used for media listing")]
        MediaGetOptionList,
        [Description("method used for media get")]
        MediaGet,

        [Description("method used for adding a property")]
        PropertyAdd,
        [Description("method used for property deletion")]
        PropertyRemove,
        [Description("method used to update the property")]
        PropertyUpdate,
        [Description("method used for property listing")]
        PropertyGetAll,
        [Description("method used for property listing")]
        PropertyLoadMoreFilter,
        [Description("method used for property listing")]
        PropertyGetOptionList,
        [Description("method used for property get")]
        PropertyGet,

        [Description("method used for adding a propertymedia")]
        PropertyMediaAdd,
        [Description("method used for propertymedia deletion")]
        PropertyMediaRemove,
        [Description("method used to update the propertymedia")]
        PropertyMediaUpdate,
        [Description("method used for propertymedia listing")]
        PropertyMediaGetAll,
        [Description("method used for propertymedia listing")]
        PropertyMediaLoadMoreFilter,
        [Description("method used for propertymedia listing")]
        PropertyMediaGetOptionList,
        [Description("method used for propertymedia get")]
        PropertyMediaGet,

        [Description("method used for adding a rolemethod")]
        RoleMethodAdd,
        [Description("method used for rolemethod deletion")]
        RoleMethodRemove,
        [Description("method used to update the rolemethod")]
        RoleMethodUpdate,
        [Description("method used for rolemethod listing")]
        RoleMethodGetAll,
        [Description("method used for rolemethod listing")]
        RoleMethodLoadMoreFilter,
        [Description("method used for rolemethod listing")]
        RoleMethodGetOptionList,
        [Description("method used for rolemethod get")]
        RoleMethodGet,

        [Description("method used for adding a session")]
        SessionAdd,
        [Description("method used for session deletion")]
        SessionRemove,
        [Description("method used to update the session")]
        SessionUpdate,
        [Description("method used for session listing")]
        SessionGetAll,
        [Description("method used for session listing")]
        SessionLoadMoreFilter,
        [Description("method used for session listing")]
        SessionGetOptionList,
        [Description("method used for session get")]
        SessionGet,

        [Description("method used for adding a subscribe")]
        SubscribeAdd,
        [Description("method used for subscribe deletion")]
        SubscribeRemove,
        [Description("method used to update the subscribe")]
        SubscribeUpdate,
        [Description("method used for subscribe listing")]
        SubscribeGetAll,
        [Description("method used for subscribe listing")]
        SubscribeLoadMoreFilter,
        [Description("method used for subscribe listing")]
        SubscribeGetOptionList,
        [Description("method used for subscribe get")]
        SubscribeGet,

        [Description("method used for adding a systemsettings")]
        SystemSettingsAdd,
        [Description("method used for systemsettings deletion")]
        SystemSettingsRemove,
        [Description("method used to update the systemsettings")]
        SystemSettingsUpdate,
        [Description("method used for systemsettings listing")]
        SystemSettingsGetAll,
        [Description("method used for systemsettings listing")]
        SystemSettingsLoadMoreFilter,
        [Description("method used for systemsettings listing")]
        SystemSettingsGetOptionList,
        [Description("method used for systemsettings get")]
        SystemSettingsGet,

        [Description("method used for adding a user")]
        UserAdd,
        [Description("method used for user deletion")]
        UserRemove,
        [Description("method used to update the user")]
        UserUpdate,
        [Description("method used for user listing")]
        UserGetAll,
        [Description("method used for user listing")]
        UserLoadMoreFilter,
        [Description("method used for user listing")]
        UserGetOptionList,
        [Description("method used for user get")]
        UserGet,

        [Description("method used for adding a userrole")]
        UserRoleAdd,
        [Description("method used for userrole deletion")]
        UserRoleRemove,
        [Description("method used to update the userrole")]
        UserRoleUpdate,
        [Description("method used for userrole listing")]
        UserRoleGetAll,
        [Description("method used for userrole listing")]
        UserRoleLoadMoreFilter,
        [Description("method used for userrole listing")]
        UserRoleGetOptionList,
        [Description("method used for userrole get")]
        UserRoleGet,
        UserResetPassword,
    }
}
