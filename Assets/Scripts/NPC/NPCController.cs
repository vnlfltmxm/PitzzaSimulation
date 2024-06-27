using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private StateMachin<NPCController> _npcState = new StateMachin<NPCController>();
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        InitState();
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
        
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) 
        {
            return true;
        }
            return false;
    }
}
