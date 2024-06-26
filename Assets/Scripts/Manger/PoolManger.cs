using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManger : Singleton<PoolManger>
{
    [HideInInspector]
    public Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();
    [HideInInspector]
    public Queue<GameObject> _usingPool = new Queue<GameObject>();
    [SerializeField] 
    private GameObject Prefabs_Dough;
    //private Queue<GameObject> _poolQueue;
    private void Awake()
    {
        AddPoolInDictionary(Prefabs_Dough);

        AddObjectInPool(Prefabs_Dough);
    }

    private void AddObjectInPool(GameObject prefabObj)
    {
        string prefabName = prefabObj.name;
        _poolDic.TryGetValue(prefabName, out Queue<GameObject> pool);
        var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(prefabName);

        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefabObj, parent);
            pool.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    private void AddPoolInDictionary(GameObject prefabObj)
    {
        _poolDic.Add(prefabObj.gameObject.name, new Queue<GameObject>());
    }

    public Queue<GameObject> GetPoolToTagName(string tagName)
    {
        _poolDic.TryGetValue(tagName, out Queue<GameObject> pool);
        return pool;
    }

    public void InPoolItem(GameObject pool,GameObject item)
    {
        _poolDic[pool.tag].Enqueue(item);
        item.transform.parent = pool.transform;
        item.SetActive(false);
    }

    public GameObject OutPoolItem(GameObject pool)
    {
        GameObject item = _poolDic[pool.tag].Dequeue();
        return item;
    }
}
