using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManger : Singleton<ShopManger>
{
    private Dictionary<string, int> _shoppingDic = new Dictionary<string, int>();
    private Dictionary<string, ToppingResorce> _toppingItemDic = new Dictionary<string, ToppingResorce>();

    private void Start()
    {
        if (DataManger.Inst.LoadedToppingResorceList != null)
        {
            _toppingItemDic = DataManger.Inst.LoadedToppingResorceList;
        }
    }

    public bool CheckPlayerMoney()
    {
        int playerMoney = PlayerController.Instance.PlayerMoney;

        if (playerMoney <= 0)
        {
            return false;
        }

        if(_shoppingDic.Count > 0)
        {
            int resultMoney = 0;
            
            foreach (var item in _shoppingDic.Keys)
            {
                resultMoney += _toppingItemDic[item].Price * _toppingItemDic[item].MinBuyValues; 
            }

            if(playerMoney - resultMoney < 0)
            {
                return false;
            }
        }

        return true;
    }

}
