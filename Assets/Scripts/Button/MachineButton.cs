using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButtonBase : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _usingCollider;
    protected string _tagName;

    protected virtual void OnEnable()
    {
        _tagName = gameObject.tag;
        
    }

   
    protected virtual void DisabeleUsingCollider()
    {
        _usingCollider.enabled = false;
    }

    protected virtual void EnableUsingCollider()
    {
        _usingCollider.enabled = true;
    }
}
