using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class NPCController : MonoBehaviour
{
    private Dictionary<string, int> _checkOrderPizzaDic = new Dictionary<string, int>();
    private StateMachin<NPCController> _npcState = new StateMachin<NPCController>();
    private Animator _animator;
    public NavMeshAgent _navMeshAgent;
    private int _randomPizzaIndex;
    private int _pizzaSize;
    private Pizza _orderPizzaData;
    [HideInInspector]
    public Pizza Pizza { get { return _orderPizzaData; } }
    public int PizzaSize { get { return _pizzaSize; } }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        InitState();
    }

    private void OnEnable()
    {
        
    }
    void Start()
    {
        _npcState.ChangeState(NPCStateName.WALK);
    }

    // Update is called once per frame
    void Update()
    {
        _npcState.UpdateState();
    }

    private void InitState()
    {
        _npcState.AddState(NPCStateName.IDLE, new NPCIdleState(this));
        _npcState.AddState(NPCStateName.WALK, new NPCWalkState(this));
        _npcState.AddState(NPCStateName.ORDER, new NPCOrderState(this));
        _npcState.AddState(NPCStateName.WAITINGPIZZA, new NPCWaitingPizzaState(this));
        _npcState.AddState(NPCStateName.CHECKPIZZA, new NPCCheckPizzaState(this));
        _npcState.AddState(NPCStateName.LEAVE, new NPCLeaveState(this));

        _npcState.SetCurrentState(NPCStateName.IDLE);
    }

    public void ChangeNPCState(NPCStateName name)
    {
        _npcState.ChangeState(name);
    }

    public void SetNPCDestinationPos(Vector3 destinationPos)
    {
        _navMeshAgent.destination = destinationPos;
    }

    public void SetNPCIsStopping(bool value)
    {
        _navMeshAgent.isStopped = value;
    }

    public void SetBoolNPCAnimotorPropertyToName(string prpertyName,bool value)
    {
        _animator.SetBool(prpertyName, value);
    }

    public bool CheckArriveDesrtination()
    {
        if (Vector3.Distance(gameObject.transform.position, _navMeshAgent.destination) <= _navMeshAgent.stoppingDistance)
        {
            return true;
        }
            return false;
    }
    public void RegisterCheckPizza()
    {
        EventManger.Instance.CheckPizza += CheckPizza;
    }
    public void UnRegisterCheckPizza()
    {
        EventManger.Instance.CheckPizza -= CheckPizza;
    }
    public void RegisterChangeStateToLeave()
    {
        EventManger.Instance.ChangeNPCStateToLeave += ChangeNPCStateToLeave;
    }
    public void UnRegisterChangeStateToLeave()
    {
        EventManger.Instance.ChangeNPCStateToLeave -= ChangeNPCStateToLeave;
    }
    public void ChangeNPCStateToLeave()
    {
        _npcState.ChangeState(NPCStateName.LEAVE);
    }
    public void RegisterCheckState()
    {
        EventManger.Instance.CheckNPCState += CheckCurrentState;
    }
    public void UnRegisterCheckState()
    {
        EventManger.Instance.CheckNPCState -= CheckCurrentState;
    }
    public bool CheckCurrentState(Enum stateName, GameObject target)
    {
        if (EventManger.Instance.CheckEventTarget(target, this.gameObject))
        {
            if (_npcState.CurrentState == _npcState.GetState(stateName))
            {
                return true;
            }
        }
        

        return false;
    }
    public void SetRandomIndex()
    {
        _randomPizzaIndex = 0;
        _randomPizzaIndex = UnityEngine.Random.Range(0, PlayerController.Instance.PizaaRecipe.Count);
        _pizzaSize = UnityEngine.Random.Range(0, 2);
        _orderPizzaData = DataManger.Inst.GetPizzaData(PlayerController.Instance.PizaaRecipe[_randomPizzaIndex]);

        for (int i = 0; i < _orderPizzaData.ToppingResorceList.Count; i++)
        {
            if(_pizzaSize == 0)
            {
                _checkOrderPizzaDic.Add(_orderPizzaData.ToppingResorceList[i], _orderPizzaData.BaseSizeToppingValues[i]);
            }
            else
            {
                _checkOrderPizzaDic.Add(_orderPizzaData.ToppingResorceList[i], _orderPizzaData.LargeSizeToppingValues[i]);
            }
        }
    }
    public void CheckPizza(Dough pizza)
    {
        if( pizza == null)
        {
            return;
        }

        if (pizza._isPizzaCooked == false)
        {
            PrintCheckPizzaText("이게 뭐야 하나도 안 익었잖아요",true);
            return;
        }

        if(pizza.IsPizzaOverCooked == true)
        {
            PrintCheckPizzaText("장난쳐요? 다 탔잖아요", true);
            return;
        }

        if (_orderPizzaData.ToppingResorceList.Count != pizza.CheckPizzaList.Count
            || CheckPizzaSize(pizza) == false
            || CheckPizzaToppingList(pizza) == false) 
        {
            PrintCheckPizzaText("주문한 피자랑 다르잖아요", true);
            return;
        }

        if(CheckPizzaToppingListValue(pizza) == false)
        {
            PrintCheckPizzaText("재료 양이 다르잖아요", true);
            return;
        }

        PrintCheckPizzaText("감사합니다", true);
    }
    private bool CheckPizzaSize(Dough pizza)
    {
        switch (_pizzaSize)
        {
            case 0:
                if (CheckePizzaSizeRadious(pizza.gameObject.transform.localScale.x, _orderPizzaData.BaseSizeRidous, 0.01f))
                {
                    return true;
                }
                break;
            case 1:
                if (CheckePizzaSizeRadious(pizza.gameObject.transform.localScale.x, _orderPizzaData.LargeSizeRidous, 0.01f)) 
                {
                    return true;
                }
                break;
            default:
                break;
        }

        return false;
        
    }
    private bool CheckePizzaSizeRadious(float pizaaScale, float orderPizzaSize, float tolerance)
    {
        return Mathf.Abs(pizaaScale - orderPizzaSize) <= tolerance;
    }
    private void PrintCheckPizzaText(string text,bool isNPCChangedLeave)
    {
        UIManger.Instance.PrintNPCText(text,isNPCChangedLeave);
        Invoke(nameof(ChangeNPCStateToLeave), 1.5f);
    }
    private void OnDisableTextBG()
    {
        UIManger.Instance.SetTextBGActive(false);
    }
    private bool CheckPizzaToppingList(Dough pizza)
    {
        foreach (var item in _orderPizzaData.ToppingResorceList)
        {
            if (!pizza.CheckPizzaList.ContainsKey(item))
            {
                return false;
            }
        }

        return true;
    }
    private bool CheckPizzaToppingListValue(Dough pizza)
    {
        foreach (var item in _checkOrderPizzaDic.Keys)
        {
            if (pizza.CheckPizzaList[item] != _checkOrderPizzaDic[item])
            {
                return false;
            }
        }
        return true;
    }

}
