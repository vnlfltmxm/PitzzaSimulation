using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KneaderMachine : MachineBase
{
    [SerializeField]
    private GameObject _doughDesPos;


    protected override void OnEnable()
    {
        base.OnEnable();
        EventManger.Instance.DoughDesPos += TransMissionDoughDesPos;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManger.Instance.DoughDesPos -= TransMissionDoughDesPos;
    }
    public Transform TransMissionDoughDesPos()
    {
        return _doughDesPos.transform;
    }
}
