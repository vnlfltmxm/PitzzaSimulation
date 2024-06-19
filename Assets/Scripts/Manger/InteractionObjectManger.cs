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

    public void OnPickUpItem(string tagName,GameObject grapPos)
    {
        OnGetItemToTag(tagName);
        OnGrapItemToPlayer(_usingPool, grapPos);
    }

    private void OnGetItemToTag(string tagName)
    {
        _usingPool = PoolManger.Instance.GetPoolToTagName(tagName);
    }
    private void OnGetItemToTag(string tagName,GameObject grapPos)
    {
        _usingPool = PoolManger.Instance.GetPoolToTagName(tagName);
    }

    private void OnGrapItemToPlayer(Queue<GameObject> pool,GameObject grapPos)
    {
        GameObject item = pool.Dequeue();
        item.SetActive(true);
        item.transform.position = grapPos.transform.position;
        item.transform.rotation = grapPos.transform.rotation;
        item.transform.parent = grapPos.transform;
    }

}
