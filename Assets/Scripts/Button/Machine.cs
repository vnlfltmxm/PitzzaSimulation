using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MachineBase : MonoBehaviour
{
    [SerializeField]
    private string _type;

    private bool _isActiveMachine = false;

    [HideInInspector]
    public bool _isTurnOnMachine {  get { return _isActiveMachine; } }
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
