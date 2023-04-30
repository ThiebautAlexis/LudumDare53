using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTrigger : Trigger
{
    public override void OnEnter(Movable _movable)
    {
        Debug.Log("enter"); 
    }

    public override void OnExit(Movable _movable)
    {
        Debug.Log("exit"); 
    }
}
