using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolFramework.Core;

public class Movable : CoolBehaviour, IUpdate, IDynamicUpdate
{
    #region Fields and Properties
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update | UpdateRegistration.Dynamic;

    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private new Collider2D collider; 

    [SerializeField] private MovableAttributes attributes; 
    [SerializeField] private float currentSpeed = 0f; 

    [SerializeField] private Vector2 movement = Vector2.zero;
    [SerializeField] private float movementMagnitude = 1f;
    [SerializeField] private Vector2 lastMovement = Vector2.zero;
    [SerializeField] private Vector2 forces = Vector2.zero; 

    [SerializeField] private float accelerationTime = 0f;
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

    public Vector2 Velocity => (movement + forces) * Time.deltaTime ; 
    #endregion 

    #region Overriden Methods
    void IDynamicUpdate.Update() => MovableUpdate();
    void IUpdate.Update()
    {
        ComputeVelocity();
    }
    protected override void OnInit()
    {
        base.OnInit();
    }
    #endregion

    #region Methods
    private void MovableUpdate()
    {
        rigidbody.position += Velocity; 
        UpdatePosition();

        movement.Set(0f, 0f);
    }

    [SerializeField] private float driftForce = 0f; 
    private void ComputeVelocity()
    {
        currentSpeed = attributes.EvaluateSpeed(ref accelerationTime);
        if (isInAcceleration)
            accelerationTime += Time.deltaTime;
        else 
            accelerationTime -= Time.deltaTime * attributes.DecelerationRate;


        if (movement == Vector2.zero) movement = transform.right; 
        movement = attributes.DampRotation(transform.right, movement.normalized, currentSpeed * movementMagnitude);
        
        driftForce = -Vector2.Dot(transform.up, movement) * attributes.InertiaCoefficient;
        AddForce(transform.up * driftForce * attributes.InertiaCoefficient);


        lastMovement.Set(movement.x, movement.y);
        forces = Vector2.ClampMagnitude(forces, currentSpeed);
        forces = Vector2.MoveTowards(forces, Vector2.zero, attributes.DriftCoefficient * Time.deltaTime);
    }


    public void AddMovement(Vector2 _direction)
    {
        movement += _direction; 
    }
    public void AddForce(Vector2 _force)
    {
        forces += _force;
    }

    public void SetMovementMagnitude(float _magnitude) => movementMagnitude = _magnitude; 

    private void UpdatePosition()
    {
        transform.position = rigidbody.position;
        if (movement.magnitude <= attributes.MinMagnitudeRotation) return;
        Quaternion _rot = Quaternion.LookRotation(Vector3.forward, Vector2.Perpendicular(movement));
        transform.rotation = _rot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + movement);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Velocity);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + forces);
    }

    #endregion
}
