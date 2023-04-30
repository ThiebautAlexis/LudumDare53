using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : Movable
{
    #region Fields and Properties
    private float accelerationTime = 0f;
    private bool isInAcceleration = false;
    public bool IsInAcceleration
    {
        get
        {
            return isInAcceleration;
        }
        set
        {
            if (value == true && accelerationTime < 0f)
                accelerationTime = 0f;
            isInAcceleration = value;
        }
    }
    #endregion

    #region Methods
    protected override void ComputeVelocity()
    {
        if (isInAcceleration)
            accelerationTime += Time.deltaTime;
        else
            accelerationTime -= Time.deltaTime * attributes.DecelerationRate;

        currentSpeed = attributes.EvaluateSpeed(ref accelerationTime);
        base.ComputeVelocity();
    }

    protected override void RefreshRotation()
    { 
        if (movement.magnitude <= attributes.MinMagnitudeRotation) return;
        Quaternion _rot = Quaternion.LookRotation(Vector3.forward, Vector2.Perpendicular(movement));
        transform.rotation = _rot;
    }
    #endregion 
}
