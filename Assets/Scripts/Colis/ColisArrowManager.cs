using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisArrowManager : CoolSingleton<ColisArrowManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private ColisArrow colisArrowPrefab;
    private Dictionary<Colis, ColisArrow> colisArrowDictionnary = new Dictionary<Colis, ColisArrow>();


    public void CreateNewArrowForColis(Colis _colis)
    {
        ColisArrow _newArrow = Instantiate(colisArrowPrefab, transform);
        _newArrow.InitValues(_colis.FinalDestination);

        colisArrowDictionnary.Add(_colis, _newArrow);
    }

    public void RemoveArrowForColis(Colis _colis)
    {
        if (!colisArrowDictionnary.ContainsKey(_colis))
            return;

        ColisArrow _colisArrow;
        colisArrowDictionnary.TryGetValue(_colis, out _colisArrow);

        if (!_colisArrow)
            return;

        Destroy(_colisArrow.gameObject);
        colisArrowDictionnary.Remove(_colis);
    }

}
