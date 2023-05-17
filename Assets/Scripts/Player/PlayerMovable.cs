using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : Movable
{
    #region Fields and Properties
    [Header("Drift")]
    [SerializeField] private bool isInDrift = false; 
    [SerializeField] private TrailRenderer[] trails; 
    [Header("Score")]
    [SerializeField] private float minDisplacementScore = 1.0f;
    [SerializeField] private float minDriftThreshold = 1.0f;
    [SerializeField] private float minDriftDuration = .25f;
    [SerializeField] private int baseScore = 100;


    [SerializeField] private float comboDuration = 1.25f;
    [SerializeField] private float driftingTimer = 0f; 
    [SerializeField] private float comboTimer = 0f; 

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

    public bool IsInDrift
    {
        get
        {
            return isInDrift; 
        }
        private set
        {
            isInDrift= value;
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].emitting = value;
            }
        }
    }
    #endregion

    #region Methods
    private int driftCount = 0; 
    protected override void OnAppliedVelocity(Vector2 _velocity, Vector2 _displacement, List<RaycastHit2D> _buffer)
    {
        base.OnAppliedVelocity(_velocity, _displacement, _buffer);

        if (_buffer.Count > 0)
        {
            ScoreManager.Instance.BreakCombo();
            driftingTimer = 0f;
            comboTimer = 0f;
            IsInDrift = false;
            driftCount = 0;
            return; 
        }
        else if (_displacement.magnitude < minDisplacementScore * Time.deltaTime)
        {
            driftingTimer = 0f;
            comboTimer = 0f;
            IsInDrift = false;
            driftCount = 0;
            return; 
        }

        if (isInDrift)
        {
            // Check is it is still drifting after a certain amount of time
            driftingTimer += Time.deltaTime;
            IsInDrift = forces.magnitude > minDriftThreshold && Mathf.Abs(driftForce) > 0f;
            if(driftingTimer >= minDriftDuration)
            {
                driftCount++; 
                driftingTimer = 0f;
            }
            // If not drifting anymore => Apply Score
            if (!isInDrift && driftCount > 0)
            {
                ScoreManager.Instance.AddScore(baseScore * driftCount);
                driftCount = 0;
            }    

            comboTimer += Time.deltaTime;
            // If drifting for more than X seconds => Add Combo ! 
            if (comboTimer > comboDuration)
            {
                comboTimer = 0f; 
                ScoreManager.Instance.AugmentCombo(1); 
            }
            return; 
        }
        IsInDrift = forces.magnitude > minDriftThreshold && Mathf.Abs(driftForce) > 0f;
    }

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
        attributes = _attributes; 
    }
    #endregion
}
