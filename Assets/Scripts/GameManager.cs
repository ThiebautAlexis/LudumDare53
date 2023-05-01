using CoolFramework.Core;
using UnityEngine;

public class GameManager : CoolSingleton<GameManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float maxGameTimeInSeconds;
    [SerializeField] private Animator endingAnimator;

    private float currentGametime = 0;

    void IUpdate.Update()
    {
        currentGametime += Time.deltaTime;

        if (currentGametime > maxGameTimeInSeconds)
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
