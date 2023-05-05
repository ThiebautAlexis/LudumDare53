using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColisArrowCaller : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private Image colisDurationIndicator;

    private Transform playerTransform;
    private Colis colisTransform;

    void IUpdate.Update()
    {
        if (!playerTransform)
            return;

        float _angle = Mathf.Atan2(colisTransform.transform.position.y - playerTransform.position.y, colisTransform.transform.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _angle - 90);

    }

    public void InitValues(Colis _colis)
    {
        colisTransform = _colis;
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void SetColisDuration(float _fillAmount, Color _color)
    {
        colisDurationIndicator.fillAmount = _fillAmount;
        colisDurationIndicator.color = _color;
    }

}
