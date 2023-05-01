using CoolFramework.Core;
using System;
using TMPro;
using UnityEngine;

public class GameManager : CoolSingleton<GameManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float maxGameTimeInSeconds;
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private TMP_Text timeText;

    private float currentGametime = 0;

    protected override void OnInit()
    {
        base.OnInit();

        currentGametime = maxGameTimeInSeconds;
    }

    void IUpdate.Update()
    {
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
                endingAnimator.SetTrigger("DD");
                break;
            case Ranks.CD:
                endingAnimator.SetTrigger("CD");
                break;
            case Ranks.BD:
                endingAnimator.SetTrigger("BD");
                break;
            case Ranks.AD:
                endingAnimator.SetTrigger("AD");
                break;
            case Ranks.SD:
                endingAnimator.SetTrigger("SD");
                break;
            case Ranks.SSD:
                endingAnimator.SetTrigger("SSD");
                break;
            case Ranks.DK:
                endingAnimator.SetTrigger("DK");
                break;
        }
    }

    public void StrikeEnding()
    {
        endingAnimator.SetTrigger("Strike");
    }
}
