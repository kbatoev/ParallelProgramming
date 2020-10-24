using System;
using System.Threading;

namespace ProducersConsumers
{
    public class Producer<T>
    {
        private readonly  ResourceManager<T> _managerWithQueue;
        private int _state;
        private readonly Func<int, T> _createResource;
        private readonly Func<int, int> _updateState;

        public Producer(ResourceManager<T> manager, int initialState, Func<int, T> createResource, Func<int, int> updateState)
        {
            _managerWithQueue = manager;
            _state = initialState;
            _createResource = createResource;
            _updateState = updateState;
        }

        public void Produce()
        {
            T resource = _createResource(_state);
            _state = _updateState(_state);
            _managerWithQueue.PutResource(resource);
            Thread.Sleep(Constants.TIME_OUT_AFTER_DONE_JOB);
        }
    }
}