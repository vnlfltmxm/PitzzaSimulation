using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _valueSetText;
    [SerializeField]
    private Text _priceText;
    [SerializeField]
    private Text _buyText;

    private int _buyValue = 0;

    private string _toppingName;

    public void InitShopItemUI(string itemName,string toppingName, int valueSet, int price)
    {
        _nameText.text = itemName;
        _toppingName = toppingName;
        _valueSetText.text = valueSet.ToString();
        _priceText.text = price.ToString();
        _buyText.text = _buyValue.ToString();
    }

    public void On_ClickPlusButton()
    {
        if (ShopManger.Instance.CheckPlayerMoney(_toppingName) == false) 
        {
            return;
        }
        _buyValue++;
        _buyText.text = _buyValue.ToString();
        EventManger.Instance.OnClickPlusButtonEvent(_toppingName);
    }

    public void On_ClickMinuseButton()
    {
        if( _buyValue <= 0 )
        {
            return;
        }
        _buyValue--;
        _buyText.text = _buyValue.ToString();
        EventManger.Instance.OnClickMinuseButtonEvent(_toppingName);
    }
}
