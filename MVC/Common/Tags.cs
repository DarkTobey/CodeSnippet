using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : System.Attribute
    {
        public String ControllerName { get; set; }

        public ControllerAttribute(string controllerName)
        {
            ControllerName = controllerName;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : System.Attribute
    {
        public String ActionName { get; set; }

        public ActionAttribute(string actionName)
        {
            ActionName = actionName;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class RouteAttribute : System.Attribute
    {
        public String RouteName { get; set; }

        public RouteAttribute(string routeName)
        {
            RouteName = routeName;
        }
    }

}
