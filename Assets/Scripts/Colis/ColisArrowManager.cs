using CoolFramework.Core;
using System.Collections.Generic;
using UnityEngine;

public class ColisArrowManager : CoolSingleton<ColisArrowManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private ColisArrow colisArrowPrefab;
    [SerializeField] private ColisArrowCaller colisCallerArrowPrefab;
    private Dictionary<Colis, ColisArrow> colisArrowDictionnary = new Dictionary<Colis, ColisArrow>();
    private Dictionary<Colis, ColisArrowCaller> colisCallerArrowDictionnary = new Dictionary<Colis, ColisArrowCaller>();


    public void CreateNewArrowForColis(Colis _colis)
    {
        if (colisArrowDictionnary.ContainsKey(_colis))
            return;

        ColisArrow _newArrow = Instantiate(colisArrowPrefab, transform);
        _newArrow.InitValues(_colis.GetColisType);

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



    public void CreateNewCallerArrowForColis(Colis _colis)
    {
        if (colisCallerArrowDictionnary.ContainsKey(_colis))
            return;

        ColisArrowCaller _newArrow = Instantiate(colisCallerArrowPrefab, transform);
        _newArrow.InitValues(_colis);

        colisCallerArrowDictionnary.Add(_colis, _newArrow);
    }

    public void RemoveCallerArrowForColis(Colis _colis)
    {
        if (!colisCallerArrowDictionnary.ContainsKey(_colis))
            return;

        ColisArrowCaller _colisArrow;
        colisCallerArrowDictionnary.TryGetValue(_colis, out _colisArrow);

        if (!_colisArrow)
            return;

        Destroy(_colisArrow.gameObject);
        colisCallerArrowDictionnary.Remove(_colis);
    }

}
