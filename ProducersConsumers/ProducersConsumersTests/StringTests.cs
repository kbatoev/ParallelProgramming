using System.Threading;
using NUnit.Framework;

namespace ProducersConsumersTests
{
    public class StringTests
    {

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
        public void StringTest(int consumersNumber, int producersNumber)
        {
            TestSetup.CheckThatResourceIsNotConsumedTwice<string>(consumersNumber, producersNumber,
                i => Thread.CurrentThread.ManagedThreadId + ": "+ i);
        }
    }
}