using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolFramework.Core; 

public class DangerZone : Trigger, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;
    private List<Movable> movables = new List<Movable>(); 
    [SerializeField] private int baseScore = 5;
    [SerializeField] private float minVelocity; 

    public void Update()
    {
        if (movables.Count > 0)
        {
            for (int i = 0; i < movables.Count; i++)
            {
                if (movables[i].Velocity.magnitude > minVelocity) 
                    ScoreManager.Instance.AddScore(baseScore); 
            }
        }
    }

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);
        movables.Add(_movable); 
    }

    public override void OnExit(Movable _movable)
    {
        base.OnExit(_movable);
        movables.Remove(_movable);
    }
}
