using CoolFramework.Core;
using UnityEngine;
using UnityEngine.Events;

public class Supervisor : Trigger, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private UnityEvent onEnableSupervisor;
    [SerializeField] private UnityEvent onDisableSupervisor;

    [SerializeField] private float durationAfterExitInSeconds = 15;
    private float currentExitTimer = 0;

    public bool HasPlayerNearby { get; private set; } = false;


    void IUpdate.Update()
    {
        if (!HasPlayerNearby)
            return;

        currentExitTimer += Time.deltaTime;

        if(currentExitTimer >= durationAfterExitInSeconds)
            DisableSupervisor();
    }

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        EnableSupervisor();
    }

    public override void OnExit(Movable _movable)
    {
        base.OnExit(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        DisableSupervisor();
    }

    private void EnableSupervisor()
    {
        HasPlayerNearby = true;
        currentExitTimer = 0;

        onEnableSupervisor?.Invoke();
    }

    private void DisableSupervisor()
    {
        HasPlayerNearby = false;
        currentExitTimer = 0;

        onDisableSupervisor?.Invoke();
    }
}
