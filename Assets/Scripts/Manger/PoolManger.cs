using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PoolManger : Singleton<PoolManger>
{
    //private Dictionary<string, int> _itemCountDic = new Dictionary<string, int>();
    private Dictionary<string, int> _itemMaxCountDic = new Dictionary<string, int>();
    private Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();
    [SerializeField] 
    private GameObject[] Prefabs_Food;
    //private Queue<GameObject> _poolQueue;
    private void Awake()
    {
        RegisterDayEvent();
    }
    private void Start()
    {
        //InitPool(Prefabs_Food);
    }
    private void OnDisable()
    {
        UnRegisterDayEvent();
    }
    private void InitPool(GameObject[] foodList)
    {
        for (int i = 0; i < foodList.Length; i++)
        {
            AddPoolInDictionary(foodList[i]);
            AddObjectInPool(foodList[i]);
            if (DataManger.Inst.IsFileCheck() == false)
            {
                SetItemMaxCountValueInDictionary(foodList[i].name, DataManger.Inst.GetToppingResorceData(foodList[i].tag).StartMaxCount);
            }
            else
            {
                SetItemMaxCountValueInDictionary(foodList[i].name, DataManger.Inst.GetplayerData("플레이어").ToppingResorceCountList[i]);
            }
            //SetItemCountValueInDictionary(foodList[i].name);
            DisableStartPoolAtPool(foodList[i]);
        }
    }
    public void InitPool()
    {
        for (int i = 0; i < Prefabs_Food.Length; i++)
        {
            AddPoolInDictionary(Prefabs_Food[i]);
            AddObjectInPool(Prefabs_Food[i]);
            if (DataManger.Inst.IsFileCheck() == false)
            {
                SetItemMaxCountValueInDictionary(Prefabs_Food[i].name, DataManger.Inst.GetToppingResorceData(Prefabs_Food[i].tag).StartMaxCount);
            }
            else
            {
                if (i < DataManger.Inst.GetplayerData("플레이어").ToppingResorceCountList.Count)
                {
                    SetItemMaxCountValueInDictionary(Prefabs_Food[i].name, DataManger.Inst.GetplayerData("플레이어").ToppingResorceCountList[i]);
                }
                else
                {
                    SetItemMaxCountValueInDictionary(Prefabs_Food[i].name, DataManger.Inst.GetToppingResorceData(Prefabs_Food[i].tag).StartMaxCount);
                }
            }
            //SetItemCountValueInDictionary(foodList[i].name);
            DisableStartPoolAtPool(Prefabs_Food[i]);
            SetPoolPosionY(Prefabs_Food[i].name);
        }
    }

    private void AddObjectInPool(GameObject prefabObj)
    {
        _poolDic.TryGetValue(prefabObj.name, out Queue<GameObject> pool);
        var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(prefabObj.tag);

        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(prefabObj, parent);
            pool.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    private void DisableStartPoolAtPool(GameObject pool)
    {
        var name = pool.gameObject.tag;
        var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(name);
        var player = DataManger.Inst.GetplayerData("플레이어");
        if (player.StartToppingResorceList.Contains(name))
        {
            parent.gameObject.SetActive(true);
        }
        else
        {
            parent.gameObject.SetActive(false);
        }
    }
    public void SetToppingZoneAtPool()
    {
        foreach (var item in PlayerController.Instance.PizaaToppingResorce)
        {
            var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(item);
            if (_itemMaxCountDic[item] > 0)
            {
                parent.gameObject.SetActive(true);
            }
           
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
        //MinusItemCountValueInDictionary(pool.tag);
        PlusItemMaxCountValueInDictionary(pool.tag);
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
            //PlusItemCountValueInDictionary(pool.tag);
            MinusItemMaxCountValueInDictionary(pool.tag);
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
        float maxVaule = DataManger.Inst.GetToppingResorceData(pool.tag).StartMaxCount;
        float m = (0.23f / (maxVaule - 1)) * (_itemMaxCountDic[pool.tag] - 1) + 0.77f;
       // float m = -0.23f / maxVaule;
        //float b = 1.0f;

        Vector3 resultPos = new Vector3(targetPool.gameObject.transform.localPosition.x, m/* * _itemMaxCountDic[pool.tag] + b*/, targetPool.gameObject.transform.localPosition.z);
        if (_itemMaxCountDic[pool.tag] >= maxVaule)
        {
            resultPos = new Vector3(targetPool.gameObject.transform.localPosition.x, 1, targetPool.gameObject.transform.localPosition.z);
        }
        targetPool.gameObject.transform.localPosition = resultPos;
        PoolMeshEnableToCount(targetPool.gameObject);
    }
    public void SetPoolPosionY(string pool)
    {
        var targetPool = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(pool);


        if (targetPool.CompareTag("Dough"))
        {
            return;
        }
        float maxVaule = DataManger.Inst.GetToppingResorceData(pool).StartMaxCount;
        float m = (0.23f / (maxVaule - 1)) * (_itemMaxCountDic[pool] - 1) + 0.77f;
        //float m = -0.23f / maxVaule;
        //float b = 1.0f;

        Vector3 resultPos = new Vector3(targetPool.gameObject.transform.localPosition.x, m /** _itemMaxCountDic[pool] + b*/, targetPool.gameObject.transform.localPosition.z);
        if (_itemMaxCountDic[pool] >= maxVaule)
        {
            resultPos = new Vector3(targetPool.gameObject.transform.localPosition.x, 1, targetPool.gameObject.transform.localPosition.z);
        }
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

        if (0/*_itemCountDic[pool.tag]*/ == _itemMaxCountDic[pool.tag])
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
    public bool CheckItemCount(string itemName)
    {

        if (_itemMaxCountDic[itemName] > 0/*_itemCountDic[itemName]*/)
        {
            return true;
        }

        return false;
    }
    private void SetItemMaxCountValueInDictionary(string itemName, int maxCount)
    {
        if (_itemMaxCountDic.ContainsKey(itemName) == false)
        {
            _itemMaxCountDic.Add(itemName, maxCount);
        }
        else
        {
            _itemMaxCountDic[itemName] += maxCount; /*- _itemCountDic[itemName];*/
           // SetPoolPosionY(itemName);
            //_itemCountDic[itemName] = 0;
        }
    }
    //private void SetItemMaxCountValueInDictionary(string itemName, int maxCount,int itemCount)
    //{
    //    if (_itemMaxCountDic.ContainsKey(itemName) == false)
    //    {
    //        _itemMaxCountDic.Add(itemName, maxCount);
    //    }
    //    else
    //    {
    //        _itemMaxCountDic[itemName] += (maxCount - itemCount);
    //    }
    //}
    //private void SetItemCountValueInDictionary(string itemName)
    //{
    //    if (!_itemCountDic.ContainsKey(itemName))
    //    {
    //        _itemCountDic.Add(itemName, 0);
    //    }
    //}

    //private void PlusItemCountValueInDictionary(string itemName)
    //{
    //    if (_itemCountDic.ContainsKey(itemName))
    //    {
    //        _itemCountDic[itemName]++;
    //    }
    //    else
    //    {
    //        _itemCountDic.Add(itemName, 1);
    //    }
    //}
    //private void MinusItemCountValueInDictionary(string itemName)
    //{
    //    if (_itemCountDic.ContainsKey(itemName))
    //    {
    //        _itemCountDic[itemName]--;
    //    }
    //}

    private void PlusItemMaxCountValueInDictionary(string itemName)
    {
        if (_itemMaxCountDic.ContainsKey(itemName))
        {
            _itemMaxCountDic[itemName]++;
        }
        else
        {
            _itemMaxCountDic.Add(itemName, 1);
        }
    }
    private void MinusItemMaxCountValueInDictionary(string itemName)
    {
        if (_itemMaxCountDic.ContainsKey(itemName))
        {
            _itemMaxCountDic[itemName]--;
        }
    }
    public void DayGone()
    {
        foreach (var item in PlayerController.Instance.PizaaToppingResorce)
        {
            SetItemMaxCountValueInDictionary(item, ShopManger.Instance.GetShopingValue(item));
        }

        SetToppingZoneAtPool();
        //ResetItemCountValue();
    }

    public void DayStart()
    {
        foreach (var item in PlayerController.Instance.PizaaToppingResorce)
        {
            SetPoolPosionY(item);
        }

        //ResetItemCountValue();
    }
    public List<int> ReturnToppingResorceCount()
    {
        List<int> ret = new List<int>();

        foreach (var item in PlayerController.Instance.PizaaToppingResorce)
        {
            ret.Add(_itemMaxCountDic[item]);
        }

        return ret;
    }

    //private void ResetItemCountValue()
    //{
    //    foreach (var item in _itemCountDic.Keys)
    //    {
    //        _itemCountDic[item] = 0;
    //    }
    //}
    private void RegisterDayEvent()
    {
        EventManger.Instance.DayGone += DayGone;
        EventManger.Instance.DayStart += DayStart;
    }
    private void UnRegisterDayEvent()
    {
        EventManger.Instance.DayGone -= DayGone;
        EventManger.Instance.DayStart -= DayStart;
    }
}
