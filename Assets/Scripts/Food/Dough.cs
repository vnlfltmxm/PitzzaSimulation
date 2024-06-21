using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    private int _doughCount = 0;
    private bool _isDoughReady = false;

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
        EventManger.Instance.HandKnead += OnPlusDoughCount;
    }
    private void UnResterHandKneadEvent()
    {
        EventManger.Instance.HandKnead -= OnPlusDoughCount;
        Debug.Log("¹ÝÁ× ÀÌº¥Æ® ÇØÁ¦");
    }
}
