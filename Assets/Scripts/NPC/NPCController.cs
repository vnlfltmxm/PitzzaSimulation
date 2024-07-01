using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class NPCController : MonoBehaviour
{
    private StateMachin<NPCController> _npcState = new StateMachin<NPCController>();
    private Animator _animator;
    public NavMeshAgent _navMeshAgent;
    private int _randomPizzaIndex;
    private Pizza _orderPizzaData;
    [HideInInspector]
    public Pizza Pizza { get { return _orderPizzaData; } }
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

        _orderPizzaData = DataManger.Inst.GetPizzaData(PlayerController.Instance.PizaaRecipe[_randomPizzaIndex]);
    }
}
