using CoolFramework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColisCartManager : CoolSingleton<ColisCartManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private Movable playerMovable;
    [SerializeField] private List<ColisCart> allLinkedColisCart = new List<ColisCart>();

    protected override void OnInit()
    {
        base.OnInit();

        playerInputs.DropCartInput.started += ReleaseLastColisCart;
    }

    public void AddNewColisCart(ColisCart _colisCart)
    {
        if (allLinkedColisCart.Count > 0)
            _colisCart.AttachCartTo(allLinkedColisCart[allLinkedColisCart.Count - 1]);
        else
            _colisCart.AttachCartTo(playerMovable);

        allLinkedColisCart.Add(_colisCart);
    }

    public void RemoveLastColisCart()
    {
        if (allLinkedColisCart.Count <= 0)
            return;

        allLinkedColisCart[allLinkedColisCart.Count - 1].AttachCartTo(null);
        allLinkedColisCart.RemoveAt(allLinkedColisCart.Count - 1);
    }

    public void RemoveColisCart(ColisCart _colisCart)
    {
        if (!allLinkedColisCart.Contains(_colisCart))
            return;

        if (allLinkedColisCart.Count <= 1)
        {
            allLinkedColisCart.RemoveAt(0);
            return;
        }

        int _colisCartIndex = allLinkedColisCart.IndexOf(_colisCart);

        allLinkedColisCart[_colisCartIndex].AttachCartTo(null);

        allLinkedColisCart.RemoveAt(_colisCartIndex);

        if (_colisCartIndex == 0)
            allLinkedColisCart[_colisCartIndex].AttachCartTo(playerMovable);
        else if(_colisCartIndex < allLinkedColisCart.Count - 1)
            allLinkedColisCart[_colisCartIndex].AttachCartTo(allLinkedColisCart[_colisCartIndex - 1]);
    }

    void ReleaseLastColisCart(InputAction.CallbackContext _ctx)
    {
        if (allLinkedColisCart.Count <= 0)
            return;

        allLinkedColisCart[allLinkedColisCart.Count - 1].ReleaseCart();

        allLinkedColisCart[allLinkedColisCart.Count - 1].AddForce(playerMovable.transform.forward * 5);

        allLinkedColisCart.RemoveAt(allLinkedColisCart.Count - 1);
    }
}
