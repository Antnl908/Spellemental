using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Object_Pooler : MonoBehaviour
{
    //Made by Daniel.

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

        public Pooling_Object CreateObject()
        {
            var returnedPoolingObject = Instantiate(poolingObject);

            return returnedPoolingObject;
        }

        public void OnGetPoolingObject(Pooling_Object poolingObject)
        {
            poolingObject.gameObject.SetActive(true);
        }

        public void OnReleasePoolingObject(Pooling_Object poolingObject)
        {
            poolingObject.gameObject.SetActive(false);
        }

        public void OnDestroyPoolingObject(Pooling_Object poolingObject)
        {
            Destroy(poolingObject.gameObject);
        }
    }

    [SerializeField]
    private List<ObjectToPool> objectPools;

    private static Dictionary<string, IObjectPool<Pooling_Object>> pools;

    public static Dictionary<string, IObjectPool<Pooling_Object>> Pools { get => pools; }

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
