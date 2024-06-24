using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ItemName
{
    Dough
}

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
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 50, ~(1 << LayerMask.NameToLayer("Ignore Raycast")));

        if (Input.GetKeyDown(KeyCode.G))
        {
            PressButtonG(hit);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            PressButtonSpace(hit);
        }
    }

    private void PressButtonG(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            var obj = hit.transform.gameObject.layer;

            switch (obj)
            {
                case 6:
                    PickUpItemToPool(hit.transform.gameObject);
                    break;
                case 7:
                    //ToppingPizza(hit.transform.gameObject, hit);
                    //PickUpItem(hit.transform.gameObject);
                    PizzaActive(hit.transform.gameObject, hit);
                    break;
                case 8:
                    DropPizzaToMachine(hit.transform.gameObject, hit);
                    break;
                case 9:
                    PickUpItem(hit.transform.gameObject);
                    break;
                case 10:
                    PushMachineButton(hit.transform.gameObject);
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

    private void PressButtonSpace(RaycastHit hit)
    {
        if (hit.transform == null)
        {
            return;
        }
        if (CheckOnHandlingItem()) 
        {
            return;
        }

        GameObject obj = hit.transform.gameObject;

        if (CheckItemLayer(obj, "Item") && CheckItemTag(obj, "Dough")) 
        {
            EventManger.Instance.OnInvokeHandKneadEvent(hit.transform.gameObject);
        }
    }
    private bool CheckItemTag(GameObject item,string tagName)
    {
        if (item.CompareTag(tagName))
        {
            return true;
        }

        return false;
    }
    private bool CheckItemLayer(GameObject item,string layerName)
    {
        if (item.layer == LayerMask.NameToLayer(layerName))
        {
            return true;
        }

        return false;
    }
    private void PizzaActive(GameObject pizza, RaycastHit hitRay)
    {
        if (pizza.transform.gameObject != null)
        {
            if (CheckOnHandlingItem())
            {
                ToppingPizza(pizza, hitRay);
            }
            else
            {
                PickUpItem(pizza);
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
        if(CheckOnHandlingItem()==false ) 
        {
            return;
        }
        var obj = _grabPos.transform.GetChild(0).gameObject;
        float dotProduct = Vector3.Dot(pointPos, machine.transform.up);

        if (dotProduct > 0.9f &&
            obj.CompareTag("Dough"))  
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

    private void ToppingPizza(GameObject pizza,RaycastHit hitRay)
    {
        var pizzaItem = _grabPos.transform.GetChild(0).gameObject;

        if (pizzaItem.CompareTag("Dough"))
        {
            return;
        }

        InteractionObjectManger.Instance.OnToppingPizza(pizzaItem, pizza, hitRay.point);
    }


    private void PushMachineButton(GameObject obj)
    {
        var MachineButton = obj.GetComponentInParent<MachineBase>();

        if(MachineButton != null)
        {
            EventManger.Instance.OnMachineActivateEvent(MachineButton.gameObject);
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
