using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ProducersConsumers
{
    public class Consumer<T>
    {
        private readonly ResourceManager<T> _managerWithQueue;
        private IList<T> ConsumedResources = new List<T>();

        public Consumer(ResourceManager<T> managerWithQueue)
        {
            _managerWithQueue = managerWithQueue;
        }

        public void Consume()
        {
            T par = default;
            if (!_managerWithQueue.TryGetResource(ref par))
                return;

            ConsumedResources.Add(par);
            Thread.Sleep(Constants.TIME_OUT_AFTER_DONE_JOB);
        }

        public int ConsumedResourcesCount()
        {
            return ConsumedResources.Count;
        }

        public T GetConsumedElementAt(int index)
        {
            return ConsumedResources[index];
        }
    }
}