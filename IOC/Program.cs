using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using IOC.Interface;
using IOC.Impl;

namespace IOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            Container.Init(ass, ass, x => x.Namespace == "IOC.Interface", x => x.Namespace == "IOC.Impl");


            //ICode codeImpl = Container.Resolve("IOC.Interface.ICode") as ICode;
            Container.ExeMethod(typeof(WholeDay), "Start");


            Console.WriteLine();
        }
    }


    public class WholeDay
    {
        public void Start(IWakeUp fac1, ICode fac2, ISleep fac3)
        {
            WakeUp(fac1);
            Code(fac2);
            Sleep(fac3);
        }

        public void WakeUp(IWakeUp fac)
        {
            string result = $"{fac.Name()} 的起床时间是 {fac.Time()} ";
            Console.WriteLine(result);
        }

        public void Code(ICode fac)
        {
            string result = $"在 {fac.Time()}  {fac.DoSomething()}";
            Console.WriteLine(result);
        }

        public void Sleep(ISleep fac)
        {
            string result = $"睡觉是在 {fac.Time()}";
            Console.WriteLine(result);
        }
    }

}
