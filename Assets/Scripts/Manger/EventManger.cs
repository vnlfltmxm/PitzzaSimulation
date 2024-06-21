using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{

    public Action<GameObject> HandKnead;
    public Action KneaderMachine;

    public void OnInvokeHandKneadEvent(GameObject target)
    {
        HandKnead?.Invoke(target);
    }

    public void OnMachineButtonEvent(string tagName)
    {
        switch (tagName)
        {
            case "Oven":
                break;
            case "Kneader":
                KneaderMachine?.Invoke();
                break;
            case "PackagingMachine":
                break;

        }
    }

   
}
