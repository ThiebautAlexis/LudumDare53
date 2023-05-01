using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisArrowCaller : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    private Transform playerTransform;
    private Colis colisTransform;

    void IUpdate.Update()
    {
        float _angle = Mathf.Atan2(colisTransform.transform.position.y - playerTransform.position.y, colisTransform.transform.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _angle - 90);

    }

    public void InitValues(Colis _colis)
    {
        colisTransform = _colis;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }



}
