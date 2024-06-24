using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    protected virtual void OnDisable()
    {
        if (this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Pizza") ||
            this.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            var parent = InteractionObjectManger.Instance.FindPrefabsParentTrasnform(this.gameObject.tag);

            //this.gameObject.transform.parent = null;
            this.gameObject.SetActive(true);
            this.gameObject.transform.parent = parent;
            this.gameObject.SetActive(false);
            this.gameObject.layer = LayerMask.NameToLayer("Item");
        }
        
    }
}
