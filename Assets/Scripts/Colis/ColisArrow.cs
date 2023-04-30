using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColisArrow : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private ColisArea colisTargetArea;

    void IUpdate.Update()
    {
        float _angle = Mathf.Atan2(colisTargetArea.transform.position.y - playerTransform.position.y, colisTargetArea.transform.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _angle - 90);
    }

    public void InitValues(ColisArea _colisTargetArea)
    {
        playerTransform = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault().transform;
        colisTargetArea = _colisTargetArea;
    }

}
