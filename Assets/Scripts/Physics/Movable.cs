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

    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private MovableAttributes attributes; 
    [SerializeField] private float currentSpeed = 0f; 

    private Vector2 movement = Vector2.zero;
    private Vector2 lastMovement = Vector2.zero;
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
            if (value == false)
                accelerationTime = 0f;
            isInAcceleration = value; 
        } 
    } 
     
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
        lastMovement = transform.right; 
    }
    #endregion

    #region Methods
    private void MovableUpdate()
    {
        //rigidbody.position += velocity * Time.deltaTime; 

        UpdatePosition(); 
    }

    [SerializeField] private Vector2 direction = Vector2.one; 
    private void ComputeVelocity()
    {
        if (isInAcceleration)
        {
            accelerationTime += Time.deltaTime;
            currentSpeed = attributes.EvaluateSpeed(accelerationTime);
        }
        else accelerationTime = 0f; 

        if(movement != Vector2.zero)
        {
            movement = attributes.DampRotation(transform.right, movement); 
            lastMovement.Set(movement.x, movement.y);
        }
        velocity = attributes.DampRotation(Vector2.right, direction)/* * currentSpeed*/;
        movement.Set(0f, 0f); 
    }


    public void AddMovement(Vector2 _direction, float _magnitude = 1f)
    {
        movement += _direction * _magnitude; 
    }

    private void UpdatePosition()
    {
        //transform.position = rigidbody.position;
        Quaternion _rot = Quaternion.LookRotation(Vector3.forward, Vector2.Perpendicular(velocity));
        transform.rotation = _rot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + velocity);
    }

    #endregion
}
