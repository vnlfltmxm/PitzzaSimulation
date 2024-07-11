using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManger : Singleton<ShopManger>
{
    private Dictionary<string, int> _shoppingDic = new Dictionary<string, int>();//쇼핑한 아니템의 개수가 들어감
    private Dictionary<string, ToppingResorce> _toppingItemDic = new Dictionary<string, ToppingResorce>();

    private int _resutMoney;

    public int Pay { get { return _resutMoney; } }
    private void Awake()
    {
        RegisterDayEvent();
    }
    private void Start()
    {
        RegisterButtonEvent();
        if (DataManger.Inst.LoadedToppingResorceList != null)
        {
            _toppingItemDic = DataManger.Inst.LoadedToppingResorceList;
        }
    }
    private void OnDisable()
    {
        UnRegisterButtonEvent();
        UnRegisterDayEvent();
    }
    public int GetShopingValue(string itemName)
    {
        if (_shoppingDic.ContainsKey(itemName) == false) 
        {
            return 0;
        }

        return _shoppingDic[itemName];
    }
    public bool CheckPlayerMoney(string itemName)
    {
        float playerMoney = PlayerController.Instance.PlayerMoney;

        if (playerMoney <= 0)
        {
            return false;
        }

        if(_shoppingDic.Count > 0)
        {
            int resultMoney = 0;
            
            foreach (var item in _shoppingDic.Keys)
            {
                resultMoney += _toppingItemDic[item].Price * _toppingItemDic[item].MinBuyValues * (_shoppingDic[item] / DataManger.Inst.GetToppingResorceData(item).MinBuyValues);
            }

            resultMoney += _toppingItemDic[itemName].Price * _toppingItemDic[itemName].MinBuyValues;

            if (playerMoney - resultMoney < 0)
            {
                return false;
            }
        }

        return true;
    }

    private void RegisterButtonEvent()
    {
        EventManger.Instance.ClickPlusButton += BuyToppingItem;
        EventManger.Instance.ClickMinuseButton += CancelToppingItem;
        
    }
    private void UnRegisterButtonEvent()
    {
        EventManger.Instance.ClickPlusButton += BuyToppingItem;
        EventManger.Instance.ClickMinuseButton += CancelToppingItem;

    }

    public void BuyToppingItem(string toppingName)
    {
        if (_shoppingDic.ContainsKey(toppingName) == false) 
        {
            _shoppingDic.Add(toppingName, DataManger.Inst.GetToppingResorceData(toppingName).MinBuyValues);
        }
        else
        {
            _shoppingDic[toppingName] += DataManger.Inst.GetToppingResorceData(toppingName).MinBuyValues;
        }
    }
    public void CancelToppingItem(string toppingName)
    {
        if (_shoppingDic.ContainsKey(toppingName) == false)
        {
            return;
        }
        else
        {
            _shoppingDic[toppingName] -= DataManger.Inst.GetToppingResorceData(toppingName).MinBuyValues;
        }
    }
    public void ResetShoppingDic()
    {
        _shoppingDic.Clear();
    }
    private void DayGone()
    {
        _resutMoney = 0;

        if (_shoppingDic.Count > 0)
        {

            foreach (var item in _shoppingDic.Keys)
            {
                _resutMoney += _toppingItemDic[item].Price * _toppingItemDic[item].MinBuyValues * (_shoppingDic[item] / DataManger.Inst.GetToppingResorceData(item).MinBuyValues);
                PlayerController.Instance.PlusToppingList(item);
            }
        }

        PlayerController.Instance.MinuseMoney(_resutMoney);
    }
    private void RegisterDayEvent()
    {
        EventManger.Instance.DayGone += DayGone;
        EventManger.Instance.DayStart += ResetShoppingDic;
    }
    private void UnRegisterDayEvent()
    {
        EventManger.Instance.DayGone -= DayGone;
        EventManger.Instance.DayStart -= ResetShoppingDic;
    }
    //private void RegisterDayStartEvent()
    //{
    //    EventManger.Instance.DayStart += ResetShoppingDic;
    //}
    //private void UnRegisterDayStartEvent()
    //{
    //    EventManger.Instance.DayStart -= ResetShoppingDic;
    //}
}
