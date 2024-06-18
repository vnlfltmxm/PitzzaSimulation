using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T> where T : class
{
    protected T Owner { get; set; }

    public BaseState(T owner)
    {
        this.Owner = owner;
    }

    public virtual void OnEnterState() { }
    public virtual void OnUpdateState() { }
    public virtual void OnFixedUpdateState() { }
    public virtual void OnExitState() { }
}
