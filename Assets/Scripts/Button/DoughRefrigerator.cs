using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughRefrigerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] _dough;

    public void ViewDough(int value)
    {
        if (value < _dough.Length) 
        {
            if (_dough[value].gameObject.activeSelf == true)
            {
                _dough[value].gameObject.SetActive(false);
            }
            else
            {
                _dough[value].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < _dough.Length; i++)
            {
                if(_dough[i].gameObject.activeSelf == false)
                {
                    _dough[i].gameObject.SetActive(true);
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
