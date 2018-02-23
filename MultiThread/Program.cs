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
            //ThreadLockDemo_GO();

            {
                object locker = new object();
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;

                Thread t1 = new Thread(() =>
                {
                    lock (locker)
                    {
                        Thread.Sleep(1 * 1000);
                        flag1 = true;
                        Monitor.PulseAll(locker);
                    }


                });
                t1.Name = "t1";
                t1.Start();

                Thread t2 = new Thread(() =>
                {
                    lock (locker)
                    {
                        Thread.Sleep(2 * 1000);
                        flag2 = true;
                        Monitor.PulseAll(locker);
                    }

                });
                t2.Name = "t2";
                t2.Start();

                Thread t3 = new Thread(() =>
                {
                    lock (locker)
                    {
                        Thread.Sleep(3 * 1000);
                        flag3 = true;
                        Monitor.PulseAll(locker);
                    }

                });
                t3.Name = "t2";
                t3.Start();



                while (true)
                {
                    lock (locker)
                    {
                        bool done = flag1 && flag2 && flag3;
                        if (!done)
                        {
                            Monitor.Wait(locker);
                        }
                        else
                        {
                            Console.WriteLine("完成");
                            Console.ReadLine();
                            break;
                        }
                    }
                }
            }

            return;

            PCModel_DO();
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

        static void PCModel_DO()
        {
            PCModel pc = new PCModel();

            Thread t1 = new Thread(pc.Product);
            t1.Name = "p";
            t1.Start();

            Thread t2 = new Thread(pc.Consumer);
            t2.Name = "c1";
            t2.Start();

            Thread t3 = new Thread(pc.Consumer);
            t3.Name = "c2";
            t3.Start();

            Console.ReadLine();
        }
    }

    /// <summary>
    /// 线程死锁
    /// </summary>
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

    /// <summary>
    /// 生产者消费者
    /// </summary>
    public class PCModel
    {
        private static int MAX = 10;

        private readonly object locker = new object();

        private Queue<object> queue = new Queue<object>();

        public void Product()
        {
            while (true)
            {
                lock (locker)
                {
                    while (queue.Count >= MAX)
                    {
                        Console.WriteLine($"库存最大,库存量为：{queue.Count}");
                        Monitor.Wait(locker);
                        Console.WriteLine($"继续执行,库存量为：{queue.Count}");
                    }
                    queue.Enqueue(new object());
                    Console.WriteLine($"生成产品,库存量为：{queue.Count}");
                    //Thread.Sleep(200);
                    Monitor.PulseAll(locker);
                }
            }
        }

        public void Consumer()
        {
            while (true)
            {
                lock (locker)
                {
                    while (queue.Count <= 0)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name},库存不足,库存量为：{queue.Count}");
                        Monitor.Wait(locker);
                        Console.WriteLine($"{Thread.CurrentThread.Name},继续执行,库存量为：{queue.Count}");
                    }
                    queue.Dequeue();
                    Console.WriteLine($"{Thread.CurrentThread.Name},消费产品,库存量为：{queue.Count}");
                    //Thread.Sleep(200);
                    Monitor.PulseAll(locker);
                }
            }
        }

    }

}
