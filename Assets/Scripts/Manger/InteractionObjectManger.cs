using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionObjectManger : Singleton<InteractionObjectManger>
{
    [SerializeField]
    private GameObject _pizzaHouse;
    [SerializeField]
    private GameObject _toppingZone;

  

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

        foreach (Transform child in _toppingZone.transform)
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
    public void OnDropItemToMachine(GameObject item, GameObject machine, Vector3 hitPointPos)
    {
        item.transform.parent = null;
        item.transform.position = hitPointPos;
        item.transform.rotation = machine.transform.rotation;
    }
    //public void OnChangeNPCState(GameObject NPC)
    //{
    //    var target=NPC.GetComponent<NPCController>();
    //    if (target == null)
    //    {
    //        return;
    //    }

    //    target.ChangeNPCState(NPCStateName.WAITINGPIZZA);
    //}
    public void OnToppingPizza(GameObject item, GameObject pizza, Vector3 hitPointPos)
    {
        GameObject pizzaItem = PoolManger.Instance.OutPoolItem(item);

        if (pizzaItem == null)
        {
            item.transform.parent = pizza.transform;
            item.transform.position = hitPointPos;
            item.transform.rotation = pizza.transform.rotation;
            if (item.CompareTag("Cheese") == true)
            {
                item.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            else
            {
                item.layer = LayerMask.NameToLayer("Pizza");
            }
            Debug.Log("재료소진2");
        }
        else
        {
            pizzaItem.SetActive(true);
            pizzaItem.transform.parent = pizza.transform;
            pizzaItem.transform.position = hitPointPos;
            if (pizzaItem.CompareTag("Cheese") == true || pizzaItem.CompareTag("Sauce") == true) 
            {
                pizzaItem.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
            else
            {
                pizzaItem.layer = LayerMask.NameToLayer("Pizza");
            }
            SetUseGravityInItem(pizzaItem, false);
        }
        
       // PoolManger.Instance.SetPoolPosionY(item);
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
        if(item == null) 
        {
            return;
        }
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

    public void UnRegisterChangeNPCState()
    {
        EventManger.Instance.CehckOrder -= OnChangeNPCState;
    }
    public void OnRegisterChangeNPCState()
    {
        EventManger.Instance.CehckOrder += OnChangeNPCState;
    }
    public void OnChangeNPCState(GameObject NPCObj, NPCStateName stateName = NPCStateName.WAITINGPIZZA)
    {
        var NPC=NPCObj.GetComponent<NPCController>();

        if(NPC == null)
        {
            return;
        }

        NPC.ChangeNPCState(stateName);
    }

    public bool OnCheckNPCState(GameObject targetNPC,NPCStateName stateName)
    {
        if (EventManger.Instance.OnCheckNPCState(stateName, targetNPC) == true)
        {
            return true;
        }

        return false;
    }
}
