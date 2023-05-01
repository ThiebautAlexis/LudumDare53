using CoolFramework.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject defaultCreditObjectSelected;
    [SerializeField] private GameObject defaultMenuObjectSelected;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private SceneBundle tutorialSceneBundle;
    [SerializeField] private SceneBundle mainMenuSceneBundle;


    public void StartTutoScene()
    {
        StartCoroutine(CoolSceneManager.Instance.LoadSceneBundle(tutorialSceneBundle, UnityEngine.SceneManagement.LoadSceneMode.Additive));
        StartCoroutine(CoolSceneManager.Instance.UnloadSceneBundle(mainMenuSceneBundle, UnityEngine.SceneManagement.UnloadSceneOptions.None));
    }

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defaultCreditObjectSelected);
    }

    public void HideCredits()
    {
        creditsMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(defaultMenuObjectSelected);
    }

    public void QuitGame() => Application.Quit();


}
