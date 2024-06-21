using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{

    public Action<GameObject> HandKnead;
    public Action TurnOnKneaderMachine;
    public Action TurnOffKneaderMachine;

    public void OnInvokeHandKneadEvent(GameObject target)
    {
        HandKnead?.Invoke(target);
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

   
}
