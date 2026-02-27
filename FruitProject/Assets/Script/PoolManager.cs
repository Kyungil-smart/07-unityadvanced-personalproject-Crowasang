using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    Dictionary<FruitData, Queue<GameObject>> _pools = new Dictionary<FruitData, Queue<GameObject>>();
    
    
    private void Awake()
    {
        Instance = this;
        
    }

    public GameObject Get(FruitData data, Vector2 position)
    {
        if (!_pools.ContainsKey(data))
        {
            _pools.Add(data, new Queue<GameObject>());
        }

        GameObject obj;
        
        if (_pools[data].Count > 0)
        {
            obj = _pools[data].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(data._fruitPrefab);
        }
        obj.transform.position = position;
        obj.GetComponent<FruitObject>().Init(data);

        return obj;

    }
    
    public void Release(FruitData data, GameObject obj)
    {
        if (!_pools.ContainsKey(data))
        {
            _pools.Add(data, new Queue<GameObject>());
        }
        
        if (obj.TryGetComponent(out IPoolable poolable))
        {
            poolable.OnDespawn();
        }
        
        obj.SetActive(false);
        _pools[data].Enqueue(obj);
    }
    
}
