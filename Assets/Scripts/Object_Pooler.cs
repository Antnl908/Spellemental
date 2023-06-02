using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Object_Pooler : MonoBehaviour
{
    //Made by Daniel.

    /// <summary>
    /// A class that is used in the inspector to add an object to the object pool.
    /// It sets the name of the pool, the object to pool, 
    /// the minimum amount of objects in the pool and the maximum amount of objects in the pool.
    /// </summary>
    [Serializable]
    public class ObjectToPool
    {
        [SerializeField]
        private string name = "Default";

        public string Name { get { return name; } }

        [SerializeField]
        private Pooling_Object poolingObject;

        public Pooling_Object PoolingObject { get { return poolingObject; } }

        [SerializeField]
        private int poolCount = 10;

        public int PoolCount { get { return poolCount; } }

        [SerializeField]
        private int maxPoolCount = 100;

        public int MaxPoolCount { get { return maxPoolCount; } }

        /// <summary>
        /// The method that creates the pooling object in the object pool.
        /// </summary>
        /// <returns>Returns the created Pooling_Object</returns>
        public Pooling_Object CreateObject()
        {
            var returnedPoolingObject = Instantiate(poolingObject);

            return returnedPoolingObject;
        }

        /// <summary>
        /// Sets the pooled object to active when it is retrieved from the pool.
        /// </summary>
        /// <param name="poolingObject">The pooling object that is retrieved</param>
        public void OnGetPoolingObject(Pooling_Object poolingObject)
        {
            poolingObject.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the pooled object to inactive when it is released to the pool.
        /// </summary>
        /// <param name="poolingObject">The released pooling object</param>
        public void OnReleasePoolingObject(Pooling_Object poolingObject)
        {
            poolingObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// Destroys the pooling object if there is no space for it in the pool.
        /// </summary>
        /// <param name="poolingObject">The destroyed pooling object</param>
        public void OnDestroyPoolingObject(Pooling_Object poolingObject)
        {
            Destroy(poolingObject.gameObject);
        }
    }

    [SerializeField]
    private List<ObjectToPool> objectPools;

    private static Dictionary<string, IObjectPool<Pooling_Object>> pools;

    public static Dictionary<string, IObjectPool<Pooling_Object>> Pools { get => pools; }

    /// <summary>
    /// Creates all of the object pools.
    /// </summary>
    private void Start()
    {
        pools = new();

        foreach (var pool in objectPools)
        {
            Pools.Add(pool.Name, new ObjectPool<Pooling_Object>(pool.CreateObject, pool.OnGetPoolingObject,
                      pool.OnReleasePoolingObject, pool.OnDestroyPoolingObject, true,
                      pool.PoolCount, pool.MaxPoolCount));
        }
    }
}
