using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : Singleton<UIManger>
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Text _interactionText;
    [SerializeField]
    private Image _crossHair;
    [SerializeField]
    private GameObject _textRoot;
    [SerializeField]
    private GameObject _PlayerUIRoot;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _buttonRoot;
    [SerializeField]
    private Text _moneytext;

    private void Awake()
    {
        InitUI();
        _interactionText.text = string.Empty;
    }

    private void InitUI()
    {
        SetTextBGActive(false);
        SetButtonActive(false);
    }
    private IEnumerator SetPrintText(string text, bool isNPCChangedLeave)
    {
        int index = 0;
        _text.text = string.Empty;
        while (index < text.Length)
        {
            _text.text += text[index];
            index++;
            yield return new WaitForSeconds(0.1f);
        }

        if (isNPCChangedLeave)
        {
            yield return new WaitForSeconds(1f);
            SetTextBGActive(false);
        }

    }
    public void SetButtonActive(bool active)
    {
        _buttonRoot.SetActive(active);
    }
    public void SetTextBGActive(bool active)
    {
        _textRoot.SetActive(active);
    }
    public void SetMoneyText(int value)
    {
        _moneytext.text = value.ToString();
    }
    public void PrintNPCText(string text, bool isNPCChangedLeave = false)
    {
        if (_textRoot.gameObject.activeSelf == false)
        {
            SetTextBGActive(true);
        }
        StartCoroutine(SetPrintText(text, isNPCChangedLeave));
    }
    public void PrintInteractionText(string text)
    {
        _interactionText.text = text;
    }

    public void OnAcceptButtonCleckEvent()
    {
        SetTextBGActive(false);
        SetButtonActive(false);
        EventManger.Instance.OnPlayerCurserLock();
    }
    public void OnRefuseButtonCleckEvent()
    {
        SetButtonActive(false);
        SetTextBGActive(false);
        EventManger.Instance.OnPlayerCurserLock();
        EventManger.Instance.OnChangeNPCStatetoLeave();
    }
    //private void OnRegisterNPCTalkEvent()
    //{
    //    EventManger.Instance.NPCTalk += PrintText;
    //}
    //private void OnUnRegisterNPCTalkEvent()
    //{
    //    EventManger.Instance.NPCTalk -= PrintText;
    //}

    //public void TransmissionInteractionText(RaycastHit hit, bool value)
    //{
    //    if (hit.transform != null)
    //    {
    //        var obj = hit.transform.gameObject.layer;

    //        switch (obj)
    //        {
    //            case 6:
    //                SetInteractionText("넣기", "꺼내기",  value);
    //                break;
    //            case 7:
    //                SetInteractionText("놓기", "줍기",  value);
    //                break;
    //            case 8:
    //                SetInteractionText("놓기", string.Empty,  value);
    //                break;
    //            case 9:
    //                SetItemInteractionText(hit.transform.gameObject,  value);
    //                break;
    //            case 10:
    //                SetInteractionText("누르기");
    //                break;
    //            default:
    //                break;
    //        }

    //    }
    //    else
    //    {
    //        SetInteractionText(string.Empty);
    //    }

    //}
    //private void SetItemInteractionText(GameObject obj,bool value)
    //{
    //    if (obj.CompareTag("Dough"))
    //    {
    //        if (EventManger.Instance.CheckRegisterDough(obj) == false)
    //        {

    //            SetInteractionText(string.Empty, "G : 줍기",  value);
    //        }
    //        else
    //        {
    //            SetInteractionText(string.Empty, "G : 줍기 Space : 반죽",  value);
    //        }
    //    }
    //    else
    //    {
    //        SetInteractionText(string.Empty, "줍기",  value);
    //    }
    //}
    //private void SetInteractionText(string text)
    //{
    //    PrintInteractionText(text);
    //}
    //private void SetInteractionText(string handlingText, string unHandlingText,bool value)
    //{
    //    if (value == true)
    //    {
    //        PrintInteractionText(handlingText);
    //    }
    //    else
    //    {
    //        PrintInteractionText(unHandlingText);
    //    }
    //}

}
