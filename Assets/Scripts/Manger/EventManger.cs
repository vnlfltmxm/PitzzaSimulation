using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{

    public Action<GameObject> HandKnead;
    public Action<GameObject> TurnOffMachine;
    public Action<GameObject> TurnOnMachine;

    public Action<Transform> DoughMove;
    public Func<Transform> DoughDesPos;


    public void OnDoughMoveEvent()
    {
        var doughPos = DoughDesPos.Invoke();

        if(doughPos == null)
        {
            return;
        }
        DoughMove?.Invoke(doughPos);

    }

    public void OnInvokeHandKneadEvent(GameObject target)
    {
        HandKnead?.Invoke(target);
    }

    public void OnMachineActivateEvent(GameObject targetMachine)
    {
        TurnOnMachine?.Invoke(targetMachine);
    }
    public void OnMachineDeactivateEvent(GameObject targetMachine)
    {
        TurnOffMachine?.Invoke(targetMachine);
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
