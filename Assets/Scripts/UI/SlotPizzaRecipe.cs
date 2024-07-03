using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotPizzaRecipe : MonoBehaviour
{
    [SerializeField]
    private Text _pizzaName;
    [SerializeField]
    private GameObject _content;
    [SerializeField]
    private GameObject _toppingResorceListPrefab;

    public void SetPizzaRecipeSlot(string pizzaName)
    {
        var pizzaData = DataManger.Inst.GetPizzaData(pizzaName);
        if(pizzaData == null )
        {
            return;
        }
        _pizzaName.text = pizzaData.Name;

        int index = 0;

        foreach (var item in pizzaData.ToppingResorceList)
        {
            GameObject list = Instantiate(_toppingResorceListPrefab, _content.transform);
            var listData = list.GetComponent<RecipeUIToppingList>();
            if ( listData == null) 
            {
                Destroy(list);
                return;
            }
            listData.SetRecipeUIToppingList(DataManger.Inst.GetToppingResorceData(item).ItemName, pizzaData.BaseSizeToppingValues[index], pizzaData.LargeSizeToppingValues[index]);
            index++;
        }
    }
}
