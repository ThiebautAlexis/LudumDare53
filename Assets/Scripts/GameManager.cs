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
                SoundManager.Instance.PlaySound("Rang DD", this.gameObject);
                break;
            case Ranks.CD:
                endingAnimator.SetTrigger("CD");
                SoundManager.Instance.PlaySound("Rang CD", this.gameObject);
                break;
            case Ranks.BD:
                endingAnimator.SetTrigger("BD");
                SoundManager.Instance.PlaySound("Rang BD", this.gameObject);
                break;
            case Ranks.AD:
                endingAnimator.SetTrigger("AD");
                SoundManager.Instance.PlaySound("Rang AD", this.gameObject);
                break;
            case Ranks.SD:
                endingAnimator.SetTrigger("SD");
                SoundManager.Instance.PlaySound("Rang SD", this.gameObject);
                break;
            case Ranks.SSD:
                endingAnimator.SetTrigger("SSD");
                SoundManager.Instance.PlaySound("Rang SSD", this.gameObject);
                break;
            case Ranks.DK:
                endingAnimator.SetTrigger("DK");
                SoundManager.Instance.PlaySound("Rang DK", this.gameObject);
                break;
        }
    }

    public void StrikeEnding()
    {
        endingAnimator.SetTrigger("Strike");
    }
}
