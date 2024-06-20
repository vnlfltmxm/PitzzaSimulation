using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameObject _grabPos;
    private NavMeshAgent _nav;
    private float _moveSpeed = 3.0f;
    private float _rotateSpeed = 10.0f;
    private float _verticalLookSpeed = 2.0f; 
    private float _maxLookAngle = 80f; 
    private float _minLookAngle = -60f; 
    private float _verticalLookRotation = 0f; 

    private void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _nav.updateRotation = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
        RotateWithMouse();
        RayToCameraFoward();
        PressButtonG();
    }

    private bool CheckKey()
    {
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveToPlayer()
    {
        if (CheckKey())
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            Vector3 forward = transform.TransformDirection(direction);
            _nav.velocity = forward * _moveSpeed;

        }
        else
        {
            _nav.velocity = Vector3.zero;
        }


    }
    private void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0, mouseX * _rotateSpeed, 0);

        transform.Rotate(rotation);

        RotationCamera();
    }

    private void RotationCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        _verticalLookRotation -= mouseY * _verticalLookSpeed;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, _minLookAngle, _maxLookAngle);
        _camera.transform.localEulerAngles = new Vector3(_verticalLookRotation, 0, 0);
    }

    private void RayToCameraFoward()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 50, Color.red);
    }

    private void PressButtonG()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 50, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                var obj = hit.transform.gameObject.layer;
                
                switch (obj)
                {
                    case 6:
                        PickUpItemToPool(hit.transform.gameObject);
                        break;
                    case 7:
                        PickUpItem(hit.transform.gameObject);
                        break;
                    case 8:
                        DropPizzaToMachine(hit.transform.gameObject, hit);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                DropItem();
            }
        }
    }

    private bool CheckOnHandlingItem()
    {
        if (_grabPos.transform.childCount > 0)
        {
            return true;
        }

        return false;
    }
    private void PickUpItem(GameObject obj)
    {
        if (obj.transform.gameObject != null)
        {
            if (CheckOnHandlingItem())
            {
                return;
            }
            else
            {
                InteractionObjectManger.Instance.OnPickUpItem(obj, _grabPos);
            }
            
        }
    }
    private void DropPizzaToMachine(GameObject machine, RaycastHit RayHitPos)
    {
        Vector3 pointPos = RayHitPos.normal;

        var obj = _grabPos.transform.GetChild(0).gameObject;
        float dotProduct = Vector3.Dot(pointPos, machine.transform.up);

        if (CheckOnHandlingItem() &&
            dotProduct > 0.9f &&
            obj.layer == LayerMask.NameToLayer("Pizza"))  
        {
            InteractionObjectManger.Instance.OnDropItemToMachine(obj, machine, RayHitPos.point);
        }
    }
    private void DropItem()
    {
        if (CheckOnHandlingItem())
        {
            InteractionObjectManger.Instance.OnDropItem(_grabPos.transform.GetChild(0).gameObject);
        }
    }
    private void PickUpItemToPool(GameObject obj)
    {
        if (obj.transform.gameObject != null)
        {
            if (CheckOnHandlingItem()) 
            {
               RetrunHandlingItem(obj);
            }
            else
            {
                InteractionObjectManger.Instance.OnPickUpItemToPool(obj, _grabPos);
            }
        }

    }

    private void RetrunHandlingItem(GameObject rayHitObj)
    {
        var grapItem = _grabPos.transform.GetChild(0).gameObject;
        if (rayHitObj.CompareTag(grapItem.tag))
        {
            InteractionObjectManger.Instance.OnReturnHandlingItemToPool(rayHitObj, grapItem);
        }
        else
        {
            return;
        }
    }


}
