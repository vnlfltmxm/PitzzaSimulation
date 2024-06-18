using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachin<T> where T : class
{
    public BaseState<T> CurrentState { get; set; }  // ���� ����
    private Dictionary<Enum, BaseState<T>> states =
    new Dictionary<Enum, BaseState<T>>();


    public StateMachin(Enum stateName, BaseState<T> state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(Enum stateName, BaseState<T> state)  // ���� ���
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState<T> GetState(Enum stateName)  // ���� ��������
    {
        if (states.TryGetValue(stateName, out BaseState<T> state))
            return state;
        return null;
    }

    public void DeleteState(Enum removeStateName)  // ���� ����
    {
        if (states.ContainsKey(removeStateName))
        {
            states.Remove(removeStateName);
        }
    }

    public void ChangeState(Enum nextStateName)    // ���� ��ȯ
    {
        CurrentState.OnExitState();   //���� ���¸� �����ϴ� �޼ҵ带 �����ϰ�,
        if (states.TryGetValue(nextStateName, out BaseState<T> newState)) // ���� ��ȯ
        {
            CurrentState = newState;
        }
        CurrentState.OnEnterState();  // ���� ���� ���� �޼ҵ� ����
    }

    public void UpdateState()
    {
        CurrentState.OnUpdateState();
    }

    public void FixedUpdateState()
    {
        CurrentState.OnFixedUpdateState();
    }
}

