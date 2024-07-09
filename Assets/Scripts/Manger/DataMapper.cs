using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza
{
    public int DataId { get; set; }
    public string ClassName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public string PrefabPath { get; set; }
    public float BaseSizeRidous {  get; set; }
    public float LargeSizeRidous {  get; set; }
    public float BasePrice { get; set; }
    public float LargePrice { get; set; }

    public List<string> ToppingResorceList = new List<string>();
    public List<int> BaseSizeToppingValues = new List<int>();
    public List<int> LargeSizeToppingValues = new List<int>();
}

public class ToppingResorce
{
    public string Name { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public int StartMaxCount {  get; set; }
    public int Price {  get; set; }
    public int MinBuyValues {  get; set; }
}

public class Player
{
    public string Name; /*{ get; set; }*/
    public List<string> StartPizzaRecipe = new List<string>();
    public float StartMoney; /*{  get; set; }*/
    public List<string> StartToppingResorceList = new List<string>();
    public List<int> ToppingResorceCountList = new List<int>();
}