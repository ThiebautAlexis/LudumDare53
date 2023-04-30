using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisArea : Trigger
{

    public override void OnEnter(Movable _movable)
    {
        Colis _collidedColis = _movable.GetComponent<Colis>();

        if (_collidedColis)
            RegisterColis(_collidedColis);
    }

    private void RegisterColis(Colis _enteredColis)
    {
        if (_enteredColis.FinalDestination != this)
            return;

        _enteredColis.Release();
        _enteredColis.Delivered();
    }
}
