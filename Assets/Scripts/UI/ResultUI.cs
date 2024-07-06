using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _Txt;
    [SerializeField] 
    private Text[] _valueTxt;
    [SerializeField]
    private GameObject _buttonSlot;
    private void OnEnable()
    {
        StartCoroutine(EnableResultUI());
    }

    private void OnDisable()
    {
        
    }

    private IEnumerator EnableResultUI()
    {
        InitTxt();
        _buttonSlot.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _Txt.Length; i++)
        {
            _Txt[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            SetValueTxt(i);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        _buttonSlot.SetActive(true);


    }

    private void InitTxt()
    {
        for (int i = 0; i < _Txt.Length; i++)
        {
            _valueTxt[i].text = string.Empty;
            _Txt[i].SetActive(false);
        }
    }
    private void SetValueTxt(int index)
    {
        switch (index)
        {
            case 0:
                _valueTxt[index].text = $"{GameManger.Instance.Revenu} ¿ø ";
                break;
            case 1:
                _valueTxt[index].text = $"{ShopManger.Instance.Pay} ¿ø ";
                break;
            case 2:
                _valueTxt[index].text = $"{GameManger.Instance.Revenu - ShopManger.Instance.Pay} ¿ø ";
                break;
            default:
                break;
        }
    }
    public void OnClickContinueButton()
    {
        EventManger.Instance.OnDayStartEventInvoke();
        this.gameObject.SetActive(false);
    }
}
