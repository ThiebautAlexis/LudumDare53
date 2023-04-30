using System;
using UnityEngine;

public class ColliderWrapper
{
    #region static field
    protected static readonly RaycastHit2D[] hitResults = new RaycastHit2D[1];
    #endregion

    #region Fields and Properties
    public BoxCollider2D Collider = null;
    // -----------------------

    public ColliderWrapper(BoxCollider2D _collider)
    {
        Collider = _collider;
    }
    #endregion 

    #region Physics Operations
    /// <summary>
    /// Raycasts from collider using a given velocity.
    /// </summary>
    public bool Raycast(Vector2 _direction, out RaycastHit2D _hit, float _distance, int _mask, bool _useTriggers)
    {
        Vector2 _offset = Collider.transform.rotation * Vector2.Scale(_direction, GetExtents() / 2);
        ContactFilter2D _filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = _mask,
            useTriggers = _useTriggers
        };
        int _hitAmount = Physics2D.Raycast((Vector2)Collider.bounds.center + _offset, _direction, _filter, hitResults, _distance);
        _hit = hitResults[0];

        return _hitAmount > 0;
    }

    /// <summary>
    /// Casts the collider using a given velocity.
    /// </summary>
    public int Cast(Vector2 _direction, RaycastHit2D[] _buffer, float _distance, int _mask, bool _useTriggers)
    {
        Vector2 _extents = GetExtents() - (Vector2.one * Physics2D.defaultContactOffset);
        ContactFilter2D _filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = _mask,
            useTriggers = _useTriggers
        };
        int _amount = Physics2D.BoxCast(Collider.bounds.center, _extents, Collider.transform.rotation.z, _direction, _filter, _buffer, _distance);
        // Reverse Buffer to order the collider from the farest one to the closest one (including self)
        Array.Reverse(_buffer, 0, _amount);
        return _amount;
    }

    /// <summary>
    /// Get overlapping colliders.
    /// </summary>
    public int Overlap(Collider2D[] _buffer, int _mask, bool _useTriggers)
    {
        ContactFilter2D _filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = _mask,
            useTriggers = _useTriggers
        };
        int _amount = Physics2D.OverlapBox(Collider.bounds.center, GetExtents(), Collider.transform.rotation.z, _filter, _buffer);

        return _amount;
    }
    #endregion

    #region Utility
    /// <summary>
    /// Get world-space non-rotated collider extents.
    /// </summary>
    public Vector2 GetExtents()
    {
        Vector2 _extents = Collider.transform.TransformVector(Collider.size);
        _extents.x = Mathf.Abs(_extents.x);
        _extents.y = Mathf.Abs(_extents.y);
        return _extents;
    }
    #endregion
}
