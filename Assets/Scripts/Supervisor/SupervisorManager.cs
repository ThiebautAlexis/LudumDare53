using CoolFramework.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SupervisorManager : CoolSingleton<SupervisorManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    public Supervisor CurrentActiveSupervisor { get; private set; } = null;

    [SerializeField] private Image firstStrikeImage;
    [SerializeField] private Image secondStrikeImage;
    [SerializeField] private Image thirdStrikeImage;
    [SerializeField] private Sprite supervisorSprite;

    private int playerStrikes = 0;

    public void RegisterCurrentActiveSupervisor(Supervisor _supervisor) => CurrentActiveSupervisor = _supervisor;

    public void RemoveCurrentSupervisor() => CurrentActiveSupervisor = null;

    /// <summary>
    /// Call if there is an error made
    /// </summary>
    /// <param name="_isForceStrike">true if the strike should pass no matter the sight of any Supervisor</param>
    public void RegisterStrike(bool _isForceStrike = false)
    {
        if (!_isForceStrike && !CurrentActiveSupervisor && !CurrentActiveSupervisor.HasPlayerNearby)
            return;

        playerStrikes++;

        SoundManager.Instance.PlaySound("Strike", this.gameObject);
        TalkieWalkie.Instance.Display(supervisorSprite, 1.5f, true);
        ScoreManager.Instance.BreakCombo();

        switch (playerStrikes)
        {
            case 1:
                firstStrikeImage.color = Color.white;
                break;
            case 2:
                secondStrikeImage.color = Color.white;
                break;
            case 3:
                thirdStrikeImage.color = Color.white;
                break;
        }

        if (playerStrikes >= 3)
        {
            GameManager.Instance.StrikeEnding();
        }
    }
}
