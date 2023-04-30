using CoolFramework.Core;
using System.Collections.Generic;
using UnityEngine;

public class ColisCart : Movable
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update | UpdateRegistration.Dynamic;

    [SerializeField] private float distanceFrompreviousCart = .25f;
    [SerializeField] private Movable linkedCart;

    protected override void ComputeVelocity()
    {
        if (linkedCart == null) return;
        currentSpeed = attributes.EvaluateSpeed(0f); 
        Vector2 _direction = (linkedCart.AttachPosition - Rigidbody.position) * currentSpeed;
        AddMovement(_direction);
        //if(movement.magnitude > 0f)
            //base.ComputeVelocity();
        //forces = Vector2.ClampMagnitude(forces, _direction.magnitude * (1 + distanceFrompreviousCart));

        //transform.position = linkedCartJoint.position - (linkedCartJoint.parent.up * distanceFrompreviousCart);
    }

    private void OnDrawGizmos()
    {
        if(linkedCart != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, linkedCart.Rigidbody.position);

            Gizmos.color = Color.green;
            Vector2 _direction = (linkedCart.AttachPosition - Rigidbody.position);
            Gizmos.DrawRay(transform.position, _direction);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Velocity);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + forces);
    }

    protected override void RefreshRotation()
    {
        if (movement.magnitude <= attributes.MinMagnitudeRotation) return;
        Quaternion _rot = Quaternion.LookRotation(Vector3.forward, Vector2.Perpendicular(linkedCart.Rigidbody.position - Rigidbody.position));
        transform.rotation = _rot;
    }

    protected override void OnAppliedVelocity(Vector2 _velocity, Vector2 _displacement, List<RaycastHit2D> _buffer){}

    public void AttachCartTo(Movable _jointAttachment)
    {
        linkedCart = _jointAttachment;
    }
}
