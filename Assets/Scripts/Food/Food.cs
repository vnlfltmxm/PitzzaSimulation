using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    protected MeshRenderer _meshRenderer;

    protected virtual void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    protected virtual void OnEnable()
    {
        ChangeMaterialColor(Color.white);
        RegisterOverCookedEvent();
    }

    private void OnDisable()
    {
        UnRegisterOverCookedEvent();
    }
    protected void ChangeMaterialColor(Color color)
    {
        if(this.gameObject.layer != LayerMask.NameToLayer("Pizza"))
        {
            return;
        }

        _meshRenderer.material.SetColor("_EmissionColor", color);
        _meshRenderer.material.color = color;
    }

    private void RegisterOverCookedEvent()
    {
        EventManger.Instance.OverCooked += ChangeMaterialColor;
    }
    private void UnRegisterOverCookedEvent()
    {
        EventManger.Instance.OverCooked -= ChangeMaterialColor;
    }
}
