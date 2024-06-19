using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionObjectManger : Singleton<InteractionObjectManger>
{
    [SerializeField]
    private GameObject _pizzaHouse;
    public GameObject _doughtRefrigerator;

    public Action<string,GameObject> _pickUpItem;

    private Queue<GameObject> _usingPool = new Queue<GameObject>();

    public Transform FindPrefabsParentTrasnform(string tagName)
    {

        foreach (Transform child in _pizzaHouse.transform)
        {
            if (child.CompareTag(tagName))
            {
                return child;
            }
        }
        return null;
    }

    //private void RegisterPickUpItemEvent()
    //{
    //    _pickUpItem += OnGetItemToTag;
    //    _pickUpItem += ;
    //}

    public void OnPickUpItem(GameObject item, GameObject grapPos)
    {
        SetUseGravityInItem(item, false);
        item.transform.position = grapPos.transform.position;
        item.transform.rotation = grapPos.transform.rotation;
        item.transform.parent = grapPos.transform;
    }
    public void OnDropItem(GameObject item)
    {
        item.transform.parent = null;
        SetUseGravityInItem(item, true);
    }
    public void OnReturnHandlingItemToPool(GameObject pool,GameObject item)
    {
        //OnGetItemPoolToTag(pool.gameObject.name);
        OnReturnItemToPool(pool, item);
    }
    //public void OnPickUpItemToPool(string tagName,GameObject grapPos)
    //{
    //    OnGetItemPoolToTag(tagName);
    //    //OnGrapItemToPlayer(_usingPool, grapPos);
    //}
    public void OnPickUpItemToPool(GameObject pool, GameObject grapPos)
    {
        OnGrapItemInPoolToPlayer(pool, grapPos);
    }
    private void OnGetItemPoolToTag(string tagName)
    {
        _usingPool = PoolManger.Instance.GetPoolToTagName(tagName);
    }
    private void OnGetItemPoolToTag(string tagName,GameObject grapPos)
    {
        _usingPool = PoolManger.Instance.GetPoolToTagName(tagName);
    }

    private void OnGrapItemInPoolToPlayer(GameObject pool,GameObject grapPos)
    {
        GameObject item = PoolManger.Instance.OutPoolItem(pool);
        item.SetActive(true);
        
        item.transform.position = grapPos.transform.position;
        item.transform.rotation = grapPos.transform.rotation;
        item.transform.parent = grapPos.transform;
        SetUseGravityInItem(item, false);
    }
    private void OnReturnItemToPool(GameObject pool, GameObject item)
    {
        PoolManger.Instance.InPoolItem(pool, item);
       
    }

    private void SetUseGravityInItem(GameObject item, bool value)
    {
        Rigidbody rd= item.GetComponent<Rigidbody>();
        if (rd == null) 
        {
            return;
        }
        
        SetConstraintsInItem(rd, value);
        rd.useGravity = value;

    }
    private void SetConstraintsInItem(Rigidbody rd, bool value)
    {
        if (value == true) 
        {
            rd.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rd.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
