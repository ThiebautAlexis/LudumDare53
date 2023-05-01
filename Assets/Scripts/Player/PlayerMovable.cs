using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : Movable
{
    #region Fields and Properties
    
    [SerializeField] private float minDisplacementScore = 1.0f;
    [SerializeField] private float scoringThreshold = 1.0f;
    [SerializeField] private int baseScore = 100;


    [SerializeField] private float comboDuration = 1.25f;
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
    #endregion

    #region Methods
    protected override void OnAppliedVelocity(Vector2 _velocity, Vector2 _displacement, List<RaycastHit2D> _buffer)
    {
        base.OnAppliedVelocity(_velocity, _displacement, _buffer);

        if (_buffer.Count > 0)
        {
            ScoreManager.Instance.BreakCombo();
            return; 
        }

        if (_displacement.magnitude < minDisplacementScore * Time.deltaTime)
        {
            comboTimer = 0f; 
            return;
        }
        if(Mathf.Abs(driftForce) > scoringThreshold)
        {
            ScoreManager.Instance.AddScore(baseScore);
            comboTimer += Time.deltaTime; 
        }
        if(comboTimer > comboDuration)
        {
            ScoreManager.Instance.AugmentCombo(1);
            comboTimer = 0f; 
        }
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
