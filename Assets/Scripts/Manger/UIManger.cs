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
    private Image _crossHair;
    [SerializeField]
    private GameObject _textRoot;
    [SerializeField]
    private GameObject _PlayerUIRoot;
    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
        InitUI();
    }

    private void InitUI()
    {
        SetTextBGActive(false);
    }
    private IEnumerator SetPrintText(string text)
    {
        int index = 0;
        _text.text = string.Empty;
        while (index < text.Length)
        {
            _text.text += text[index];
            index++;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void SetTextBGActive(bool active)
    {
        _textRoot.SetActive(active);
    }
    public void PrintText(string text)
    {
        SetTextBGActive(true);
        StartCoroutine(SetPrintText(text));
    }

    //private void OnRegisterNPCTalkEvent()
    //{
    //    EventManger.Instance.NPCTalk += PrintText;
    //}
    //private void OnUnRegisterNPCTalkEvent()
    //{
    //    EventManger.Instance.NPCTalk -= PrintText;
    //}

}
