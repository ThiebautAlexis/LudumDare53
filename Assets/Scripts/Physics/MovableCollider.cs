using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovableCollider 
{
    /// <summary>
    /// Maximum distance compared to cast first hit collider
    /// to be considered as a valid hit.
    /// </summary>
    public const float MaxCastDifferenceDetection = .001f;


    #region Global Members
    [SerializeField] private BoxCollider2D collider = null;
    private ColliderWrapper wrapper = null;

    /// <summary>
    /// Default mask used for collision detections.
    /// </summary>
    public int CollisionMask = 0;

    public BoxCollider2D Collider
    {
        get => collider;
        set
        {
            collider = value;
            Initialize();
        }
    }

    // -----------------------

    /// <summary>
    /// World-space collider bounding box center.
    /// </summary>
    public Vector2 Center => collider.bounds.center;

    /// <summary>
    /// World-space non-rotated collider extents.
    /// </summary>
    public Vector2 Extents => wrapper.GetExtents();

    /// <summary>
    /// Collider Offset.
    /// </summary>
    public Vector2 Offset => collider.offset;
    #endregion

    #region Initialization
    /// <summary>
    /// Initializes this <see cref="DreadfulCollider"/>.
    /// Always call initialization before any use,
    /// preferably on Start or Awake.
    /// </summary>
    public void Initialize()
    {
        Initialize(CollisionMask);
    }

    /// <summary>
    /// Initializes this <see cref="DreadfulCollider"/>.
    /// Always call initialization before any use,
    /// preferably on Start or Awake.
    /// </summary>
    /// <param name="_collisionMask">Default mask to be used for collider collision detections.</param>
    public void Initialize(int _collisionMask)
    {
        CollisionMask = _collisionMask;
        wrapper = new ColliderWrapper(Collider);
    }
    #endregion

    #region Cast 
    private static readonly RaycastHit2D[] castBuffer = new RaycastHit2D[8];

    /// <summary>
    /// Get hit informations from last cast at specified index.
    /// Note that last raycast is from the whole game, not specific
    /// to this collider.
    /// </summary>
    /// <param name="_index">Index to get hit details at.
    /// Should be used in association with informations from Cast methods.</param>
    /// <returns>Details informations about hit at specified index
    /// from last cast.</returns>
    public RaycastHit2D GetCastHit(int _index)
    {
        return castBuffer[_index];
    }

    /// <summary>
    /// Casts this collider using a given velocity.
    /// 
    /// Give first hit distance from object.
    /// </summary>
    /// <param name="_velocity">Velocity used for cast.</param>
    /// <param name="_distance">Traveled distance before first collision.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>True if collided with something on the way, false otherwise.</returns>
    public bool DoCast(Vector2 _velocity, out float _distance, bool _useTriggers = false)
    {
        int _amount = Cast(_velocity, out RaycastHit2D _hit, _velocity.magnitude, CollisionMask, _useTriggers);

        _distance = _hit.distance;
        return _amount > 0;
    }

    /// <summary>
    /// Casts this collider using a given velocity.
    /// 
    /// Give detailed informations about main trajectory collision.
    /// </summary>
    /// <param name="_velocity">Velocity used for cast.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>True if collided with something on the way, false otherwise.</returns>
    public bool DoCast(Vector2 _velocity, out RaycastHit2D _hit, bool _useTriggers = false)
    {
        int _amount = Cast(_velocity, out _hit, _velocity.magnitude, CollisionMask, _useTriggers);
        return _amount > 0;
    }

    /// <summary>
    /// Casts this collider using a given velocity.
    /// 
    /// Give detailed informations about main trajectory collision.
    /// </summary>
    /// <param name="_velocity">Velocity used for cast.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_mask"><see cref="LayerMask"/> to use for collision detection.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>True if collided with something on the way, false otherwise.</returns>
    public bool DoCast(Vector2 _velocity, out RaycastHit2D _hit, int _mask, bool _useTriggers = false)
    {
        int _amount = Cast(_velocity, out _hit, _velocity.magnitude, _mask, _useTriggers);
        return _amount > 0;
    }

    /// <summary>
    /// Casts this collider in a given direction.
    /// 
    /// Give detailed informations about main trajectory collision.
    /// </summary>
    /// <param name="_direction">Cast direction.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_distance">Maximum cast distance.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>True if collided with something on the way, false otherwise.</returns>
    public bool DoCast(Vector2 _direction, out RaycastHit2D _hit, float _distance, bool _useTriggers = false)
    {
        int _amount = Cast(_direction, out _hit, _distance, CollisionMask, _useTriggers);
        return _amount > 0;
    }

    /// <summary>
    /// Casts this collider in a given direction.
    /// 
    /// Give detailed informations about main trajectory collision.
    /// </summary>
    /// <param name="_direction">Cast direction.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_distance">Maximum cast distance.</param>
    /// <param name="_mask"><see cref="LayerMask"/> to use for collision detection.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>True if collided with something on the way, false otherwise.</returns>
    public bool DoCast(Vector2 _direction, out RaycastHit2D _hit, float _distance, int _mask, bool _useTriggers = false)
    {
        int _amount = Cast(_direction, out _hit, _distance, _mask, _useTriggers);
        return _amount > 0;
    }

    /// <summary>
    /// Casts this collider using a given velocity.
    /// 
    /// Indicates trajectory consistent hits amount,
    /// and first hit distance from object.
    /// </summary>
    /// <param name="_velocity">Velocity used for cast.</param>
    /// <param name="_distance">Traveled distance before first collision.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>Total trajectory consistent hit amount.</returns>
    public int Cast(Vector2 _velocity, out float _distance, bool _useTriggers = false)
    {
        int _amount = Cast(_velocity, out RaycastHit2D _hit, _velocity.magnitude, CollisionMask, _useTriggers);

        _distance = _hit.distance;
        return _amount;
    }

    /// <summary>
    /// Casts this collider using a given velocity.
    /// 
    /// Indicates trajectory consistent hits amount,
    /// and give detailed informations about the main one.
    /// </summary>
    /// <param name="_velocity">Velocity used for cast.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>Total trajectory consistent hit amount.</returns>
    public int Cast(Vector2 _velocity, out RaycastHit2D _hit, bool _useTriggers = false)
    {
        float _distance = _velocity.magnitude;
        int _amount = Cast(_velocity, out _hit, _distance, CollisionMask, _useTriggers);

        return _amount;
    }

    /// <summary>
    /// Casts this collider in a given direction.
    /// 
    /// Indicates trajectory consistent hits amount,
    /// and give detailed informations about the main one.
    /// </summary>
    /// <param name="_direction">Cast direction.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_distance">Maximum cast distance.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>Total trajectory consistent hit amount.</returns>
    public int Cast(Vector2 _direction, out RaycastHit2D _hit, float _distance, bool _useTriggers = false)
    {
        int _amount = Cast(_direction, out _hit, _distance, CollisionMask, _useTriggers);
        return _amount;
    }

    /// <summary>
    /// Casts this collider in a given direction.
    /// 
    /// Indicates trajectory consistent hits amount,
    /// and give detailed informations about the main one.
    /// </summary>
    /// <param name="_direction">Cast direction.</param>
    /// <param name="_hit">Main trajectory hit detailed informations.</param>
    /// <param name="_distance">Maximum cast distance.</param>
    /// <param name="_mask"><see cref="LayerMask"/> to use for collision detection.</param>
    /// <param name="_useTriggers">How should the cast interact with triggers?</param>
    /// <returns>Total trajectory consistent hit amount.</returns>
    public int Cast(Vector2 _direction, out RaycastHit2D _hit, float _distance, int _mask, bool _useTriggers = false)
    {
        _direction.Normalize();
        _distance += Physics2D.defaultContactOffset * 2;

        int _amount = wrapper.Cast(_direction, castBuffer, _distance, _mask, _useTriggers);
        if (_amount > 0)
        {
            // Remove this object collider if detected.
            if (castBuffer[_amount - 1].collider == collider)
            {
                _amount--;
                if (_amount == 0)
                {
                    _hit = GetDefaultHit();
                    return 0;
                }
            }

#if DEBUG_LOGS
                // Debug thing. Should be remove one day.
                for (int _i = 0; _i < _amount; _i++)
                {
                    if(castBuffer[_i].collider == collider)
                        collider.LogWarning($"Found Collider => {_i+1}/{_amount} is {castBuffer[_i].collider.name}");
                }
#endif
            PhysicsUtility.SortRaycastHitByDistance(castBuffer, _amount);

            _hit = castBuffer[0];
            _hit.distance = Mathf.Max(0, _hit.distance - Physics.defaultContactOffset);

            for (int _i = 1; _i < _amount; _i++)
            {
                if (castBuffer[_i].distance > (_hit.distance + MaxCastDifferenceDetection))
                    return _i;
            }
        }
        else
            _hit = GetDefaultHit();

        return _amount;

        // ----- Local Method ----- //

        RaycastHit2D GetDefaultHit()
        {
            RaycastHit2D _hit = new RaycastHit2D();
            _hit.distance = _distance - Physics.defaultContactOffset;

            return _hit;
        }
    }
    #endregion

    #region Overlap
    private static readonly Collider2D[] overlapBuffer = new Collider2D[8];

    /// <summary>
    /// Get collider at specified index from last overlap.
    /// Note that last overlap is from the whole game, not specific
    /// to this collider.
    /// </summary>
    /// <param name="_index">Index to get collider at.
    /// Should be used in association with amount from Overlap methods.</param>
    /// <returns>Colliders at specified index from last overlap.</returns>
    public Collider2D GetOverlapCollider(int _index)
    {
        return overlapBuffer[_index];
    }

    // -----------------------

    /// <summary>
    /// Get informations about overlapping colliders.
    /// </summary>
    /// <param name="_useTriggers">How should the overlap interact with triggers?</param>
    /// <returns>Amount of overlapping colliders.</returns>
    public int Overlap(bool _useTriggers = true)
    {
        int _amount = wrapper.Overlap(overlapBuffer, CollisionMask, _useTriggers);
        return _amount;
    }

    /// <summary>
    /// Get informations about overlapping colliders.
    /// </summary>
    /// <param name="_mask"><see cref="LayerMask"/> to use for collision detection.</param>
    /// <param name="_useTriggers">How should the overlap interact with triggers?</param>
    /// <returns>Amount of overlapping colliders.</returns>
    public int Overlap(int _mask, bool _useTriggers = true)
    {
        int _amount = wrapper.Overlap(overlapBuffer, _mask, _useTriggers);
        return _amount;
    }
    #endregion 

}
