using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MVC.Common
{
    public class Dispatcher
    {
        private static Dispatcher _Dispatcher = null;

        private static Dictionary<string, RouteInfo> _Dict = new Dictionary<string, RouteInfo>();

        public static Dispatcher Instance(string assemblyName = null, Func<System.Type, bool> whereType = null)
        {
            if (_Dispatcher == null)
            {
                _Dispatcher = new Dispatcher();
                _Dispatcher.Init(assemblyName, whereType);
            }
            return _Dispatcher;
        }

        private void Init(string assemblyName, Func<System.Type, bool> whereType)
        {
            if (assemblyName == null)
                assemblyName = Assembly.GetExecutingAssembly().FullName;

            if (whereType == null)
                whereType = x => true;

            RegisterRoute(assemblyName, whereType);
        }

        private void RegisterRoute(string assemblyName, Func<System.Type, bool> whereType)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(whereType).ToArray();
            foreach (var type in types)
            {
                ControllerAttribute controller = type.GetCustomAttribute<ControllerAttribute>();
                if (controller == null) continue;

                foreach (var methods in type.GetMethods())
                {
                    ActionAttribute action = methods.GetCustomAttribute<ActionAttribute>();
                    if (action == null) continue;

                    RouteInfo info = new RouteInfo()
                    {
                        AssemblyName = assemblyName,
                        TypeName = type.FullName,
                        MethodName = methods.Name,
                        ControllerName = controller.ControllerName,
                        ActionName = action.ActionName,
                    };

                    if (_Dict.ContainsKey(info.Path)) throw new Exception("已经添加过相同的路由," + Newtonsoft.Json.JsonConvert.SerializeObject(info));
                    _Dict.Add(info.Path, info);
                }
            }
        }

        public IResult ResolveRoute(string path)
        {
            path = path.ToLower();
            if (_Dict.ContainsKey(path))
            {
                RouteInfo info = _Dict[path];

                Type type = Type.GetType(info.TypeName);
                MethodInfo method = type.GetMethod(info.MethodName);
                object obj = Assembly.Load(info.AssemblyName).CreateInstance(info.TypeName);

                List<object> paramList = new List<object>();
                //TODO Action 中参数绑定
                //ParameterInfo[] paramInfo = method.GetParameters();
                //foreach (var p in paramInfo)
                //{
                //    Type pt = p.ParameterType;
                //    object po = Container.Resolve(pt.FullName);
                //    paramList.Add(po);
                //}

                return method.Invoke(obj, paramList.ToArray()) as IResult;
            }
            else
            {
                return new StringResult("该页面没有找到");
            }
        }
    }

    public class RouteInfo
    {
        public string AssemblyName { get; set; }

        public string TypeName { get; set; }

        public string MethodName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Path { get { return (ControllerName + "/" + ActionName).ToLower(); } }
    }
}
