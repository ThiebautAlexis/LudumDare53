using CoolFramework.Core;
using UnityEngine;
using UnityEngine.Events;

public class Supervisor : Trigger
{
    public bool HasPlayerNearby { get; private set; } = false;

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        SetPlayerNearby(true);
    }

    public override void OnExit(Movable _movable)
    {
        base.OnExit(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        SetPlayerNearby(false);
    }

    public void SetPlayerNearby(bool _isPlayerNearby)
    {
        HasPlayerNearby = _isPlayerNearby;
    }
}
