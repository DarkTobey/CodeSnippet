﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using IOC.Interface;
using IOC.ImplA;

namespace IOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            Container.Init(ass, ass, x => x.Namespace == "IOC.Interface", x => x.Namespace == "IOC.ImplB");


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
            Console.WriteLine($"{fac.Time()}   {fac.Name()}   起床");
        }

        public void Code(ICode fac)
        {
            Console.WriteLine($"{fac.Time()}   {fac.DoSomething()}");
        }

        public void Sleep(ISleep fac)
        {
            Console.WriteLine($"{fac.Time()}   睡觉");
        }
    }

}
