using CoolFramework.Core;
using System.Collections.Generic;

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

    public void RegisterStrike()
    {
        playerStrikes++;

        if(playerStrikes >= 3)
        {
            // end game
        }
    }
}
