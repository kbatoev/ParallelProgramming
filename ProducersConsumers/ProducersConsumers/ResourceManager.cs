using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducersConsumers
{
    public class ResourceManager<T>
    {
        private readonly object _readLock = new object();
        private readonly object _writeLock = new object();
        private int _readIndex;
        private int _writeIndex;
        private readonly List<T> _resources = new List<T>();

        public bool TryGetResource(ref T resourceRef)
        {
            bool result = true;

            Monitor.Enter(_readLock);
            if (_readIndex > _writeIndex)
            {
                throw new UnreachableException();
            }

            if (_readIndex == _writeIndex)
            {
                result = false;
            }
            else
            {
                resourceRef = _resources[_readIndex];
                _readIndex++;
                Console.WriteLine($"Got resource: {resourceRef}");
            }
            Monitor.Exit(_readLock);

            return result;
        }

        public void PutResource(T resource)
        {
            Monitor.Enter(_writeLock);
            _resources.Add(resource);
            _writeIndex++;
            Console.WriteLine($"Put resource: {resource}");
            Monitor.Exit(_writeLock);
        }
    }
}