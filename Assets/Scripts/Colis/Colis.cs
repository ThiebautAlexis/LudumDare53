using CoolFramework.Core;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Colis : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float colisDurationInSeconds = 60;
    private float colisCurrentDurationInSeconds;
    private ColisSpawner spawnOrigin;

    protected override void OnInit()
    {
        base.OnInit();

        spawnOrigin = GetComponentInParent<ColisSpawner>();
    }

    void IUpdate.Update()
    {
        colisCurrentDurationInSeconds += Time.deltaTime;

        if (colisCurrentDurationInSeconds > colisDurationInSeconds)
            Timeout();
    }

    public void Pickup()
    {
        if (spawnOrigin)
        {
            spawnOrigin.ColisPickedUp();
            spawnOrigin = null;
            transform.parent = null;
        }
    }

    public void Release()
    {

    }

    public void Timeout()
    {

    }

    private void OnDestroy()
    {
        if (spawnOrigin)
            spawnOrigin.ColisPickedUp();

        ColisManager.Instance.RemoveColis();
    }
}
