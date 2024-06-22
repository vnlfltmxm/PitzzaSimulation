using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{

    public Action<GameObject> HandKnead;
    public Action TurnOnKneaderMachine;
    public Action TurnOffKneaderMachine;
    public Action<GameObject> TurnOffMachine;
    public Action<GameObject> TurnOnMachine;

    public void OnInvokeHandKneadEvent(GameObject target)
    {
        HandKnead?.Invoke(target);
    }

    public void OnMachineActivateEvent(GameObject targetMachine)
    {
        TurnOnMachine?.Invoke(targetMachine);
    }
    public void OnMachineUnactivateEvent(GameObject targetMachine)
    {
        TurnOffMachine?.Invoke(targetMachine);
    }

    public void OnMachineButtonPushEvent(string tagName)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                TurnOnKneaderMachine?.Invoke();
                break;
            case "PackagingMachine":
                break;

        }
    }
    public bool CheckEventTarget(GameObject target, GameObject thisObj)
    {
        if (target == null || target != thisObj)
        {
            return false;
        }

        return true;
    }

}
