using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttributesTrigger : Trigger
{
    [SerializeField] private MovableAttributes attributes;

    public override void OnEnter(Movable _movable)
    {
        if(_movable is PlayerMovable _playerMovable)
        {
            _playerMovable.SetAttributes(attributes);
            this.enabled = false;
        }
    }
}
