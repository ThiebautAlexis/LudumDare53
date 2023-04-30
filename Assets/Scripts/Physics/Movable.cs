using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolFramework.Core;

public class Movable : CoolBehaviour, IUpdate, IDynamicUpdate
{
    #region Fields and Properties
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update | UpdateRegistration.Dynamic;

    [Header("Settings")]
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private MovableAttributes attributes; 
    [SerializeField] private new MovableCollider collider;
    private CollisionSystem collisionSystem;

    [Header("Movement")]
    [SerializeField] private float currentSpeed = 0f; 
    [SerializeField] private Vector2 movement = Vector2.zero;
    [SerializeField] private Vector2 lastMovement = Vector2.zero;
    private float movementMagnitude = 1f;
    [Header("Forces")]
    [SerializeField] private Vector2 forces = Vector2.zero; 
    private float driftForce = 0f;
    [Header("Collisions")]
    [SerializeField] private LayerMask collisionLayer; 
    [SerializeField] private float bounciness = 1.0f;  


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

    public Rigidbody2D Rigidbody => rigidbody; 
    public MovableCollider Collider => collider;

    public Vector2 Velocity => (movement + forces) * Time.deltaTime ;

    public Vector2 RawVelocity => (movement + forces);
    public Vector2 Movement => movement * Time.deltaTime; 
    public Vector2 Forces => forces * Time.deltaTime; 

    #endregion

    #region Overriden Methods
    void IDynamicUpdate.Update() => MovableUpdate();
    void IUpdate.Update() => ComputeVelocity();
    protected override void OnInit()
    {
        base.OnInit();
        collisionSystem = new CollisionSystem(this);
        collider.Initialize(collisionLayer); 
    }
    #endregion

    #region Methods
    /// <summary>
    /// Core loop of the movable.
    /// </summary>
    private void MovableUpdate()
    {
        if(RawVelocity != Vector2.zero)
        {
            Vector2 _lastPosition = rigidbody.position; 
            List<RaycastHit2D> _buffer = collisionSystem.PerformCollisions(Movement, Forces);
            UpdatePosition();
            Vector2 _displacement = rigidbody.position - _lastPosition;
            OnAppliedVelocity(Velocity, _displacement, _buffer); 
        }

        movement.Set(0f, 0f);
    }

    /// <summary>
    /// When the Velocity has been applied, if an obstacle has been touched, apply bounciness.
    /// </summary>
    /// <param name="_velocity">Velocity applied theorically</param>
    /// <param name="_displacement">Displacement applied to the movable</param>
    /// <param name="_buffer">Hit obstacles</param>
    private void OnAppliedVelocity(Vector2 _velocity, Vector2 _displacement, List<RaycastHit2D> _buffer)
    {
        RaycastHit2D _hit;
        // Iterate over collision buffer to find encountered obstacles.
        for (int _i = 0; _i < _buffer.Count; _i++)
        {
            _hit = _buffer[_i];
            AddForce(_hit.normal * bounciness); 
        }
    }

    /// <summary>
    /// Compute the Velocity according to the acceleration, the targeted movement and rotation.
    /// </summary>
    private void ComputeVelocity()
    {
        if (isInAcceleration)
            accelerationTime += Time.deltaTime;
        else 
            accelerationTime -= Time.deltaTime * attributes.DecelerationRate;

        currentSpeed = attributes.EvaluateSpeed(ref accelerationTime);

        if (movement.magnitude == 0f) movement = transform.right; 
        movement = attributes.DampRotation(transform.right, movement.normalized, currentSpeed * movementMagnitude);
        
        driftForce = Vector2.Dot(-transform.up, movement) * attributes.InertiaCoefficient;
        AddForce(transform.up * driftForce * attributes.InertiaCoefficient);


        lastMovement.Set(movement.x, movement.y);
        forces = Vector2.ClampMagnitude(forces, currentSpeed);
        forces = Vector2.MoveTowards(forces, Vector2.zero, attributes.DriftCoefficient * Time.deltaTime);
    }

    /// <summary>
    /// Update the position according to the rigidbody position.
    /// If the magnitude of the movement is great enough, apply rotation.
    /// Refreseh the overlap to detect the Triggers
    /// </summary>
    private void UpdatePosition()
    {
        transform.position = rigidbody.position;
        RefreshOverlaps(); 
        if (movement.magnitude <= attributes.MinMagnitudeRotation) return;
        Quaternion _rot = Quaternion.LookRotation(Vector3.forward, Vector2.Perpendicular(movement));
        transform.rotation = _rot;
    }

    public void AddMovement(Vector2 _direction)
    {
        movement += _direction; 
    }
    public void AddForce(Vector2 _force)
    {
        forces += _force;
    }


    private static readonly List<Trigger> triggerBuffer = new List<Trigger>(); 
    private readonly List<Trigger> triggerOverlaps = new List<Trigger>(); 
    private void RefreshOverlaps()
    {
        // Get all overlapping colliders, then extract from physics ones and manage triggers behaviour.
        int _overlapAmount = collider.Overlap();
        triggerBuffer.Clear();
        for (int _i = 0; _i < _overlapAmount; _i++)
        {
            Collider2D _collider = collider.GetOverlapCollider(_i);
            if (_collider.isTrigger)
            {
                // Manage trigger interactions.
                if (_collider.TryGetComponent(out Trigger _trigger))
                {
                    triggerBuffer.Add(_trigger);

                    if (HasEnteredTrigger(_trigger))
                    {
                        _trigger.OnEnter(this);
                        triggerOverlaps.Add(_trigger);
                    }
                }
            }
        }

        // Exit from no more overlapping triggers.
        for (int _i = triggerOverlaps.Count; _i-- > 0;)
        {
            Trigger _trigger = triggerOverlaps[_i];

            if (HasExitedTrigger(_trigger))
            {
                _trigger.OnExit(this);
                triggerOverlaps.RemoveAt(_i);
            }
        }

        // ----- Local Methods ----- //

        bool HasEnteredTrigger(Trigger _trigger)
        {
            for (int _i = 0; _i < triggerOverlaps.Count; _i++)
            {
                Trigger _other = triggerOverlaps[_i];
                if (_trigger.Compare(_other))
                    return false;
            }

            return true;
        }

        bool HasExitedTrigger(Trigger _trigger)
        {
            for (int _i = 0; _i < triggerBuffer.Count; _i++)
            {
                Trigger _other = triggerBuffer[_i];
                if (_trigger.Compare(_other))
                    return false;
            }

            return true;
        }
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
