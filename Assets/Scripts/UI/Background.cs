using CoolFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : CoolSingleton<Background>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private Canvas background;

    public void SetCamera()
    {
        background.worldCamera = Camera.main;
        background.planeDistance = 100;
    }
}
