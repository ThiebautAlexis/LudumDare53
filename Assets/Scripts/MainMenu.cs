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

    [SerializeField] private int tutoSceneIndex = 2;
    [SerializeField] private int mainGameSceneIndex = 3; 


    public void StartTutoScene() => SimplifiedSceneManager.Instance.LoadScene(tutoSceneIndex); 

    public void StartMainGameScene() => SimplifiedSceneManager.Instance.LoadScene(mainGameSceneIndex);

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
