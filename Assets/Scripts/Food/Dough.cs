using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    private int _doughCount = 0;
    private bool _isDoughReady = false;


    private void OnEnable()
    {
        _isDoughReady = false;
        _doughCount = 0;
        this.gameObject.layer = LayerMask.NameToLayer("Item");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Kneader") && _isDoughReady == false) 
        {
            ResterHandKneadEvent();
        }
    }

    private void DoughReady()
    {
        _isDoughReady = true;
        this.gameObject.layer = LayerMask.NameToLayer("Pizza");
        UnResterHandKneadEvent();
    }

    private void OnPlusDoughCount(GameObject thisGameObj)
    {
        if(thisGameObj != this.gameObject)
        {
            Debug.Log("³ª¾Æ´Ô");
            return;
        }

        _doughCount++;
        Debug.Log($"{_doughCount}È¸ ¹ÝÁ×");
        if(_doughCount >= 10)
        {
            DoughReady();
            Debug.Log("¹ÝÁ× ²ý");
        }
    }

    private void ResterHandKneadEvent()
    {
        InteractionObjectManger.Instance.HandKnead += OnPlusDoughCount;
    }
    private void UnResterHandKneadEvent()
    {
        InteractionObjectManger.Instance.HandKnead -= OnPlusDoughCount;
        Debug.Log("¹ÝÁ× ÀÌº¥Æ® ÇØÁ¦");
    }
}
