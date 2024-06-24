using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dough : Food
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
            _isMoveReady = true;
            ResterMoveDoughEvent();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.GetComponentInParent<MachineBase>() != null)
        {
            var machine = collision.transform.GetComponentInParent<MachineBase>();

            if (machine._isTurnOnMachine)
            {
                switch (machine._machineType)
                {
                    case "Kneader":
                        EventManger.Instance.OnDoughMoveEvent();
                        break;
                }
            }

            
        }
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
        _isMoveReady = false;
        _doughCount = 0;
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
        EventManger.Instance.DoughMove += StartMove;
    }
    private void UnResterMoveDoughEvent()
    {
        EventManger.Instance.DoughMove -= StartMove;
    }

    private void StartMove(Transform destination)
    {
        if (_isMoveReady)
        {
            _isMoveReady = false;
            StartCoroutine(MoveDough(destination));
        }
    }

    public IEnumerator MoveDough(Transform destination)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        while (true)
        {

            yield return new WaitForFixedUpdate();


            float y = destination.position.y;
            float z = destination.position.z;

            Vector3 test = new Vector3(this.transform.position.x, y, z);

            this.transform.position = Vector3.Lerp(transform.position, test, Time.deltaTime);

            //float desY = destination.position.y - transform.position.y;
            //float desZ = destination.position.z - transform.position.z;
            //Vector3 moveDes = new Vector3(0, desY, desZ);
            //this.transform.Translate(moveDes * Time.deltaTime, Space.World);

            RotateDough(destination);
            if (CheckDestinationPos(destination))
            {
                this.gameObject.layer = LayerMask.NameToLayer("Pizza");
                UnResterMoveDoughEvent();
                yield break;
            }
        }
    }

    private void RotateDough(Transform destination)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = destination.rotation;

        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime);
    }

    private bool CheckDestinationPos(Transform destination)
    {
        if (destination.position.y - this.transform.position.y <= 0.1f &&
            destination.position.z - this.transform.position.z <= 0.1f)
        {
            return true;
        }

        return false;
    }

}
