using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshManger : Singleton<NavmeshManger>
{
    [SerializeField]
    private GameObject _npcRespawonPos;
    [SerializeField]
    private GameObject _npcDestinationPos;

    public Vector3 GetRespawnPos()
    {
        return _npcRespawonPos.transform.position;
    }

    public Vector3 GetDestinationPos()
    {
        return _npcDestinationPos.transform.position;
    }
}
