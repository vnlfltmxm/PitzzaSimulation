using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Machine : MonoBehaviour
{
    [SerializeField]
    private string _type;

    private bool _isActiveMachine = false;

    [HideInInspector]
    public bool _isTurnOnMachine {  get { return _isActiveMachine; } }
    private void OnActivateMachine(GameObject target)
    {
        if(target == null || target != this.gameObject)
        {
            return;
        }

        _isActiveMachine = true;
    }

    private void OnUnactivateMachine(GameObject target)
    {
        if (target == null || target != this.gameObject)
        {
            return;
        }
        _isActiveMachine = false;
    }

    private void RegisterOnMachineEvent(GameObject target)
    {
        EventManger.Instance.TurnOnMachine += OnActivateMachine;
    }

    private void RegisterOffMachineEvent(GameObject target)
    {
        EventManger.Instance.TurnOffMachine += OnUnactivateMachine;
    }



}
