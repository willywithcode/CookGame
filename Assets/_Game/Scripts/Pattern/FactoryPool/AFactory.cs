using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFactory<T> : ScriptableObject where T : PoolElement
{
    // Pool chua cac doi tuong
    protected readonly Queue<T> pool = new Queue<T>();
    // Prefab cua doi tuong
    [SerializeField] protected T prefab;
    // So luong doi tuong trong pool
    [SerializeField] protected int poolSize = 10;
    // Root chua cac doi tuong
    private Transform root;
    // Tao pool
    public Transform Root => root;
    public virtual void CreatePool()
    {
        root = new GameObject(prefab.name + "Pool").transform;
        root.SetParent(GameManager.Instance.transform);
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
            obj.TF.SetParent(root);
        }
    }
    // Lay doi tuong tu pool
    public virtual T GetObject(Vector3 pos = default,Vector3 scale = default, Vector3 rot = default, Transform parent = null)
    {
        if(root == null) CreatePool();
        if (pool.Count == 0)
        {
            T extendObj = Instantiate(prefab);;
            extendObj.gameObject.SetActive(false);
            extendObj.transform.SetParent(root);
            pool.Enqueue(extendObj);
        }
        T obj = pool.Dequeue();
        obj.OnInit();
        obj.gameObject.SetActive(true);
        if(parent != null) obj.TF.SetParent(parent);
        obj.TF.position = pos;
        obj.TF.localScale = scale == default ? Vector3.one : scale;
        obj.TF.eulerAngles = rot;
        return obj;
    }
    // Tra doi tuong vao pool
    public virtual void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        obj.TF.SetParent(root);
    }
#if UNITY_EDITOR
    // Setup prefab va pool size
    // Dung trong Inspector
    public void SetUp(T prefab, int poolSize = 10)
    {
        this.prefab = prefab;
        this.poolSize = poolSize;
    }
#endif
}
