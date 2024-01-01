using EstateAgent.Entities.Enums;

namespace EstateAgent.WebApp.Models
{
    public class RouteMethod
    {
        public Dictionary<string,MethodTypes> RouteMethodTables { get; set; }

        public RouteMethod()
        {
            RouteMethodTables= new Dictionary<string,MethodTypes>();
            

        }

    }
}
