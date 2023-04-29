using System;
using System.Collections.Generic;
using UnityEngine; 

namespace CoolFramework.Core
{
    /// <summary>
    /// Poolable Object Instancied and store in the pools.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary> Called when the Poolable Object is created.</summary>
        void OnCreated();
        /// <summary> Called when the Poolable Object is sent to pool.</summary>
        void OnSentToPool();
        /// <summary> Called when the Poolable Object is removed from pool.</summary>
        void OnRemovedFromPool();
    }

    public interface IPoolInstanceManager<T> where T : IPoolableObject
    {
        /// <summary> Create and return an Instance of the IPoolableObject. </summary>
        T CreateInstance();

        /// <summary>Destroy an Instance of the IPoolableObject. </summary>
        void DestroyInstance(T _instance); 
    }

    [Serializable]
    public sealed class ObjectPool<T> where T : IPoolableObject
    {
        #region Fields and Properties
        [SerializeField] private List<T> pool = new List<T>();
        private IPoolInstanceManager<T> poolManager; 
        #endregion

        #region Constructor
        public ObjectPool(int _initialCapacity = 1)
        {
            pool = new List<T>(_initialCapacity);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the pool and the manager.
        /// </summary>
        /// <param name="_manager">Poolable Manager to handle the pool objects Instanciation.</param>
        public void InitPool(IPoolInstanceManager<T> _manager)
        {
            if(poolManager != null)
            {
                Debug.LogWarning("This Pool has already been initialized.");
                return;
            }
            poolManager = _manager;
            for (int i = pool.Count; i < pool.Capacity; i++)
            {
                AddNewInstance(); 
            }
        }

        /// <summary>
        /// Create a new Instance and add it to the Pool.
        /// </summary>
        private void AddNewInstance()
        {
            T _instance = poolManager.CreateInstance();
            _instance.OnCreated();

            pool.Add(_instance);
            _instance.OnSentToPool();
        }

        /// <summary>
        /// Get the first available Instance. If there is no Instance in the pool, create and add a new one before getting it.
        /// </summary>
        /// <returns> The first available Instance.</returns>
        public T Get()
        {
            T _instance; 
            if(pool.Count == 0)
            {
                AddNewInstance();
            }
            _instance = pool[0];
            _instance.OnRemovedFromPool(); 
            return _instance;
            
        }

        /// <summary>
        /// Send the Instance Object back to the pool.
        /// </summary>
        /// <param name="_instance">Object sent back to the pool.</param>
        /// <returns>True if the object can be sent in the pool. False otherwise.</returns>
        public bool Send(T _instance)
        {
            // The Instance of the poolable object is already in the pool.
            if (pool.IndexOf(_instance) < 0) return false; 

            pool.Add(_instance);
            _instance.OnSentToPool();
            return true; 
        }
        #endregion 

    }
}
