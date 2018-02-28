using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC.Common;

namespace MVC.Controller
{
    [Controller("Home")]
    public class HomeController
    {
        [Action("Login")]
        public IResult Login()
        {
            return new ViewResult("View/Login.cst");
        }

        [Action("Index")]
        public IResult Index()
        {
            return new StringResult("这里Index页面");
        }
    }
}
