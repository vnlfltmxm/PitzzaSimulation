using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManger : Singleton<PoolManger>
{
    [HideInInspector]
    public Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();

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
}
