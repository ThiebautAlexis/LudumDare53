using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movable Attributes", menuName = "GGJ/Movable/Attributes")]
public class MovableAttributes : ScriptableObject
{
    #region Fields and Properties
    [Header("Speed")]
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float accelerationDuration = 1.0f;
    [SerializeField] private float decelerationRate = 2.0f; 
    [SerializeField] private AnimationCurve accelerationCurve = new AnimationCurve();

    [Header("Rotation")]
    [SerializeField] private float rotationDamping = 5.0f;
    [SerializeField] private float minMagnitudeRotation = .1f;

    [Header("Inertia")]
    [SerializeField] float inertiaCoefficient = .1f; 
    [SerializeField] float driftCoefficient = .1f;

    public float DecelerationRate => decelerationRate; 
    public float InertiaCoefficient => inertiaCoefficient;
    public float DriftCoefficient => driftCoefficient;

    public float MinMagnitudeRotation => minMagnitudeRotation; 
    #endregion

    #region Methods 
    public float EvaluateSpeed(ref float _duration)
    {
        _duration = Mathf.Clamp(_duration, 0f, accelerationDuration); 
        return accelerationCurve.Evaluate(_duration / accelerationDuration) * maxSpeed; 
    }
    public float EvaluateSpeed(float _duration)
    {
        _duration = Mathf.Clamp(_duration, 0f, accelerationDuration);
        return accelerationCurve.Evaluate(_duration / accelerationDuration) * maxSpeed;
    }
    public Vector2 DampRotation(Vector2 _currentRotation, Vector2 _movement, float _magnitude)
    {
        Vector2 _velocity = Vector2.zero; 
        Vector2 _direction = Vector2.SmoothDamp(_currentRotation.normalized, _movement.normalized, ref _velocity, Time.deltaTime * rotationDamping);
        return _direction * _magnitude; 
    }
    #endregion
}
