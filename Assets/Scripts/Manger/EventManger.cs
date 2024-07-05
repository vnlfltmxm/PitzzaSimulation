using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class EventManger : Singleton<EventManger>
{
    private Dictionary<GameObject, GameObject> _registedDoughDic= new Dictionary<GameObject, GameObject>();
    public Action<GameObject> HandKnead;
    public Action<GameObject> TurnOffMachine;
    public Action<GameObject> TurnOnMachine;
    public Action<Color,GameObject> OverCooked;
    public Action PlayerCurserLock;
    public Action Packing;
    public Action<Dough> CheckPizza;
    public Action<Transform> DoughMove;
    public Action<string> ClickPlusButton;
    public Action<string> ClickMinuseButton;
    //public Action<string> NPCTalk;
    public Func<Transform> DoughDesPos;
    public Func<Enum,GameObject,bool> CheckNPCState;
    public Action ChangeNPCStateToLeave;
    public Action DayGone;
    public Action DayStart;
    public delegate void MyDelegate(GameObject obj, NPCStateName tempEnum = NPCStateName.WAITINGPIZZA);//이 방식은 action처럼 함수 선언시 모양은 맟춰줘야하지만 디폴트 변수를 사용함으로써 변수사용은 안해도 된다 NPCStateName부분만 
    public MyDelegate CehckOrder;

    // public Action<GameObject,Enum> CheckOrder;

    //public void OnNpcTalkEvent(string text)
    //{
    //    NPCTalk?.Invoke(text);
    //}
    public void OnDayGoneEventInvoke()
    {
        DayGone?.Invoke();
    }
    public void OnDayStartEventInvoke()
    {
        DayStart?.Invoke();
    }

    public void OnClickPlusButtonEvent(string itemName)
    {
        ClickPlusButton?.Invoke(itemName);
    }
    public void OnClickMinuseButtonEvent(string itemName)
    {
        ClickMinuseButton?.Invoke(itemName);
    }
    public void OnPlayerCurserLock()
    {
        PlayerCurserLock?.Invoke();
    }
    public void OnCheckPizzaEventInvoke(Dough pizza)
    {
        CheckPizza?.Invoke(pizza);
    }
    public void OnChangeNPCStatetoLeave()
    {
        ChangeNPCStateToLeave?.Invoke();
    }
    public void OnInvokeCheckOrder(GameObject obj)
    {
        CehckOrder?.Invoke(obj);
    }
    public bool? OnCheckNPCState(Enum statName,GameObject target)
    {
        if(CheckNPCState != null)
        {
            return CheckNPCState?.Invoke(statName,target);
        }
        else
        {
            return false;
        }
    }
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
    public void OnRegiterDouhgDic(GameObject dough)
    {
        if (!_registedDoughDic.ContainsKey(dough))
        {
            _registedDoughDic.Add(dough, dough);
        }
    }
    public void UnRegiterDouhgDic(GameObject dough)
    {
        if (_registedDoughDic.ContainsKey(dough))
        {
            _registedDoughDic.Remove(dough);
        }
    }
    public bool CheckRegisterDough(GameObject dough)
    {
        if (_registedDoughDic.ContainsKey(dough))
        {
            return true;
        }
        return false;
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
