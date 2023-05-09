using UnityEngine; 
using UnityEngine.InputSystem; 
using UnityEngine.UI;
using CoolFramework.Core;

public class SkipSceneManager : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Update;

    [SerializeField] private InputAction skipSceneInput;
    [SerializeField] private float inputDuration = 1.0f; 
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private Image progressImage; 

    private bool isHoldingInput = false;
    private float progressValue = 0f; 

    protected override void EnableBehaviour()
    {
        base.EnableBehaviour();
        skipSceneInput.Enable(); 
        skipSceneInput.performed += SkipToNextScene;
        skipSceneInput.started += OnInputChanged; 
        skipSceneInput.canceled += OnInputChanged; 
    }

    protected override void DisableBehaviour()
    {
        base.DisableBehaviour();
        skipSceneInput.performed -= SkipToNextScene;
        skipSceneInput.started -= OnInputChanged;
        skipSceneInput.canceled -= OnInputChanged;
        skipSceneInput.Disable(); 
    }

    private void SkipToNextScene(InputAction.CallbackContext _context)
    {
        SimplifiedSceneManager.Instance.LoadScene(nextSceneIndex);
    }

    private void OnInputChanged(InputAction.CallbackContext _context)
    {

        if (_context.started)
            isHoldingInput = true; 
        else if(_context.canceled)
        {
            isHoldingInput = false;
            progressImage.fillAmount = 0f;
            progressValue = 0f; 
        }
    }

    void IUpdate.Update()
    { 
        if(isHoldingInput)
        {
            progressValue += Time.deltaTime ;
            progressImage.fillAmount = progressValue / inputDuration; 
        }
    }
}
