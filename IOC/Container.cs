﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace IOC
{
    public static class Container
    {
        private static Dictionary<string, ImplementInfo> dict = new Dictionary<string, ImplementInfo>();

        public static void Register(Assembly interFace, Assembly implement, Func<Type, bool> whereFac, Func<Type, bool> whereImpl)
        {
            var facTypes = interFace.GetTypes().Where(whereFac);
            var implTypes = implement.GetTypes().Where(whereImpl);

            foreach (var imp in implTypes)
            {
                var fac = imp.GetInterfaces();
                if (!fac.Any()) continue;
                foreach (var f in fac)
                {
                    if (dict.ContainsKey(f.FullName)) continue;
                    if (facTypes.Contains(f))
                    {
                        dict.Add(f.FullName, new ImplementInfo()
                        {
                            AssemblyName = implement.FullName,
                            TypeFullName = imp.FullName,
                        });
                        continue;
                    };
                }
            }
        }

        public static void Register(Type interFace, Type implement)
        {
            dict.Add(interFace.FullName, new ImplementInfo()
            {
                AssemblyName = implement.Assembly.FullName,
                TypeFullName = implement.FullName
            });
        }

        public static object Resolve(string key)
        {
            if (!dict.ContainsKey(key)) throw new Exception("该接口没有注册");
            var implementInfo = dict[key];
            return Assembly.Load(implementInfo.AssemblyName).CreateInstance(implementInfo.TypeFullName);
        }

        public static T Resolve<T>()
        {
            var type = typeof(T);
            if (!dict.ContainsKey(type.FullName)) throw new Exception("该接口没有注册");
            var implementInfo = dict[type.FullName];
            return (T)Assembly.Load(implementInfo.AssemblyName).CreateInstance(implementInfo.TypeFullName);
        }
    }


    public class ImplementInfo
    {
        public string AssemblyName { get; set; }
        public string TypeFullName { get; set; }
    }

}
