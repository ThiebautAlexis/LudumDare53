using CoolFramework.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColisManager : CoolSingleton<ColisManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private AnimationCurve colisSpawnRate;
    private List<ColisSpawner> allColisSpawners = new List<ColisSpawner>();
    private List<ColisArea> allColisAreas = new List<ColisArea>();

    private int currentColisInGame = 0;
    private float currentGameDuration = 0;


    protected override void OnInit()
    {
        base.OnInit();

        allColisSpawners.Clear();
        allColisSpawners.AddRange(FindObjectsOfType<ColisSpawner>());

        allColisAreas.Clear();
        allColisAreas.AddRange(FindObjectsOfType<ColisArea>());
    }

    void IUpdate.Update()
    {
        currentGameDuration += Time.deltaTime;

        EvaluateColisAmount();
    }

    private void EvaluateColisAmount()
    {
        int _targetColisInGame = (int)colisSpawnRate.Evaluate(currentGameDuration);

        if (currentColisInGame >= _targetColisInGame)
            return;

        // Spawns the difference between current and target colis
        for (int i = currentColisInGame; i < _targetColisInGame; i++)
        {
            SpawnColis();
        }
    }

    private void SpawnColis()
    {
        // Random list
        allColisSpawners = allColisSpawners.OrderBy(_list => System.Guid.NewGuid()).ToList();

        // Spawn colis on available spawner
        foreach (ColisSpawner _spawner in allColisSpawners)
        {
            if(_spawner.AvailableForSpawn)
            {
                Colis _spawnedColis = _spawner.SpawnColis();

                if (!_spawnedColis)
                    continue;

                _spawnedColis.RegisterDestination(allColisAreas[Random.Range(0, allColisAreas.Count)]);
                currentColisInGame++;
                break;
            }
        }
    }

    public void RemoveColis()
    {
        currentColisInGame--;
    }
}
