using System.Collections.Generic;
using UnityEngine;

public class ColisSpawner : MonoBehaviour
{
    public bool AvailableForSpawn { get; private set; } = true;

    [SerializeField] private Vector2 colisSpawnPosition;

    [SerializeField] private List<Colis> allColis;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere((Vector2)transform.position + colisSpawnPosition, .2f);
    }


    public void SpawnColis()
    {
        Colis _colisToSpawn = allColis[Random.Range(0, allColis.Count)];

        if (!_colisToSpawn)
            return;

        Colis _spawnedColis = Instantiate(_colisToSpawn, colisSpawnPosition, Quaternion.identity);
        _spawnedColis.transform.parent = transform;

        AvailableForSpawn = false;
    }

    public void ColisPickedUp()
    {
        AvailableForSpawn = true;
    }
}
