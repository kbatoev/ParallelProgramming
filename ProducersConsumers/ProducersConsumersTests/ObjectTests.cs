using NUnit.Framework;

namespace ProducersConsumersTests
{
    public class ObjectTests
    {
        public class Resource
        {
            private static object idLock = new object();
            private static int id;

            private int _id { get; }

            public Resource()
            {
                lock (idLock)
                {
                    _id = id++;
                }
            }

            public override string ToString()
            {
                return "Resource id = " + _id;
            }
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(0, 10)]
        [TestCase(0, 20)]
        [TestCase(0, 25)]
        [TestCase(1, 20)]
        [TestCase(1, 15)]
        [TestCase(20, 0)]
        [TestCase(25, 0)]
        [TestCase(10, 3)]

        public void ResourceTest(int consumersNumber, int producersNumber)
        {
            TestSetup.CheckThatResourceIsNotConsumedTwice(consumersNumber, producersNumber, i => new Resource());

        }
    }
}