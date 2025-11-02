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
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
  
    public bool Is(PlayerState state) => CurrentState == state;

  
    public bool CanReceiveInput()
    {
        return CurrentState switch
        {
            PlayerState.Die => false,
            PlayerState.Interacting => false,
            _ => true
        };
    }
}
