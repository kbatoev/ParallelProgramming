using System;
using System.Threading;
using ProducersConsumers;

namespace RunProducersAndConsumers
{
    class Program
    {
        static void Main(string[] args)
        {
            const int consumers = 5;
            const int producers = 3;
            Console.WriteLine($"number of consumers: {consumers}");
            Console.WriteLine($"number of producers: {producers}");
            var cluster = new Cluster<int>(consumers, producers, i => i, i => i + 1);
            int sleep = 5;
            Console.WriteLine($"Cluster will start in {sleep} seconds. Press any key to stop.");
            Thread.Sleep(sleep * 1000);
            cluster.Start();

            Console.ReadKey();
            bool result = cluster.Stop();
            if (result)
            {
                Console.WriteLine("Cluster succeccfully stopped");
            }
            else
            {
                Console.WriteLine("Cluster was unable to stop");
            }
        }
    }
}