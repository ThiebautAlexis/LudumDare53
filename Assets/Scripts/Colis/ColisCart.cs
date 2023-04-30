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
        currentSpeed = linkedCart.CurrentSpeed; 
        Vector2 _direction = (linkedCart.Rigidbody.position - Rigidbody.position); 
        AddForce(_direction - _direction.normalized * distanceFrompreviousCart);
        base.ComputeVelocity();
        //transform.position = linkedCartJoint.position - (linkedCartJoint.parent.up * distanceFrompreviousCart);
    }

    private void OnDrawGizmos()
    {
        if(linkedCart != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, linkedCart.Rigidbody.position);

            Gizmos.color = Color.green;
            Vector2 _direction = (linkedCart.Rigidbody.position - Rigidbody.position);
            Gizmos.DrawRay(transform.position, _direction - _direction * distanceFrompreviousCart);
        }
    }

    public void AttachCartTo(Movable _jointAttachment)
    {
        linkedCart = _jointAttachment;
    }
}
