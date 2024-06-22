using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    private int _doughCount = 0;
    private bool _isDoughReady = false;
    private bool _isMoveReady = false;

    [SerializeField]
    private Mesh[] _meshs;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
       InitDough();
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

        if (collision.transform.CompareTag("KneaderInputPos") && _isDoughReady == true)
        {
            ResterHandKneadEvent();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("KneaderInputPos") && _isDoughReady == true)
        {
            ResterHandKneadEvent();
        }
    }


    private void ChangeMesh(Mesh mesh)
    {
        _meshFilter.mesh = mesh;
        ChangeMeshCollider(mesh);
    }

    private void ChangeMeshCollider(Mesh mesh)
    {
        _meshCollider.sharedMesh = null;
        _meshCollider.sharedMesh = mesh;
    }

    private void InitDough()
    {
        _isDoughReady = false;
        _doughCount = 0;
        this.gameObject.layer = LayerMask.NameToLayer("Item");
        ChangeMesh(_meshs[0]);
    }

    private void DoughReady()
    {
        _isDoughReady = true;
        this.gameObject.layer = LayerMask.NameToLayer("Pizza");
        ChangeMesh(_meshs[1]);
        UnResterHandKneadEvent();
    }
    
    private void OnPlusDoughCount(GameObject thisGameObj)
    {
        if (EventManger.Instance.CheckEventTarget(thisGameObj, this.gameObject) == false)
        {
            return;
        }

        _doughCount++;

        if(_doughCount >= 10)
        {
            DoughReady();
        }
    }

    private void ResterHandKneadEvent()
    {
        EventManger.Instance.HandKnead += OnPlusDoughCount;
    }
    private void UnResterHandKneadEvent()
    {
        EventManger.Instance.HandKnead -= OnPlusDoughCount;
    }
    private void ResterMoveDoughEvent()
    {
        EventManger.Instance.HandKnead += OnPlusDoughCount;
    }
    private void UnResterMoveDoughEvent()
    {
        EventManger.Instance.HandKnead -= OnPlusDoughCount;
    }

    public IEnumerator MoveDough(Transform destination)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        while (true)
        {

            yield return new WaitForSeconds(0.5f);

            float desY = destination.position.y - transform.position.y;
            float desZ = destination.position.z - transform.position.z;

            Vector3 moveDes = new Vector3(0, desY, desZ);

            this.transform.Translate(moveDes);



            if (CheckDestinationPos(destination))
            {
                this.gameObject.layer = LayerMask.NameToLayer("Pizza");
                yield break;
            }
        }
    }

    private bool CheckDestinationPos(Transform destination)
    {
        if (destination.position.y - this.transform.position.y <= 1 &&
            destination.position.z - this.transform.position.z <= 1)
        {
            return true;
        }

        return false;
    }

}
