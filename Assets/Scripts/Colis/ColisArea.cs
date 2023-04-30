using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colis _collidedColis = collision.GetComponent<Colis>();

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
