using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenMachine : MachineBase
{
    [SerializeField]
    private GameObject _ovenGlass;

    private Color _black = new Color(0, 0, 0, 0.7f);
    private Color _red = new Color(1, 0, 0, 0.7f);

    protected override void OnEnable()
    {
        base.OnEnable();
        ChangeOvenGlassColor(_black);
    }

    protected override void OnActivateMachine(GameObject target)
    {
        base.OnActivateMachine(target);

        if (_isActiveMachine)
        {
            ChangeOvenGlassColor(_red);
        }
        else
        {
            ChangeOvenGlassColor(_black);
        }
    }

    private void ChangeOvenGlassColor(Color color)
    {
        _ovenGlass.GetComponent<MeshRenderer>().material.color = color;
    }

}
