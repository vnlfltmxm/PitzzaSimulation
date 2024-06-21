using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButtonBase : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _usingCollider;
    private string _tagName;

    protected virtual void OnEnable()
    {
        _tagName = gameObject.tag;
    }

    public void RegisterMachinButtonEvent(string tagName, Action action)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                EventManger.Instance.KneaderMachine += action;
                break;
            case "PackagingMachine":
                break;

        }
    }
    public void UnRegisterMachinButtonEvent(string tagName, Action action)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                EventManger.Instance.KneaderMachine -= action;
                break;
            case "PackagingMachine":
                break;

        }
    }
    protected virtual void SetUsingCollider(bool value)
    {
        _usingCollider.enabled = value;
    }


}
