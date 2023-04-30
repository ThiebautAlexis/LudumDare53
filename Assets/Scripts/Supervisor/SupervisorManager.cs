using CoolFramework.Core;
using System.Collections.Generic;
using System.Linq;

public class SupervisorManager : CoolSingleton<SupervisorManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    public Supervisor CurrentActiveSupervisor { get; private set; }  = null;

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

        if(playerStrikes >= 3)
        {
            // end game
        }
    }
}
