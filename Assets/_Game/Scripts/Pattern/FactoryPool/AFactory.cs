using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFactory<T> : ScriptableObject where T : PoolElement
{
    protected Queue<T> pool = new Queue<T>();
    [SerializeField] protected T prefab;
    [SerializeField] protected int poolSize = 10;
    public virtual void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public virtual T GetObject()
    {
        if (pool.Count == 0)
        {
            T extendObj = Instantiate(prefab);;
            extendObj.gameObject.SetActive(false);
            pool.Enqueue(extendObj);
        }
        T obj = pool.Dequeue();
        obj.OnInit();
        obj.gameObject.SetActive(true);
        return obj;
    }
    public virtual void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
