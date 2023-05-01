using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColisArrow : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private Transform playerTransform;
    private List<ColisArea> correspondingAreas = new List<ColisArea>();

    void IUpdate.Update()
    {
        ColisArea _closestArea = null;
        float _smallestDistance = Mathf.Infinity;
        foreach (ColisArea _area in correspondingAreas)
        {
            float _distanceFromArea = Vector2.Distance(playerTransform.position, _area.transform.position);

            if (_distanceFromArea >= _smallestDistance)
                continue;

            _smallestDistance = _distanceFromArea;
            _closestArea = _area;
        }

        if (!_closestArea)
            return;

        float _angle = Mathf.Atan2(_closestArea.transform.position.y - playerTransform.position.y, _closestArea.transform.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _angle - 90);
    }

    public void InitValues(ColisType _colisType)
    {
        playerTransform = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault().transform;
        correspondingAreas.Clear();
        correspondingAreas.AddRange(FindObjectsOfType<ColisArea>().Where(_area => _area.GetColisType == _colisType));
    }

}
