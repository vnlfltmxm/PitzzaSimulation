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
    public List<string> ToppingResorceList { get; set; }
    public List<int> BaseSizeToppingValues {  get; set; }
    public List<int> LargeSizeToppingValues { get; set; }
}

public class ToppingResorce
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int StartMaxCount {  get; set; }
    public int Price {  get; set; }
    public int MinBuyValues {  get; set; }
}

public class Player
{
    public string Name { get; set; }
    public string StartPizzaRecipe {  get; set; }
    public int StartMoney {  get; set; }
    public List<string> StartToppingResorceList { get; set; }
}