using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movable Attributes", menuName = "GGJ/Movable/Attributes")]
public class MovableAttributes : ScriptableObject
{
    #region Fields and Properties
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float accelerationDuration = 1.0f;
    [SerializeField] private float decelerationRate = 2.0f; 
    [SerializeField] private AnimationCurve accelerationCurve = new AnimationCurve();

    [SerializeField] private float rotationDamping = 5.0f;
    [SerializeField] private float minMagnitudeRotation = .1f; 

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

    public Vector2 DampRotation(Vector2 _currentRotation, Vector2 _movement, float _magnitude)
    {
        Vector2 _direction = _currentRotation;
        Vector2.SmoothDamp(_currentRotation.normalized, _movement.normalized, ref _direction, rotationDamping); 
        return _direction * _magnitude; 
        /*
        Debug.Log(_currentRotation + " => " + _movement);
        float _angle = Mathf.Acos(Vector2.Dot(_currentRotation.normalized, _movement.normalized));
        //_angle = Mathf.Min(_angle, rotationDamping * Mathf.Deg2Rad); // Clamp angle between 0 and the rotation Dampling

        float _clockwise = Mathf.Sign(Vector3.Cross(_currentRotation, _movement).z); 
        Vector2 _dragAxis = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
        //Vector2 _perpendicular = new Vector2(-Mathf.Sin(_angle), Mathf.Cos(_angle));
        Vector2 _perpendicular = _clockwise > 0f ?
                                 new Vector2(-Mathf.Sin(_angle), Mathf.Cos(_angle)) :
                                 new Vector2(Mathf.Sin(_angle), -Mathf.Cos(_angle));

        Vector2 _direction = _dragAxis * _currentRotation.x + _perpendicular * _currentRotation.y;
        //Debug.DrawLine(Vector2.zero, _currentRotation, Color.yellow, 5f);
        //Debug.DrawLine(Vector2.zero, _dragAxis * 1.5f, Color.green, 5f);
        //Debug.DrawLine(Vector2.zero, _perpendicular * 1.5f, Color.red, 5f);
        //Debug.DrawLine(Vector2.zero, _direction, Color.blue, 5f);
        return _direction * _magnitude;  
        */
    }
    #endregion
}
