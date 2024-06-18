using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField]
    private Camera _camera;
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
        _camera.transform.localEulerAngles = new Vector3(_verticalLookRotation, _camera.transform.localEulerAngles.y, 0);
    }

    private void RayToCameraFoward()
    {
        if(Physics.Raycast(_camera.transform.position,_camera.transform.forward,out RaycastHit hit, 50))
        {

            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 50, Color.red);
        }
    }

}
