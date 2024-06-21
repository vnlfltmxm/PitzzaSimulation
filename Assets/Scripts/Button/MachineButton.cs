using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButtonBase : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _usingCollider;
    protected string _tagName;

    protected virtual void OnEnable()
    {
        _tagName = gameObject.tag;
        RegisterMachinButtonEvent(_tagName, EnableUsingCollider, DisabeleUsingCollider);
        
    }

    protected void RegisterMachinButtonEvent(string tagName, Action pushAction, Action unPushAction)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                EventManger.Instance.TurnOnKneaderMachine += pushAction;
                EventManger.Instance.TurnOffKneaderMachine += unPushAction;
                break;
            case "PackagingMachine":
                break;

        }
    }

    
    protected void UnRegisterMachinButtonEvent(string tagName, Action action, Action unPushAction)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                EventManger.Instance.TurnOnKneaderMachine -= action;
                EventManger.Instance.TurnOffKneaderMachine -= unPushAction;
                break;
            case "PackagingMachine":
                break;

        }
    }
    protected virtual void DisabeleUsingCollider()
    {
        _usingCollider.enabled = false;
    }

    protected virtual void EnableUsingCollider()
    {
        _usingCollider.enabled = true;
    }
}
