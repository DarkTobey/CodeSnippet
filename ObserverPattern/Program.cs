using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    class Program
    {
        static void Main(string[] args)
        {

            new Reactor().Run();

            var subject = Subject.GetInstance();
            var a = new ObserverA(subject);
            var b = new ObserverB(subject);
            subject.State = 1;
            subject.State = 6;

            var dispatcher = MessageBus.GetInstance();

            dispatcher.Subscribe("a", new Callback("XD"));
            dispatcher.Subscribe("a", new Callback("Tobey"));
            dispatcher.Subscribe("b", new Callback("Ani"));
            dispatcher.Publish("a", obs =>
            {
                (obs as Callback).Do();
            });

            Console.ReadLine();
        }
    }

    #region 观察者模式(一般意义上的)

    public class Subject
    {
        private int state;

        private static List<Observer> observers = new List<Observer>();

        private static Subject instance = new Subject();

        private Subject()
        {
        }

        public static Subject GetInstance()
        {
            return instance;
        }


        /// <summary>
        /// 当然,我这里模拟mvvm的get set实现,stata的值改变之后，通知所有的观察者更新View
        /// </summary>
        public int State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                NotifyAllObservers();
            }
        }

        public void AddObserver(Observer observer)
        {
            observers.Add(observer);
        }

        public void NotifyAllObservers()
        {
            foreach (Observer observer in observers)
            {
                observer.Update();
            }
        }
    }

    public abstract class Observer
    {
        public Subject Subject;
        public abstract void Update();
    }

    public class ObserverA : Observer
    {
        public ObserverA(Subject subject)
        {
            this.Subject = subject;
            this.Subject.AddObserver(this);
        }

        public override void Update()
        {
            Console.WriteLine("A is ready to update : " + Subject.State);
        }
    }

    public class ObserverB : Observer
    {
        public ObserverB(Subject subject)
        {
            this.Subject = subject;
            this.Subject.AddObserver(this);
        }

        public override void Update()
        {
            Console.WriteLine("B is ready to update : " + Subject.State);
        }
    }

    #endregion

    #region 订阅发布模式（观察者模式另外一种实现方式，两种方式适用场景不同，根据实际情况选择）

    public class MessageBus
    {
        private static Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();

        private static object msgLock = new object();

        private static MessageBus instance = new MessageBus();

        private MessageBus()
        {
        }

        public static MessageBus GetInstance()
        {
            return instance;
        }

        public void Subscribe(string topic, object obj)
        {
            if (string.IsNullOrEmpty(topic)) return;
            lock (msgLock)
            {
                if (dic == null)
                    dic = new Dictionary<string, List<object>>();

                if (!dic.ContainsKey(topic))
                    dic[topic] = new List<object>();

                dic[topic].Add(obj);
            }
        }

        public void UnSubscribe(object obj)
        {
            lock (msgLock)
            {
                if (dic == null) return;
                List<string> removeTopic = new List<string>();
                foreach (var item in dic)
                {
                    item.Value.Remove(obj);
                    if (item.Value.Count == 0)
                        removeTopic.Add(item.Key);
                }
                foreach (var topic in removeTopic)
                {
                    dic.Remove(topic);
                }
            }
        }

        public void Publish(string topic, Action<object> act)
        {
            if (string.IsNullOrEmpty(topic)) return;
            lock (msgLock)
            {
                if (dic == null) return;
                if (!dic.ContainsKey(topic)) return;
                var list = dic[topic];
                foreach (var i in list)
                {
                    act(i);
                }
            }
        }
    }

    #endregion

    public class Callback
    {
        public string Message { get; set; }

        public Callback(string message)
        {
            Message = message;
        }

        public void Do()
        {
            Console.WriteLine($"回调的执行信息: {Message}");
        }
    }

    /// <summary>
    /// 基于Reactor思想实现一个简单的事件驱动模型，模拟 浏览器的运行情况
    /// 
    /// Reactor模式是处理并发I/O比较常见的一种模式，用于同步I/O，中心思想是将所有要处理的I/O事件注册到一个中心I/O多路复用器上，同时主线程/进程阻塞在多路复用器上；
    /// 一旦有I/O事件到来或是准备就绪(文件描述符或socket可读、写)，多路复用器返回并将事先注册的相应I/O事件分发到对应的处理器中。
    /// </summary>
    public class Reactor
    {
        //IO多路复用--epoll详解
        //http://www.cnblogs.com/harvyxu/p/7487588.html


        // 事件驱动模式--Reactor
        // http://www.cnblogs.com/harvyxu/p/7498763.html


        //一个浏览器页面至少应该有几个线程(至少有三个 常驻线程:javascript引擎线程,界面渲染线程,浏览器事件触发线程,除些以外,也有一些执行完就终止的线程,如Http请求线程)
        //http://blog.csdn.net/kfanning/article/details/5768776


        //单线程的js是如何处理异步操作的(这其实是病句，一个线程执行js代码，I/O操作可以再其他线程完成，完成之后通知js线程就行了)
        //https://www.cnblogs.com/woodyblog/p/6061671.html

        //用于事件驱动的分发器
        private MessageBus Dispatcher = MessageBus.GetInstance();

        //执行队列
        private Queue<Callback> TaskQueue = new Queue<Callback>();

        public void Run()
        {
            //其他的一些线程

            //UI渲染线程
            Task.Run(() =>
            {
                //渲染UI
            });

            //js执行线程
            Task.Run(() =>
            {
                //你的js代码会订阅很多事件,并注册对应的回调函数
                Dispatcher.Subscribe("btn1_click", new Callback("btn1注册的第一个回调"));
                Dispatcher.Subscribe("btn1_click", new Callback("btn2注册的第二个回调"));

                Dispatcher.Subscribe("btn2_click", new Callback("btn2"));
                Dispatcher.Subscribe("btn3_click", new Callback("btn3"));

                //模拟产生了一些任务并放到队列中，setTimeout，setInterval，ajax请求等
                //当注册ajax任务时，如果设置ajax为同步执行，将js线程await,ajax完成后在执行队列线程中notify即可
                //setTimeout 新起一个线程，睡眠n秒，之后向队列中推一个任务
                //setInterval 新起一个线程，在循环中{睡眠n秒，向队列中推一个任务},
                while (true)
                {
                    lock (TaskQueue)
                    {
                        if (TaskQueue.Count < 1000)
                        {
                            TaskQueue.Enqueue(new Callback("第" + TaskQueue.Count + "个元素"));
                        }
                    }
                }
            });

            //执行任务队列的线程
            Task.Run(() =>
            {
                while (true)
                {
                    lock (TaskQueue)
                    {
                        if (TaskQueue.Count > 0)
                        {
                            var item = TaskQueue.Dequeue();
                            item.Do();
                        }
                    }
                }
            });

            //主线程用来处理事件收发（他会向操作系统订阅用户的各种IO操作，同时自己也接受别人的订阅， 当他收到操作系统的通知时，他再将通知下发给订阅他的人）
            //主线程应该阻塞在分发器上，随时等待着系统通知（这里就手动输入代替系统通知）
            while (true)
            {
                string input = Console.ReadLine();
                Dispatcher.Publish(input, x =>
                {
                    (x as Callback).Do();
                });
            }

        }
    }


}
