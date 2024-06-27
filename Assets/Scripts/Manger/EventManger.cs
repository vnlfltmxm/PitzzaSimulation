using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{

    public Action<GameObject> HandKnead;
    public Action<GameObject> TurnOffMachine;
    public Action<GameObject> TurnOnMachine;
    public Action<Color,GameObject> OverCooked;
    public Action Packing;
    public Action<Transform> DoughMove;
    //public Action<string> NPCTalk;
    public Func<Transform> DoughDesPos;
    
    //public void OnNpcTalkEvent(string text)
    //{
    //    NPCTalk?.Invoke(text);
    //}

    public void OnRegisterPackingEvent(Action action)
    {
        Packing += action;
    }
    public void OnUnRegisterPackingEvent(Action action)
    {
        Packing -= action;
    }
    public void OnPackingEvent()
    {
        Packing?.Invoke();
    }
    public void OnDoughMoveEvent()
    {
        var doughPos = DoughDesPos.Invoke();

        if(doughPos == null)
        {
            return;
        }
        DoughMove?.Invoke(doughPos);

    }

    public void OnRegisterOverCookedEvent(Action<Color,GameObject> action)
    {
        OverCooked += action;
    }

    public void OnOverCookedEvent(GameObject target)
    {
        OverCooked?.Invoke(Color.black,target);
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
