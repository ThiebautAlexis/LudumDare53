using CoolFramework.Core;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisCart : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float distanceFrompreviousCart = .25f;
    [SerializeField] private Transform backColisCartJoint;
                             
    [SerializeField] private Transform linkedCartJoint;

    void IUpdate.Update()
    {
        if (!linkedCartJoint)
            return;

        transform.position = linkedCartJoint.position - (linkedCartJoint.parent.up * distanceFrompreviousCart);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(linkedCartJoint)
            Gizmos.DrawLine(transform.position, linkedCartJoint.position);
    }

    public void AttachCartTo(Transform _jointAttachment)
    {
        linkedCartJoint = _jointAttachment;
    }
}
