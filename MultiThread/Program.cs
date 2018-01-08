using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThread
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadLockDemo_GO();


        }

        static void ThreadLockDemo_GO()
        {
            ThreadLockDemo c = new ThreadLockDemo();

            Thread t1 = new Thread(c.M1);
            t1.Start();

            Thread t2 = new Thread(c.M2);
            t2.Start();

            //注意，这里还不能让 前台线程 结束，因为两个线程可能还没有全部开始
            Thread.Sleep(1000);
        }
    }

    public class ThreadLockDemo
    {
        object lock1 = new object();

        object lock2 = new object();

        public void M1()
        {
            //（0） M1 正常执行，占用lock1
            lock (lock1)
            {
                Console.WriteLine("M1 Do");

                //Thread.Sleep(100);
                //（1） 如果执行到这里时 ,t1失去执行权, t2获得执行权,开始执行M2 , 则会进入（2）
                //（4） 重新获得执行权，继续执行，发现lock2被占用，无法继续 ， 等待M2释放lock2 。。。  // 这时死锁就发生了，（3）（4）都在等待彼此释放资源，

                lock (lock2)
                {
                    Console.WriteLine("M1 Do Anthor");
                }
            }
        }

        public void M2()
        {
            //（2） M2获得执行权 占用lock2 继续执行 ， 进入（3）
            lock (lock2)
            {
                Console.WriteLine("M2 Do");

                // （3）继续执行时,发现lock1被占用，无法继续， 等待M1释放lock1 。。。   然后执行权重新交给t1,继续执行M1 ， 进入（4）
                lock (lock1)
                {
                    Console.WriteLine("M2 Do Anthor");
                }
            }
        }

    }

}
