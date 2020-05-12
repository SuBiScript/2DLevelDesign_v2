using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerState
{
    public abstract void Update(PlayerController pc);
    public abstract void FixedUpdate(PlayerController  pc);
    public abstract void CheckTransition(PlayerController  pc);
    public abstract void OnStateEnter(PlayerController  pc);
    public abstract void OnStateExit(PlayerController  pc);
}