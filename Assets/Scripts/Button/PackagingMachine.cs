using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagingMachine : MachineBase
{
    [SerializeField]
    private GameObject _packingZone;
    [SerializeField]
    private GameObject _packingZoneParent;
    [SerializeField]
    private GameObject _machineFloor;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManger.Instance.OnRegisterPackingEvent(ReversMovePackingZone);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManger.Instance.OnUnRegisterPackingEvent(ReversMovePackingZone);
    }

    protected override void OnActivateMachine(GameObject target)
    {
        if (EventManger.Instance.CheckEventTarget(target, this.gameObject) == false || _isActiveMachine == true)
        {
            return;
        }

        _isActiveMachine = true;
        StartCoroutine(MovePackingZone(_machineFloor.transform, _packingZone));
    }

    public IEnumerator MovePackingZone(Transform destination,GameObject moveObj)
    {
        while (true)
        {

            yield return new WaitForFixedUpdate();

            moveObj.transform.position = Vector3.Lerp(moveObj.transform.position, destination.position, Time.deltaTime);

            //float desY = destination.position.y - transform.position.y;
            //float desZ = destination.position.z - transform.position.z;
            //Vector3 moveDes = new Vector3(0, desY, desZ);
            //this.transform.Translate(moveDes * Time.deltaTime, Space.World);
            if (CheckDestinationPos(destination,moveObj))
            {

                if (destination.gameObject == _packingZoneParent) 
                {
                    _isActiveMachine = false;
                }
                else
                {
                    EventManger.Instance.OnPackingEvent();
                }
                yield break;
            }
        }
    }

    private void ReversMovePackingZone()
    {
        StartCoroutine(MovePackingZone(_packingZoneParent.transform, _packingZone));
    }

    private bool CheckDestinationPos(Transform destination, GameObject moveObj)
    {
        if(destination.gameObject != _packingZoneParent)
        {
            if (destination.localPosition.y - moveObj.transform.localPosition.y >= 0.45f)
            {
                return true;
            }
        }
        else
        {
            if (destination.localPosition.y + moveObj.transform.localPosition.y >= 0.7f)
            {
                return true;
            }
        }
        

        return false;
    }
}
