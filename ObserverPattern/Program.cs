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
            var subject = Subject.GetInstance();
            var a = new ObserverA(subject);
            var b = new ObserverB(subject);
            subject.State = 1;
            subject.State = 6;


            MessageBus.Subscribe("a", new Callback("XD"));
            MessageBus.Subscribe("a", new Callback("Tobey"));
            MessageBus.Subscribe("b", new Callback("Ani"));
            MessageBus.Publish("a", obs =>
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

        public static void Subscribe(string topic, object obj)
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

        public static void UnSubscribe(object obj)
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

        public static void Publish(string topic, Action<object> act)
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
        public string Name { get; set; }

        public Callback(string name)
        {
            Name = name;
        }

        public void Do()
        {
            Console.WriteLine($"my name is {Name}");
        }
    }
}
