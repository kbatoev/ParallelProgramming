using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProducersConsumers
{
    public class Cluster<T>
    {
        private CancellationTokenSource _source;
        private List<Task<bool>> _tasks;
        public ResourceManager<T> ResourceManager { get; }
        public List<Producer<T>> Producers { get; }
        public List<Consumer<T>> Consumers { get; }

        public Cluster(int consumersNumber, int producersNumber, Func<int, T> createResource, Func<int, int> updateState)
        {
            ResourceManager = new ResourceManager<T>();
            Producers = new List<Producer<T>>(producersNumber);
            for (int i = 0; i < producersNumber; ++i)
            {
                Producer<T> p = new Producer<T>(ResourceManager, 0, createResource, updateState);
                Producers.Add(p);
            }

            Consumers = new List<Consumer<T>>(consumersNumber);
            for (int i = 0; i < consumersNumber; ++i)
            {
                Consumer<T> consumer = new Consumer<T>(ResourceManager);
                Consumers.Add(consumer);
            }
        }


        private Func<bool> CreateTask(object obj, CancellationToken token)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Unable to create task");
            }

            Func<bool> run = () =>
            {
                int startingId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Starting {0} on ThreadId = {1}", obj is Producer<T> ? "producer" : "consumer", startingId);

                while (true)
                {
                    if (obj is Producer<T>)
                    {
                        (obj as Producer<T>).Produce();
                    }
                    else if (obj is Consumer<T>)
                    {
                        (obj as Consumer<T>).Consume();
                    }
                    else
                    {
                        throw  new UnreachableException();
                    }

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }

                int finishingId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("{0} finished on ThreadId = {1}", obj is Producer<T> ? "Producer" : "Consumer", finishingId);

                return startingId == finishingId;
            };

            return run;
        }

        public void Start()
        {
            if (_source != null) return;
            _source = new CancellationTokenSource();
            _tasks = new List<Task<bool>>();

            CancellationToken token = _source.Token;
            Console.WriteLine($"token can be cancelled = {token.CanBeCanceled}");

            List<object> all = new List<object>(Consumers);
            all.AddRange(Producers);
            foreach (var obj in all)
            {
                Task<bool> task = Task.Factory.StartNew<bool>(CreateTask(obj, token), TaskCreationOptions.LongRunning);
                _tasks.Add(task);
            }
        }

        public bool Stop()
        {
            _source.Cancel();
            Console.WriteLine($"Sent cancellation request: {_source.Token.IsCancellationRequested}");
            Console.WriteLine("Waiting for tasks to finish");
            Task.WaitAll(_tasks.Select(t => (Task) t).ToArray());
            Console.WriteLine("All tasks finished");
            _source.Dispose();
            _source = null;
            return _tasks.TrueForAll(t => t.IsCompleted && t.Result);
        }
    }
}