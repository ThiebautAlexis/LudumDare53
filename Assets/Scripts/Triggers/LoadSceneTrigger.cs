using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : Trigger
{
    [SerializeField] private int nextSceneIndex = 1; 
    [SerializeField] private float waitingTime = 1; 
    private bool hasBeenActivated = false; 

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);
        if(!hasBeenActivated && _movable is PlayerMovable _playerMovable)
        {
            hasBeenActivated = true; 
            StartCoroutine(WaitBeforeSceneLoading()); 

            IEnumerator WaitBeforeSceneLoading()
            {
                yield return new WaitForSeconds(waitingTime); 
                SimplifiedSceneManager.Instance.LoadScene(nextSceneIndex); 
            }
        }
    }
}
