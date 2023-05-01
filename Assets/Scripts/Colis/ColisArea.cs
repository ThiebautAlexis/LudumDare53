using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisArea : Trigger
{
    [SerializeField] private ColisType colisTypeAccepted;
    public ColisType GetColisType { get { return colisTypeAccepted; } }

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        ColisCart _collidedColisCart = _movable.GetComponent<ColisCart>();

        if(!_collidedColisCart)
            return;

        Colis _colis = _collidedColisCart.GetComponentInChildren<Colis>();

        if (_colis)
            RegisterColis(_colis);
    }

    private void RegisterColis(Colis _enteredColis)
    {
        if (_enteredColis.GetColisType != colisTypeAccepted)
            return;

        _enteredColis.Delivered();
    }
}
