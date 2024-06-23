using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KneaderMachine : MachineBase
{
    [SerializeField]
    private GameObject _doughDesPos;


    private void OnEnable()
    {
        EventManger.Instance.DoughDesPos += TransMissionDoughDesPos;
    }
    public Transform TransMissionDoughDesPos()
    {
        return _doughDesPos.transform;
    }
}
