using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoolFramework.Core; 

public class CameraController : CoolBehaviour, ILateUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Late;

    [SerializeField] private PlayerMovable player;
    Vector3 _position = Vector3.zero; 

    void ILateUpdate.Update()
    {
        _position.Set(player.transform.position.x, player.transform.position.y, -1f) ;
        transform.position = _position;

    }
}
