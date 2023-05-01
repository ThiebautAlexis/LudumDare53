using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPackageTrigger : Trigger
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ColisCart spawnedObject;

    public override void OnEnter(Movable _movable)
    {
        if(_movable.TryGetComponent(out PlayerController _controller))
        {
            Instantiate(spawnedObject, spawnPosition.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
