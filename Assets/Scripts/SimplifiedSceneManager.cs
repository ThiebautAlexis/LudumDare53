using System.Collections;
using UnityEngine;
using CoolFramework.Core;
using UnityEngine.SceneManagement; 

public class SimplifiedSceneManager : CoolSingleton<SimplifiedSceneManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private int firstLoadedSceneIndex = 1;
    [SerializeField] private bool loadFirstScene = true; 
    private int currentLoadedSceneIndex;

    protected override void OnInit()
    {
        base.OnInit();
        if(loadFirstScene)
        {
            currentLoadedSceneIndex = firstLoadedSceneIndex;
            StartCoroutine(SimpleSceneLoading(currentLoadedSceneIndex)); 
        }
    }

    public void LoadScene(int _sceneIndex)
    {
        StartCoroutine(ReplaceAndLoadScene(_sceneIndex)); 
    }

    private IEnumerator ReplaceAndLoadScene(int _sceneIndex)
    {
        yield return SceneManager.UnloadSceneAsync(currentLoadedSceneIndex);

        currentLoadedSceneIndex = _sceneIndex;

        yield return SceneManager.LoadSceneAsync(currentLoadedSceneIndex, LoadSceneMode.Additive); 
    }

    private IEnumerator SimpleSceneLoading(int _sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive); 
    }
}
