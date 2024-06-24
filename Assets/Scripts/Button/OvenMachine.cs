using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenMachine : MachineBase
{
    [SerializeField]
    private GameObject _ovenGlass;

    protected override void OnEnable()
    {
        base.OnEnable();
        _ovenGlass.SetActive(false);
    }

    protected override void OnActivateMachine(GameObject target)
    {
        base.OnActivateMachine(target);

        if (_isActiveMachine)
        {
            _ovenGlass.SetActive(true);
        }
        else
        {
            _ovenGlass.SetActive(false);
        }
    }
}
