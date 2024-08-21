using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaHouseController : MonoBehaviour
{
    [SerializeField]
    private GameObject _nightLight;
   
    // Start is called before the first frame update
    void Start()
    {
        EventManger.Instance.LightOn += LightOn;
        EventManger.Instance.LightOff += LightOff;
    }

    private void OnDestroy()
    {
        EventManger.Instance.LightOn -= LightOn;
        EventManger.Instance.LightOff -= LightOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LightOn()
    {
        _nightLight.SetActive(true);
    }

    private void LightOff()
    {
        _nightLight.SetActive(false);
    }
}
