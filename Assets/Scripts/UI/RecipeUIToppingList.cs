using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIToppingList : MonoBehaviour
{
    [SerializeField]
    private Text _toppingResorceNameTxt;
    [SerializeField]       
    private Text _toppingResorceRagulerValueTxt;
    [SerializeField]       
    private Text _toppingResorceLargeValueTxt;

    public void SetRecipeUIToppingList(string toppingResorceName, int pizzaBaseSizeToppingValue, int pizzaLargeSizeToppingValue)
    {
        _toppingResorceNameTxt.text = toppingResorceName;
        _toppingResorceRagulerValueTxt.text = pizzaBaseSizeToppingValue.ToString() + "°³";
        _toppingResorceLargeValueTxt.text = pizzaLargeSizeToppingValue.ToString() + "°³";
    }
}
