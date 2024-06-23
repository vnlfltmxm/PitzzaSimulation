using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MachineBase : MonoBehaviour
{
    [SerializeField]
    private string _type;

    protected bool _isActiveMachine = false;

    [HideInInspector]
    public bool _isTurnOnMachine {  get { return _isActiveMachine; } }
    [HideInInspector]
    public string _machineType { get { return _type; } }
    protected virtual void OnEnable()
    {
        EventManger.Instance.TurnOnMachine += OnActivateMachine;
    }
    protected void RegisterMachinButtonEvent()
    {
    }


    protected void UnRegisterMachinButtonEvent()
    {
    }
    protected virtual void OnActivateMachine(GameObject target)
    {
        if (EventManger.Instance.CheckEventTarget(target, this.gameObject) == false)
        {
            return;
        }

        _isActiveMachine = true;
    }

    protected virtual void OnUnactivateMachine(GameObject target)
    {
        if (EventManger.Instance.CheckEventTarget(target, this.gameObject) == false)
        {
            return;
        }
        _isActiveMachine = false;
    }

  
    


}
