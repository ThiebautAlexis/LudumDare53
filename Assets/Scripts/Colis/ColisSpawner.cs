using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ColisSpawner : Trigger
{
    public bool AvailableForSpawn { get; private set; } = true;

    [SerializeField] private Vector2 colisSpawnPosition;

    [SerializeField] private List<ColisCart> allColis;

    public bool HasPlayerNearby { get; private set; } = false;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere((Vector2)transform.position + colisSpawnPosition, .2f);
    }


    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        if(_movable.GetComponent<PlayerController>())
            HasPlayerNearby = true;
    }

    public override void OnExit(Movable _movable)
    {
        base.OnExit(_movable);

        if (_movable.GetComponent<PlayerController>())
            HasPlayerNearby = false;
    }

    public ColisCart SpawnColis()
    {
        ColisCart _colisToSpawn = allColis[Random.Range(0, allColis.Count)];

        if (!_colisToSpawn)
            return null;

        ColisCart _spawnedColis = Instantiate(_colisToSpawn, (Vector2)transform.position + colisSpawnPosition, Quaternion.identity);
        _spawnedColis.transform.parent = transform;

        AvailableForSpawn = false;

        return _spawnedColis;
    }

    public void ColisPickedUp()
    {
        AvailableForSpawn = true;
    }
}
