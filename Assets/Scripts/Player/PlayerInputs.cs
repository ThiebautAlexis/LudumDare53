using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[CreateAssetMenu(fileName = "Player Inputs", menuName = "GGJ/Inputs")]
public class PlayerInputs : ScriptableObject
{
    #region Fields and Properties
    public InputAction AccelerationInput;
    public InputAction DirectionInput;
    #endregion

    #region Methods
    public void EnableInputs()
    {
        AccelerationInput.Enable();
        DirectionInput.Enable();
    }

    public void DisableInputs()
    {
        AccelerationInput.Disable();
        DirectionInput.Disable(); 
    }
    #endregion

}
