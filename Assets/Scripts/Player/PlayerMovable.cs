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

    public void SetAttributes(MovableAttributes _attributes)
    {
        Debug.Log("hello?"); 
        attributes = _attributes; 
    }
    #endregion
}
