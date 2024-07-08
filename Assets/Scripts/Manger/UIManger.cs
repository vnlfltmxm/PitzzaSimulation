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
    private GameObject _shopUIRoot;
    [SerializeField]
    private GameObject _PlayerCoinUIRoot;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _buttonRoot;
    [SerializeField]
    private Text _moneytext;
    [SerializeField]
    private Text _shopPlayerMoneytext;
    [SerializeField]
    private Text _shopResultMoneytext;
    [SerializeField]
    private GameObject _shopItemUIPrefabs;
    [SerializeField]
    private GameObject _cotent;
    [SerializeField]
    private GameObject _scrollView;
    [SerializeField]
    private GameObject _shopMenuUI;
    [SerializeField]
    private GameObject _recipeUIRoot;
    [SerializeField]
    private GameObject _recipeCotent;
    [SerializeField]
    private GameObject _recipeUIPrefab;
    [SerializeField]
    private Text _hourTxt;
    [SerializeField]
    private Text _minuitTxt;
    [SerializeField]
    private Image _curtainImage;
    [SerializeField]
    private GameObject _resultUI;
    private void Awake()
    {
        InitUI();
        _interactionText.text = string.Empty;
        RegisterClickButtonEvent();
        RegisterDayEvent();
    }
    private void Start()
    {
        InitShopUI();
        InitRecipeUI();
        StartCoroutine(OutCurtain());
    }
    private void OnDisable()
    {
        UnRegisterClickButtonEvent();
        UnRegisterDayEvent();
    }
    private void InitUI()
    {
        SetTextBGActive(false);
        SetButtonActive(false);
        SetResultUIActive(false);
    }
    private void InitShopUI()
    {
        var toppingList = DataManger.Inst.LoadedToppingResorceList;
        if(toppingList == null)
        {
            return;
        }

        foreach(var item in toppingList.Keys)
        {
            GameObject itemUI = Instantiate(_shopItemUIPrefabs,_cotent.transform);
            var shopItemUI = itemUI.GetComponent<ShopItemUI>();
            if ( shopItemUI == null)
            {
                Destroy(itemUI);
                return;
            }
            var toppingItem = toppingList[item];
            shopItemUI.InitShopItemUI(toppingItem.ItemName,toppingItem.Name, toppingItem.MinBuyValues, toppingItem.Price * toppingItem.MinBuyValues);


        }
        SetPlayerShopMoneyText(DataManger.Inst.GetplayerData("플레이어").StartMoney);
        SetResultShopMoneyText(DataManger.Inst.GetplayerData("플레이어").StartMoney);
        _scrollView.SetActive(false);
        _shopUIRoot.SetActive(false );
    }
    private void InitRecipeUI()
    {
        var recipeBook = DataManger.Inst.LoadedPizzaList;
        if(recipeBook == null)
        {
            return;
        }

        foreach (var item in recipeBook.Keys)
        {
            GameObject recipe = Instantiate(_recipeUIPrefab, _recipeCotent.transform);
            var recipeUIData = recipe.GetComponent<SlotPizzaRecipe>();
            if(recipeUIData == null)
            {
                Destroy(recipe);
                return;
            }

            recipeUIData.SetPizzaRecipeSlot(item);
            recipe.SetActive(false);
            recipe.SetActive(true);
        }

        SetRecipeUIRoot(false);
    }

    public void SetRecipeUIRoot(bool value)
    {
        _recipeUIRoot.SetActive(value);
    }
    private void RegisterClickButtonEvent()
    {
        EventManger.Instance.ClickPlusButton += OnClickPlusButton;
        EventManger.Instance.ClickMinuseButton += OnClickMinuseButton;
    }
    private void UnRegisterClickButtonEvent()
    {
        EventManger.Instance.ClickPlusButton -= OnClickPlusButton;
        EventManger.Instance.ClickMinuseButton -= OnClickMinuseButton;
    }
    private void OnClickPlusButton(string toppingName)
    {
        int resultMoney = int.Parse(_shopResultMoneytext.text);

        var toppingItem = DataManger.Inst.GetToppingResorceData(toppingName);
        SetResultShopMoneyText(resultMoney - (toppingItem.MinBuyValues * toppingItem.Price));
    }
    private void OnClickMinuseButton(string toppingName)
    {
        int resultMoney = int.Parse(_shopResultMoneytext.text);

        var toppingItem = DataManger.Inst.GetToppingResorceData(toppingName);
        SetResultShopMoneyText(resultMoney + (toppingItem.MinBuyValues * toppingItem.Price));
    }
    public void SetPlayerShopMoneyText(int value)
    {
        _shopPlayerMoneytext.text = value.ToString();
    }

    public void SetResultShopMoneyText(int value)
    {
        _shopResultMoneytext.text = value.ToString();
    }


    private IEnumerator SetPrintText(string text, bool isNPCChangedLeave)
    {
        int index = 0;
        _text.text = string.Empty;
        while (index < text.Length)
        {
            _text.text += text[index];
            index++;
            yield return new WaitForSeconds(0.05f);
        }

        if (isNPCChangedLeave)
        {
            yield return new WaitForSeconds(1f);
            SetTextBGActive(false);
        }

    }
    public void SetResultUIActive(bool active)
    {
        _resultUI.SetActive(active);
    }
    private void SetTimeTxt(Text text,int value)
    {
        if (value < 10)
        {
            text.text = $"0{value}";
        }
        else
        {
            text.text = value.ToString();
        }
    }
    public void SetHourTimeText(int hour)
    {
       SetTimeTxt(_hourTxt, hour);
    }
    public void SetMinuteTimeText(int minute)
    {
        SetTimeTxt(_minuitTxt, minute);
    }
    public void SetMinuteTimeText(string minute)
    {
        _minuitTxt.text = minute;
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
        if(_interactionText.text == text)
        {
            return;
        }
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
    public void OnShopMenuOpen()
    {
        _crossHair.enabled = false;
        PrintInteractionText(string.Empty);
        _shopUIRoot.SetActive(true);
        _PlayerCoinUIRoot.SetActive(false);
    }

    public bool CheckOpenShopNenu()
    {
        if (_shopUIRoot.activeSelf == false)
        {
            return true;
        }

        return false;
    }

    public void OnClickShopOpenButton()
    {
        _shopMenuUI.SetActive(false);
        _scrollView.SetActive(true);
    }
    public void On_ClickRecipeOpenButton()
    {
        _shopMenuUI.SetActive(false);
        _recipeUIRoot.SetActive(true);
        
    }
    public void OnClickCloseButton()
    {
        if( _scrollView.activeSelf == true)
        {
            _scrollView.SetActive(false);
            _shopMenuUI.SetActive(true);
        }
        else if( _shopMenuUI.activeSelf == true)
        {
            _shopUIRoot.SetActive(false);
            _crossHair.enabled = true;
            _PlayerCoinUIRoot.SetActive(true);
            EventManger.Instance.OnPlayerCurserLock();
        }else if(_recipeUIRoot.activeSelf == true)
        {
            _recipeUIRoot.SetActive(false);
            _shopMenuUI.SetActive(true);
        }
    }

    private IEnumerator OnCurtain()
    {
        float alphaValue = 0;
        while(_curtainImage.color.a <= 1)
        {
            yield return new WaitForSeconds(0.1f);

            _curtainImage.color = new Color(0, 0, 0, alphaValue);
            alphaValue += 0.1f;
        }

        SetResultUIActive(true);
    }
    private IEnumerator OutCurtain()
    {
        float alphaValue = 1;
        while (_curtainImage.color.a >= 0)
        {
            yield return new WaitForSeconds(0.1f);

            _curtainImage.color = new Color(0, 0, 0, alphaValue);
            alphaValue -= 0.1f;
        }
        EventManger.Instance.OnPlayerCurserLock();
    }
    private void DayGone()
    {
        StartCoroutine(OnCurtain());
        if(_textRoot.activeSelf == true)
        {
            _textRoot.SetActive(false);
        }

        if(_buttonRoot.activeSelf == true)
        {
            _buttonRoot.SetActive(false);
        }
    }

    private void DayStart()
    {
        StartCoroutine(OutCurtain());
        SetPlayerShopMoneyText(PlayerController.Instance.PlayerMoney);
        SetMoneyText(PlayerController.Instance.PlayerMoney);
    }

    private void OpenResultUI()
    {

    }
    private void RegisterDayEvent()
    {
        EventManger.Instance.DayGone += DayGone;
        EventManger.Instance.DayStart += DayStart;
    }
    private void UnRegisterDayEvent()
    {
        EventManger.Instance.DayGone -= DayGone;
        EventManger.Instance.DayStart -= DayStart;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
