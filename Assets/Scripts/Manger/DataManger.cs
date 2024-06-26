using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DataManger : MonoBehaviour
{
    public Dictionary<string, Pizza> LoadedPizzaList { get; private set; }
    public Dictionary<string, ToppingResorce> LoadedToppingResorceList { get; private set; }
    public Dictionary<string, Player> LoadedPlayer { get; private set; }

    private readonly string _dataRootPath = "C:/Users/KGA/Desktop/PizzaDataTable";//읽는거 실패시 \\을 /로

    public static DataManger Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {

        ReadData(nameof(Pizza));//엑셀파일?변환파일의 이름을 가져오는거인듯? 그래서 TempCharter가 아닌것
        ReadData(nameof(ToppingResorce));
        ReadData(nameof(Player));
    }

    private void ReadData(string name)
    {
        switch (name)
        {
            case nameof(Pizza):
                ReadPizzaTable(name);
                break;
            case nameof(ToppingResorce):
                ReadToppingResorceTable(name);
                break;
            case nameof(Player):
                ReadPlayerTable(name);
                break;
        }
    }

    //종현이형 변환기 사용시 행으로 나누어져 있음 XML구조 형식 RDB형식에 더 가깝다고함
    //구조적,확장성 좋음
    //다만 파싱이 조금 힘들고 데이터가 많으면 파일크기가 커짐
    //데이터가 자주 변경되거나 확장가능성이 높다면 이게 더 효율적
    //private void ReadCharacterTable(string name)
    //{
    //    LoadedCharaterList = new Dictionary<int, TempCharacter>();

    //    XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");//제이슨의 경우 XDocumet랑xml을 바꿔줄것
    //    var dataElements = doc.Descendants("row");//변환된 파일의 <data sss>를 얻어온다는뜻? <data sss>의 data가 바뀌면 ()안도 바꿔야함


    //    foreach (var dataElement in dataElements)
    //    {
    //        var tempCharacter = new TempCharacter();
    //        tempCharacter.DataId = int.Parse(dataElement.Element(nameof(tempCharacter.DataId)).Value);
    //        tempCharacter.Name = dataElement.Element(nameof(tempCharacter.Name)).Value;
    //        tempCharacter.Description = dataElement.Element(nameof(tempCharacter.Description)).Value;
    //        tempCharacter.IconPath = dataElement.Element(nameof(tempCharacter.IconPath))?.Value ?? string.Empty;


    //        string skillNameListStr = dataElement.Element("SkillNameList")?.Value ?? string.Empty;
    //        //토큰을 분리->토큰이란 자료를 잘라서 나오는거?필요한 자료만 취하는것?
    //        if (string.IsNullOrEmpty(skillNameListStr) == false)
    //        {
    //            skillNameListStr = skillNameListStr.Replace("{", string.Empty);
    //            skillNameListStr = skillNameListStr.Replace("}", string.Empty);

    //            var skillNames = skillNameListStr.Split(',');

    //            var list = new List<string>();
    //            if (skillNames.Length > 0)
    //            {
    //                foreach (var skilName in skillNames)
    //                {
    //                    list.Add(skilName);
    //                    tempCharacter.SkillClassNameList = list;
    //                }
    //            }
    //        }

    //        LoadedCharaterList.Add(tempCharacter.DataId, tempCharacter);
    //    }
    //}
    private void ReadPizzaTable(string name)
    {
        LoadedPizzaList = new Dictionary<string, Pizza>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempPizza = new Pizza();
            tempPizza.DataId = int.Parse(data.Attribute(nameof(tempPizza.DataId)).Value);
            tempPizza.ClassName = data.Attribute(nameof(tempPizza.ClassName)).Value;
            tempPizza.Name = data.Attribute(nameof(tempPizza.Name)).Value;
            tempPizza.Description = data.Attribute(nameof(tempPizza.Description)).Value;
            tempPizza.BaseSizeRidous = float.Parse(data.Attribute(nameof(tempPizza.BaseSizeRidous)).Value);
            tempPizza.LargeSizeRidous = float.Parse(data.Attribute(nameof(tempPizza.LargeSizeRidous)).Value);

            //tempSkill.IconName = data.Attribute(nameof(tempSkill.IconName)).Value;
            SetDataList(out tempPizza.ToppingResorceList, data, "ToppingResorceList");
            SetDataList(out tempPizza.BaseSizeToppingValues, data, "BaseSizeToppingValues");
            SetDataList(out tempPizza.LargeSizeToppingValues, data, "LargeSizeToppingValues");

            LoadedPizzaList.Add(tempPizza.ClassName, tempPizza);


        }
    }
    private void SetDataList<T>(out List<T> usingList, XElement data,string listName)
    {
        string toppingResorceListStr = data.Attribute(listName).Value;
        if (!string.IsNullOrEmpty(toppingResorceListStr))
        {
            toppingResorceListStr = toppingResorceListStr.Replace("{", string.Empty);
            toppingResorceListStr = toppingResorceListStr.Replace("}", string.Empty);

            var names = toppingResorceListStr.Split(',');

            var list = new List<T>();

            if (names.Length > 0)
            {
                foreach (var pizzaName in names)
                {
                    T value = (T)Convert.ChangeType(pizzaName, typeof(T));
                    list.Add(value);
                }
            }
            usingList = list;
        }
        else
        {
            usingList = null;
        }
    }
    //교수님 변환기 사용시
    //작은 데이터셋에서 유리
    //확장 힘듬
    //데잍터가 고정 되어있고 파싱의 효율이 중요하다면 이게 더 효율적
    private void ReadToppingResorceTable(string name)
    {
        LoadedToppingResorceList = new Dictionary<string, ToppingResorce>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempToppingResorce = new ToppingResorce();
            tempToppingResorce.Name = data.Attribute(nameof(tempToppingResorce.Name)).Value;
            tempToppingResorce.Description = data.Attribute(nameof(tempToppingResorce.Description)).Value;
            tempToppingResorce.Price = int.Parse(data.Attribute(nameof(tempToppingResorce.Price)).Value);
            tempToppingResorce.MinBuyValues = int.Parse(data.Attribute(nameof(tempToppingResorce.MinBuyValues)).Value);
            tempToppingResorce.StartMaxCount = int.Parse(data.Attribute(nameof(tempToppingResorce.StartMaxCount)).Value);
            //tempSkill.IconName = data.Attribute(nameof(tempSkill.IconName)).Value;

            
            LoadedToppingResorceList.Add(tempToppingResorce.Name, tempToppingResorce);
        }
    }

    private void ReadPlayerTable(string name)
    {
        LoadedPlayer = new Dictionary<string, Player>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempPlayer = new Player();
            tempPlayer.Name = data.Attribute(nameof(tempPlayer.Name)).Value;
            tempPlayer.StartPizzaRecipe = data.Attribute(nameof(tempPlayer.StartPizzaRecipe)).Value;
            tempPlayer.StartMoney = int.Parse(data.Attribute(nameof(tempPlayer.StartMoney)).Value);
            SetDataList(out tempPlayer.StartToppingResorceList, data, "StartToppingResorceList");
            LoadedPlayer.Add(tempPlayer.Name, tempPlayer);
        }
    }
    public Pizza GetPizzaData( DataManger manger, string dataName)
    {
        if (manger.LoadedPizzaList.Count == 0
            || manger.LoadedPizzaList.ContainsKey(dataName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return manger.LoadedPizzaList[dataName];
    }
    public Player GetplayerData( DataManger manger, string dataClassName)
    {
        if (manger.LoadedPlayer.Count == 0
            || manger.LoadedPlayer.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return manger.LoadedPlayer[dataClassName];
    }

    public ToppingResorce GetBuffData( DataManger manger, string dataClassName)
    {
        if (manger.LoadedToppingResorceList.Count == 0
            || manger.LoadedToppingResorceList.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return manger.LoadedToppingResorceList[dataClassName];
    }

    //public string GetSkillName( DataTemporalManger manger, string dataClassName)
    //{
    //    var skillData = manger.GetSkillData(dataClassName);

    //    return (skillData != null) ? skillData.Name : string.Empty;
    //}

    //public  string GetBuffDescription( DataTemporalManger manger, string dataClassName)
    //{
    //    var buffData = manger.GetBuffData(dataClassName);
    //    string desc = string.Empty;
    //    if (buffData != null)
    //    {
    //        desc = string.Format(buffData.Description, buffData.BuffValues);
    //    }

    //    return desc;
    //}
}
