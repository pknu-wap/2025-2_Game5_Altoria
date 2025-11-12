using System;
using UnityEngine;
using static Define;

[Serializable]
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;
    public event Action<PlayerState> OnStateChanged;

 
    public void SetState(PlayerState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    public void AddState(PlayerState state)
    {
        if (HasState(state)) return;

        CurrentState |= state;
        OnStateChanged?.Invoke(CurrentState);
    }

  
    public void RemoveState(PlayerState state)
    {
        if (!HasState(state)) return;

        CurrentState &= ~state;
        OnStateChanged?.Invoke(CurrentState);
    }

  
  
    public bool HasState(PlayerState state)
    {
        return (CurrentState & state) != 0;
    }


    public bool CanReceiveInput()
    {
        return !HasState(PlayerState.Die) && !HasState(PlayerState.Interacting);
    }


    public void Reset()
    {
        CurrentState = PlayerState.Idle;
        OnStateChanged?.Invoke(CurrentState);
    }
}
