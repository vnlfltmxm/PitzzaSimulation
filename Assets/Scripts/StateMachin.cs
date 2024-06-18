using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachin<T> where T : class
{
    public BaseState<T> CurrentState { get; set; }  // 현재 상태
    private Dictionary<Enum, BaseState<T>> states =
    new Dictionary<Enum, BaseState<T>>();


    public StateMachin(Enum stateName, BaseState<T> state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void AddState(Enum stateName, BaseState<T> state)  // 상태 등록
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState<T> GetState(Enum stateName)  // 상태 꺼내오기
    {
        if (states.TryGetValue(stateName, out BaseState<T> state))
            return state;
        return null;
    }

    public void DeleteState(Enum removeStateName)  // 상태 삭제
    {
        if (states.ContainsKey(removeStateName))
        {
            states.Remove(removeStateName);
        }
    }

    public void ChangeState(Enum nextStateName)    // 상태 전환
    {
        CurrentState.OnExitState();   //현재 상태를 종료하는 메소드를 실행하고,
        if (states.TryGetValue(nextStateName, out BaseState<T> newState)) // 상태 전환
        {
            CurrentState = newState;
        }
        CurrentState.OnEnterState();  // 다음 상태 진입 메소드 실행
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

