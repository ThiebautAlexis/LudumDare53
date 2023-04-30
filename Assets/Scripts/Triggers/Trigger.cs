using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolFramework.Core;
public abstract class Trigger : CoolBehaviour 
{
    public int ID => GetInstanceID();

    public bool Compare(Trigger other) => ID == other.ID; 

    #region Callbacks
    /// <summary>
    /// Called when something enters this trigger.
    /// </summary>
    /// <param name="_movable">Movable who entered this trigger.</param>
    public virtual void OnEnter(Movable _movable) { }

    /// <summary>
    /// Called when something exits this trigger.
    /// </summary>
    /// <param name="_movable">Movable who exited this trigger.</param>
    public virtual void OnExit(Movable _movable) { }
    #endregion
}
