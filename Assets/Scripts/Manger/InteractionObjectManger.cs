using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionObjectManger : Singleton<InteractionObjectManger>
{
    [SerializeField]
    private GameObject _pizzaHouse;
    public GameObject _doughtRefrigerator;


    public Transform FindPrefabsParentTrasnform(string tagName)
    {
        foreach (Transform child in _pizzaHouse.transform)
        {
            if (child.CompareTag(tagName))
            {
                return child;
            }
        }
        return null;
    }
}
