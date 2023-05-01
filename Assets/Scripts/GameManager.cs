using CoolFramework.Core;
using CoolFramework.SceneManagement;
using System;
using TMPro;
using UnityEngine;

public class GameManager : CoolSingleton<GameManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init |�UpdateRegistration.Update;

    [SerializeField] private float maxGameTimeInSeconds;
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int mainMenuSceneIndex = 1;

    private float currentGametime = 0;
    private bool isGameEnded = false;

    protected override void OnInit()
    {
        base.OnInit();

        currentGametime = maxGameTimeInSeconds;
    }

    void IUpdate.Update()
    {
        if (isGameEnded)
            return;

        currentGametime -= Time.deltaTime;

        TimeSpan _timespan = TimeSpan.FromSeconds(currentGametime);
        timeText.text = _timespan.ToString(@"mm\:ss");

        if (currentGametime <= 0)
            TimeEnding();
    }

    private void TimeEnding()
    {
        switch (ScoreManager.Instance.CurrentRank)
        {
            case Ranks.DD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang DD", this.gameObject);
                break;
            case Ranks.CD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang CD", this.gameObject);
                break;
            case Ranks.BD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang BD", this.gameObject);
                break;
            case Ranks.AD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang AD", this.gameObject);
                break;
            case Ranks.SD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang SD", this.gameObject);
                break;
            case Ranks.SSD:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang SSD", this.gameObject);
                break;
            case Ranks.DK:
                endingAnimator.SetTrigger("End");
                SoundManager.Instance.PlaySound("Rang DK", this.gameObject);
                break;
        }
        isGameEnded = true;

        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        ScoreManager.Instance.BlockInfos();
    }

    public void StrikeEnding()
    {
        isGameEnded = true;

        endingAnimator.SetTrigger("Strike");
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        ScoreManager.Instance.BlockInfos();
    }

    public void LoadMainMenu()
    {
        SimplifiedSceneManager.Instance.LoadScene(mainMenuSceneIndex);
    }
}
