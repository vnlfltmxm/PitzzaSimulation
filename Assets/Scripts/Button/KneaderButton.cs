using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KneaderButton : MachineButtonBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //base.RegisterMachinButtonEvent(_tagName, EnableUsingCollider, DisabeleUsingCollider);
    }
}
