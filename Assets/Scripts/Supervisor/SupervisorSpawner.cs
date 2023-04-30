using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupervisorSpawner : Trigger
{
    [SerializeField] private Supervisor supervisor;

    [SerializeField] private float chanceToSpawnPercentage = 20;

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        if (SupervisorManager.Instance.CurrentActiveSupervisor)
            return;
        
        float _random100 = Random.Range(0, 100);

        if (_random100 > chanceToSpawnPercentage)
            return;

        SupervisorManager.Instance.RegisterCurrentActiveSupervisor(supervisor);
        supervisor.gameObject.SetActive(true);
    }

    public override void OnExit(Movable _movable)
    {
        base.OnExit(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        if(SupervisorManager.Instance.CurrentActiveSupervisor && SupervisorManager.Instance.CurrentActiveSupervisor == supervisor)
            SupervisorManager.Instance.RemoveCurrentSupervisor();

        supervisor.gameObject.SetActive(false);
    }
}
