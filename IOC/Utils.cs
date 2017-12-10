using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOC
{
    public class Utils
    {
        public static void ExeMethod(Type type, string methodName)
        {
            object obj = type.Assembly.CreateInstance(type.FullName);
            MethodInfo method = type.GetMethod(methodName);
            if (method == null) throw new Exception($"未能找到{methodName}方法");
            ParameterInfo[] paramInfo = method.GetParameters();
            List<object> paramList = new List<object>();
            foreach (var p in paramInfo)
            {
                Type pt = p.ParameterType;
                object po = Container.Resolve(pt.FullName);
                paramList.Add(po);
            }
            method.Invoke(obj, paramList.ToArray());
        }
    }
}
