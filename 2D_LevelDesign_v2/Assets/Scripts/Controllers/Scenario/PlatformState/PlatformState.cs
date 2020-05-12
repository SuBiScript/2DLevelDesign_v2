using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformState 
{
    public abstract void Init(PlatformController pc);
    public abstract void Update(PlatformController pc);
    public abstract void FixedUpdate(PlatformController pc);
    public abstract void CheckTransition(PlatformController pc);
}