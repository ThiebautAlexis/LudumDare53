using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem 
{
    #region Global Members
    /// <summary>
    /// Maximum amount of recursive loop calculs
    /// to be used for collision performance.
    /// </summary>
    public const int MaxCollisionCalculRecursivity = 3;

    // -----------------------

    protected static readonly List<RaycastHit2D> buffer = new List<RaycastHit2D>(3);
    protected readonly Movable movable = null;

    /// <summary>
    /// Default empty buffer when no collision has been performed.
    /// </summary>
    public static List<RaycastHit2D> DefaultBuffer
    {
        get
        {
            buffer.Clear();
            return buffer;
        }
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Creates a collision system to be associated with a <see cref="Movable"/>.
    /// </summary>
    public CollisionSystem(Movable _movable)
    {
        movable = _movable;
    }
    #endregion

    #region Collision Calculs
    /// <summary>
    /// Performs collisions on associated <see cref="Movable"/>
    /// from a certain velocity, and move its rigidbody accordingly.
    /// </summary>
    /// <returns>All hits encountered the object had collision with.</returns>
    public List<RaycastHit2D> PerformCollisions(Vector2 _movement, Vector2 _forces)
    {
        buffer.Clear();
        ComputeVelocity(ref _movement);

        if (_movement != Vector2.zero)
        {
            // Calculate movement collisions.
            CalculateCollisions(_movement);
        }

        if (_forces != Vector2.zero)
        {
            // Calculate forces collisions.
            CalculateCollisions(_forces);
        }
        return buffer;
    }

    private void CalculateCollisions(Vector2 _velocity)
    {
        Rigidbody2D _rigidbody = movable.Rigidbody;
        MovableCollider _collider = movable.Collider;

        int _amount = _collider.Cast(_velocity, out float _distance);
        int _start = buffer.Count;
        if (_amount > 0)
        {
            RegisterCastInfos(_amount, _collider);
            for (int i = _start; i < _start + _amount; i++)
            {

                if (buffer[i].normal.x != 0) _velocity.x = 0f;
                if (buffer[i].normal.y != 0) _velocity.y = 0f;
                continue;
            }
        }
        _rigidbody.position += _velocity;
    }
    #endregion

    #region Additional Calculs
    /// <summary>
    /// Compute associated <see cref="Movable"/> velocity before collisions.
    /// </summary>
    public virtual void ComputeVelocity(ref Vector2 _movement) { }
    #endregion

    #region Utility
    protected void RegisterCastInfo(RaycastHit2D _hit)
    {
        buffer.Add(_hit);
    }

    protected void RegisterCastInfos(int _amount, MovableCollider _collider)
    {
        for (int _i = 0; _i < _amount; _i++)
        {
            buffer.Add(_collider.GetCastHit(_i));
        }
    }
    #endregion
}
