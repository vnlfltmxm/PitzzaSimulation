using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZone : MonoBehaviour
{
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision != null)
    //    {
    //        PoolManger.Instance.InPoolItem(collision.gameObject, collision.gameObject);
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            PoolManger.Instance.InPoolItem(other.gameObject, other.gameObject);
        }
    }
}
