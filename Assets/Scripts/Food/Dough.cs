using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Dough : Food
{
    private int _doughCount = 0;
    private bool _isDoughReady = false;
    private bool _isMoveReady = false;
    private bool _isDoughCooked = false;
    private bool _isDoughOverCooked = false;
    private float _doughCookedTime;
    private float _doughKneadCount = 0;
    private bool _isPizzaPackaging = false;
    [SerializeField]
    private GameObject _meitingCheese;
    [SerializeField]
    private Mesh[] _meshs;

    protected MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    public bool _isPizzaCooked { get { return _isDoughCooked; } }

    protected override void Awake()
    {
        base.Awake();
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    protected override void OnEnable()
    {
        ChangeMaterialColor(Color.white, null);
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

        if (collision.transform.CompareTag("KneaderInputPos") )
        {
            _isMoveReady = true;
            ResterMoveDoughEvent();
        }

        if (collision.transform.GetComponentInParent<MachineBase>() != null)
        {
            var machine = collision.transform.GetComponentInParent<MachineBase>();
            switch (machine._machineType)
            {
                case "PackagingMachine":
                    EventManger.Instance.OnRegisterPackingEvent(PackagingDough);
                    break;
            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.transform.GetComponentInParent<MachineBase>() != null)
        {
            var machine = collision.transform.GetComponentInParent<MachineBase>();

            if (machine._isTurnOnMachine)
            {
                switch (machine._machineType)
                {
                    case "Kneader":
                        EventManger.Instance.OnDoughMoveEvent();
                        break;
                    case "Oven":
                        DoughCooked();
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

    private void DoughCooked()
    {
       
        if (_isDoughOverCooked == false)
        {
            _doughCookedTime += Time.deltaTime;
            
            if (_isDoughCooked == false && _doughCookedTime >= 10.0f)
            {
                //마테리얼 변경
                Color cookedColor = new Color(1, 0.68f, 0.28f, 1);
                base.ChangeMaterialColor(cookedColor);
                Melting();
                _isDoughCooked = true;
            }

            if (_doughCookedTime >= 15.0f)
            {
                //마테리얼 변경
                EventManger.Instance.OnOverCookedEvent(this.gameObject);
                base.ChangeMaterialColor(Color.black);
                _isDoughOverCooked = true;
                if (_meitingCheese.activeSelf == true)
                {
                    ChangeCheeseColor(Color.black);
                }
            }
        }
    }
    
    private void Melting()
    {
        if (CheckObj(this.gameObject,"Cheese"))
        {
            _meitingCheese.SetActive(true);
            PoolManger.Instance.ReturnItemInPool(this.gameObject, "Cheese", true);
            ChangeCheeseColor(new Color(1, 0.86f, 0, 1));

            if (CheckObj(this.gameObject, "Sauce"))
            {
                PoolManger.Instance.ReturnItemInPool(this.gameObject, "Sauce", true);
            }
        }

        
    }
    private void ChangeCheeseColor(Color color)
    {
        _meitingCheese.GetComponent<MeshRenderer>().material.color = color;
    }
    private bool CheckObj(GameObject item, string tagName)
    {
        if (item.transform.childCount > 0)
        {
            for (int i = item.transform.childCount - 1; i >= 0; i--)
            {
                var child = item.transform.GetChild(i);
                if (child.CompareTag(tagName))
                {
                    return true;
                }
                CheckObj(child.gameObject, tagName);
            }
        }
        return false;
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
        _doughCookedTime = 0;
        _isDoughCooked = false;
        _isDoughOverCooked = false;
        _doughKneadCount = 0;
        _meshRenderer.enabled = true;
        _isPizzaPackaging = false;
        DisableBox();
        ChangeMesh(_meshs[0]);
        SetDoughScale(_doughKneadCount);
    }
    private void DisableBox()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
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
        if (_isPizzaPackaging)
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
    private void SetDoughScale(float kneadCount)
    {
        Vector3 startScale = new Vector3(1, 1, 1);
        gameObject.transform.localScale = startScale + (startScale * (kneadCount * 0.1f));
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
                if (_isPizzaPackaging || !_isDoughReady) 
                {
                    yield break;
                }
                _doughKneadCount++;
                SetDoughScale(_doughKneadCount);
               
                yield break;
            }
        }
    }
    private void PackagingDough()
    {
        SetDoughScale(0);
        _meshRenderer.enabled = false;
        if (_doughKneadCount < 5) 
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        _isPizzaPackaging = true;
        _meitingCheese.SetActive(false);
        PoolManger.Instance.ReturnItemInPool(this.gameObject,"Dough",false);
        EventManger.Instance.OnUnRegisterPackingEvent(PackagingDough);
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
