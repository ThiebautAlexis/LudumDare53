using CoolFramework.Core;
using System.Collections.Generic;
using System.Linq;

public class SupervisorManager : CoolSingleton<SupervisorManager>
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

    private List<Supervisor> allSupervisors = new List<Supervisor>();

    private int playerStrikes = 0;

    protected override void OnInit()
    {
        base.OnInit();

        allSupervisors.Clear();
        allSupervisors.AddRange(FindObjectsOfType<Supervisor>());
    }

    /// <summary>
    /// Call if there is an error made
    /// </summary>
    /// <param name="_isForceStrike">true if the strike should pass no matter the sight of any Supervisor</param>
    public void RegisterStrike(bool _isForceStrike = false)
    {
        if (!_isForceStrike && !allSupervisors.Any(_supervisor => _supervisor.HasPlayerNearby))
            return;

        playerStrikes++;

        if(playerStrikes >= 3)
        {
            // end game
        }
    }
}
