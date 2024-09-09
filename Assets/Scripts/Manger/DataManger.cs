using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.IO;

public class DataManger : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, Pizza> LoadedPizzaList { get; private set; }
    public Dictionary<string, ToppingResorce> LoadedToppingResorceList { get; private set; }
    public Dictionary<string, Player> LoadedPlayer { get; private set; }

    private readonly string _dataRootPath = Application.streamingAssetsPath;//"Application.StreamingAssetsPath";
    private readonly string _dataRootPathInHome = "C:/Users/qkr38/Downloads/PizzaDataTable";
    public static DataManger Inst { get; private set; }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
        Inst = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {

        ReadData(nameof(Pizza));//엑셀파일?변환파일의 이름을 가져오는거인듯? 그래서 TempCharter가 아닌것
        ReadData(nameof(ToppingResorce));
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열 읽기
            string jsonData = File.ReadAllText(filePath);

            // JSON 문자열을 객체로 변환
            Player data = new Player();
            data = LoadData();
            //data.Name = jsonData.Attribute(nameof(data.Name)).Value;
            //data.StartPizzaRecipe = data.Attribute(nameof(data.StartPizzaRecipe)).Value;
            //data.StartMoney = int.Parse(data.Attribute(nameof(data.StartMoney)).Value);
            //SetDataList(out data.StartToppingResorceList, data, "StartToppingResorceList");
            

        }
        else
        {
            ReadData(nameof(Player));
            
        }
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
            tempPizza.BasePrice = float.Parse(data.Attribute(nameof(tempPizza.BasePrice)).Value);
            tempPizza.LargePrice = float.Parse(data.Attribute(nameof(tempPizza.LargePrice)).Value);
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
            tempToppingResorce.ItemName = data.Attribute(nameof(tempToppingResorce.ItemName)).Value;
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
            //tempPlayer.StartPizzaRecipe = data.Attribute(nameof(tempPlayer.StartPizzaRecipe)).Value;
            SetDataList(out tempPlayer.StartPizzaRecipe, data, "StartPizzaRecipe");
            tempPlayer.StartMoney = int.Parse(data.Attribute(nameof(tempPlayer.StartMoney)).Value);
            SetDataList(out tempPlayer.StartToppingResorceList, data, "StartToppingResorceList");
            SetDataList(out tempPlayer.ToppingResorceCountList, data, "ToppingResorceCountList");
            LoadedPlayer.Add(tempPlayer.Name, tempPlayer);
        }


        

    }
    public Pizza GetPizzaData(string dataName)
    {
        if (LoadedPizzaList.Count == 0
            || !LoadedPizzaList.ContainsKey(dataName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedPizzaList[dataName];
    }
    public Player GetplayerData(string dataClassName)
    {
        if (LoadedPlayer.Count == 0
            || !LoadedPlayer.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedPlayer[dataClassName];
    }

    public ToppingResorce GetToppingResorceData(string dataClassName)
    {
        if (LoadedToppingResorceList.Count == 0
            || !LoadedToppingResorceList.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedToppingResorceList[dataClassName];
    }
   
    private void LoadFile()
    {
        // 파일 경로 설정
        filePath = Application.persistentDataPath + "/playerData.json";

        // 데이터 생성
        Player data = new Player();
        
        // 데이터 저장
        SaveData();

    }

    public void SaveData()
    {
        Player data = new Player();
        data.Name = PlayerController.Instance.Player.Name;
        data.StartPizzaRecipe = PlayerController.Instance.PizaaRecipe;
        data.StartToppingResorceList = PlayerController.Instance.PizaaToppingResorce;
        data.StartMoney = PlayerController.Instance.PlayerMoney;
        data.ToppingResorceCountList = PoolManger.Instance.ReturnToppingResorceCount();
        // 객체를 JSON 문자열로 변환
        string jsonData = JsonUtility.ToJson(data, true);

        // JSON 문자열을 파일에 저장
        File.WriteAllText(filePath, jsonData);

    }
    public IEnumerator SaveDataCo()
    {
        Player data = new Player();
        data.Name = PlayerController.Instance.Player.Name;
        data.StartPizzaRecipe = PlayerController.Instance.PizaaRecipe;
        data.StartToppingResorceList = PlayerController.Instance.PizaaToppingResorce;
        data.StartMoney = PlayerController.Instance.PlayerMoney;
        data.ToppingResorceCountList = PoolManger.Instance.ReturnToppingResorceCount();
        // 객체를 JSON 문자열로 변환
        string jsonData = JsonUtility.ToJson(data, true);

        // JSON 문자열을 파일에 저장
        File.WriteAllText(filePath, jsonData);
        yield return null;

    }

    Player LoadData()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열 읽기
            string jsonData = File.ReadAllText(filePath);
            LoadedPlayer = new Dictionary<string, Player>();
            // JSON 문자열을 객체로 변환
            Player data = JsonUtility.FromJson<Player>(jsonData);
            LoadedPlayer.Add(data.Name, data);
            
            return data;
        }
        else
        {
           
            return null;
        }
    }
    public bool IsFileCheck()
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            return false;
        }
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
