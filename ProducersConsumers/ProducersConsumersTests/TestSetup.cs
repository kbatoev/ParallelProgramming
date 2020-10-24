using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using ProducersConsumers;

namespace ProducersConsumersTests
{
    public class TestSetup
    {
        private static int Seconds2MilliSeconds(int seconds)
        {
            return seconds * 1000;
        }
        public static  void CheckThatResourceIsNotConsumedTwice<T>(int consumersNumber, int producersNumber, Func<int, T> createResource)
        {
            Cluster<T> cluster = new Cluster<T>(consumersNumber, producersNumber, createResource, i => i + 1 );

            cluster.Start();

            Thread.Sleep(Seconds2MilliSeconds(5));

            bool allTasksFinished = cluster.Stop();
            Assert.IsTrue(allTasksFinished);

            HashSet<T> uniqueConsumedValues = new HashSet<T>();
            foreach (var c in cluster.Consumers)
            {
                for (int i = 0; i < c.ConsumedResourcesCount(); ++i)
                {
                    T v = c.GetConsumedElementAt(i);
                    Assert.IsFalse(uniqueConsumedValues.Contains(v), $"checking {v} is not in set");
                    uniqueConsumedValues.Add(v);
                }
            }

            if (producersNumber == 0)
            {
                Assert.IsTrue(uniqueConsumedValues.Count == 0);
            }
        }
    }
}