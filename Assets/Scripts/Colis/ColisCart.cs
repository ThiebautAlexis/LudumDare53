using CoolFramework.Core;
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
        Vector2 _direction = (linkedCart.AttachPosition - Rigidbody.position);
        AddForce(_direction);
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

    public void AttachCartTo(Movable _jointAttachment)
    {
        linkedCart = _jointAttachment;
    }
}
