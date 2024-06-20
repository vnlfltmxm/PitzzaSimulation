using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManger : Singleton<PoolManger>
{
    private Dictionary<string, int> _itemCountDic = new Dictionary<string, int>();
    private Dictionary<string, int> _itemMaxCountDic = new Dictionary<string, int>();
    [HideInInspector]
    public Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();
    [HideInInspector]
    public Queue<GameObject> _usingPool = new Queue<GameObject>();
    [SerializeField] 
    private GameObject[] Prefabs_Food;
    //private Queue<GameObject> _poolQueue;
    private void Awake()
    {
        InitPool(Prefabs_Food);
    }

    private void InitPool(GameObject[] foodList)
    {
        for (int i = 0; i < foodList.Length; i++)
        {
            AddPoolInDictionary(foodList[i]);
            AddObjectInPool(foodList[i]);
            SetItemMaxCountValueInDictionary(foodList[i].name, 5);
            SetItemCountValueInDictionary(foodList[i].name);
        }
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
        MinusItemCountValueInDictionary(pool.tag);
    }

    public GameObject OutPoolItem(GameObject pool)
    {
        if (CheckItemCount(pool.tag))
        {
            GameObject item = _poolDic[pool.tag].Dequeue();
            PlusItemCountValueInDictionary(pool.tag);
            return item;
        }
        else
        {
            Debug.Log("재료 소진");
            return null;
        }
    }

    private void AddItemMaxCountInDictionary(GameObject item)
    {

    }
    private bool CheckItemCount(string itemName)
    {

        if (_itemMaxCountDic[itemName] > _itemCountDic[itemName])
        {
            return true;
        }

        return false;
    }
    private void SetItemMaxCountValueInDictionary(string itemName, int maxCount)
    {
        _itemMaxCountDic.Add(itemName, maxCount);
    }
    private void SetItemCountValueInDictionary(string itemName)
    {
        if (!_itemCountDic.ContainsKey(itemName))
        {
            _itemCountDic.Add(itemName, 0);
        }
    }

    private void PlusItemCountValueInDictionary(string itemName)
    {
        if (_itemCountDic.ContainsKey(itemName))
        {
            _itemCountDic[itemName]++;
        }
    }
    private void MinusItemCountValueInDictionary(string itemName)
    {
        if (_itemCountDic.ContainsKey(itemName))
        {
            _itemCountDic[itemName]--;
        }
    }
}
