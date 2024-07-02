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

    public void InitShopItemUI(string name, int valueSet, int price)
    {
        _nameText.text = name;
        _valueSetText.text = valueSet.ToString();
        _priceText.text = price.ToString();
        _buyText.text = _buyValue.ToString();
    }

    public void OnClickMinuseButton()
    {
        _buyValue++;
        _buyText.text = _buyValue.ToString();
    }

    public void OnClickPlusButton()
    {
        if( _buyValue <= 0 )
        {
            return;
        }
        _buyValue--;
        _buyText.text = _buyValue.ToString();
    }
}
