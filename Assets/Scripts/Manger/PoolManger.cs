using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManger : Singleton<PoolManger>
{
    private Dictionary<string, int> _itemCountDic = new Dictionary<string, int>();
    private Dictionary<string, int> _itemMaxCountDic = new Dictionary<string, int>();
    private Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();
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
        ReturnItemInPool(item);
        MinusItemCountValueInDictionary(pool.tag);
        SetPoolPosionY(pool);
    }
    public void ReturnItemInPool(GameObject item, string targetTagName,bool isTargetTagInclude)
    {
        if (item.transform.childCount > 0)
        {
            for (int i = item.transform.childCount - 1; i >= 0; i--)
            {
                var child = item.transform.GetChild(i);

                ReturnItemInPool(child.gameObject, targetTagName, isTargetTagInclude);
            }
        }

        if (_poolDic.ContainsKey(item.tag) == false || item.CompareTag(targetTagName) != isTargetTagInclude) 
        {
            return;
        }

        _poolDic[item.tag].Enqueue(item);

        var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(item.tag);
        item.transform.parent = parent.transform;
        item.SetActive(false);

    }
    public void ReturnItemInPool(GameObject item)
    {
        if (item.transform.childCount > 0)
        {
            for (int i = item.transform.childCount - 1; i >= 0; i--)  
            {
                var child = item.transform.GetChild(i);

                ReturnItemInPool(child.gameObject);
            }
        }

        if (_poolDic.ContainsKey(item.tag) == false) 
        {
            return;
        }

        _poolDic[item.tag].Enqueue(item);

        var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(item.tag);
        item.transform.parent = parent.transform;
        item.SetActive(false);

    }
    public GameObject OutPoolItem(GameObject pool)
    {
        if (CheckItemCount(pool.tag))
        {
            GameObject item = _poolDic[pool.tag].Dequeue();
            PlusItemCountValueInDictionary(pool.tag);
            SetPoolPosionY(pool);
            return item;
        }
        else
        {
            Debug.Log("재료 소진");
            return null;
        }
    }
    public void SetPoolPosionY(GameObject pool)
    {
        var targetPool = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(pool.tag);
        
        
        if (targetPool.CompareTag("Dough"))
        {
            return;
        }
        float m = -0.23f / _itemMaxCountDic[pool.tag];
        float b = 1.0f;

        Vector3 resultPos = new Vector3(targetPool.gameObject.transform.localPosition.x, m * _itemCountDic[pool.tag] + b, targetPool.gameObject.transform.localPosition.z);
        targetPool.gameObject.transform.localPosition = resultPos;
        PoolMeshEnableToCount(targetPool.gameObject);
    }
    private void PoolMeshEnableToCount(GameObject pool)
    {
        var chiled = pool.transform.GetChild(0);
        if(chiled == null)
        {
            return;
        }

        if (_itemCountDic[pool.tag] == _itemMaxCountDic[pool.tag])
        {
            chiled.gameObject.SetActive(false);
        }
        else
        {
            if (chiled.gameObject.activeSelf == false) 
            {
                chiled.gameObject.SetActive(true);
            }
        }
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
