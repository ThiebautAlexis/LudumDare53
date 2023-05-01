using CoolFramework.Core;
using CoolFramework.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : CoolBehaviour
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject defaultCreditObjectSelected;
    [SerializeField] private GameObject defaultMenuObjectSelected;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private SceneBundle tutorialSceneBundle;
    [SerializeField] private SceneBundle mainMenuSceneBundle;

    [SerializeField] private int tutoSceneIndex = 2;
    [SerializeField] private int mainGameSceneIndex = 3;


    protected override void OnInit()
    {
        base.OnInit();

        SoundManager.Instance.PlaySound("Drift_King_Ecran_Titre", this.gameObject);
    }

    public void StartTutoScene() => SimplifiedSceneManager.Instance.LoadScene(tutoSceneIndex); 

    public void StartMainGameScene() => SimplifiedSceneManager.Instance.LoadScene(mainGameSceneIndex);

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(defaultCreditObjectSelected);
    }

    public void HideCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defaultMenuObjectSelected);
    }

    public void QuitGame() => Application.Quit();


}
