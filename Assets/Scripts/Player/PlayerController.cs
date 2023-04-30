using UnityEngine; 
using CoolFramework.Core;
using UnityEngine.InputSystem;
using System;

public class PlayerController : CoolBehaviour, IInputUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Input;

    #region Fields and Properties
    [SerializeField] private PlayerMovable playerMovable;
    [SerializeField] private PlayerInputs playerInputs; 
    #endregion


    #region Overriden methods
    void IInputUpdate.Update() => InputUpdate();

    //void IUpdate.Update(){}

    //void IDynamicUpdate.Update(){}

    protected override void OnInit()
    {
        base.OnInit();
        playerInputs.EnableInputs();
        playerInputs.AccelerationInput.started += StartAcceleration;
        playerInputs.AccelerationInput.canceled += StopAcceleration;
    }


    protected override void DisableBehaviour()
    {
        playerInputs.AccelerationInput.started -= StartAcceleration;
        playerInputs.AccelerationInput.canceled -= StopAcceleration;
        playerInputs.DisableInputs();
        base.DisableBehaviour();
    }
    #endregion

    #region Methods
    void StartAcceleration(InputAction.CallbackContext _ctx) => playerMovable.IsInAcceleration = true; 
    void StopAcceleration(InputAction.CallbackContext _ctx) => playerMovable.IsInAcceleration = false; 

    [SerializeField]private float magnitude = 0f; 
    [SerializeField]private Vector2 direction = Vector2.zero;
    private void InputUpdate()
    {
        if (playerInputs.AccelerationInput.inProgress)
            magnitude = playerInputs.AccelerationInput.ReadValue<float>();

        direction = playerInputs.DirectionInput.ReadValue<Vector2>();

        playerMovable.AddMovement(direction.normalized);
        //playerMovable.SetMovementMagnitude(magnitude); 
    }
    #endregion 

}
