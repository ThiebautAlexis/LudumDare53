using CoolFramework.Core;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Colis : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float colisDurationInSeconds = 60;
    [SerializeField] private ColisTimerUI colisTimerUI;
    private float colisCurrentDurationInSeconds;
    private ColisSpawner spawnOrigin;
    public ColisArea FinalDestination { get ; private set; }

    protected override void OnInit()
    {
        base.OnInit();

        spawnOrigin = GetComponentInParent<ColisSpawner>();

        InvokeRepeating("Pickup", 0, 4);

        InvokeRepeating("Release", 2, 4);
    }

    void IUpdate.Update()
    {
        colisCurrentDurationInSeconds += Time.deltaTime;

        colisTimerUI.UpdateTimer(colisCurrentDurationInSeconds / colisDurationInSeconds);

        if (colisCurrentDurationInSeconds > colisDurationInSeconds)
            Timeout();
    }

    private void OnDrawGizmosSelected()
    {
        if (!FinalDestination)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, FinalDestination.transform.position);
    }

    public void RegisterDestination(ColisArea _destination)
    {
        FinalDestination = _destination;
    }

    public void Pickup()
    {
        if (spawnOrigin)
        {
            spawnOrigin.ColisPickedUp();
            spawnOrigin = null;
            transform.parent = null;
        }
        ColisArrowManager.Instance.CreateNewArrowForColis(this);
    }

    public void Release()
    {
        ColisArrowManager.Instance.RemoveArrowForColis(this);

    }

    public void Timeout()
    {
        SupervisorManager.Instance.RegisterStrike();
        Destroy(this.gameObject);
    }

    public void Delivered()
    {
        // temp
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Destroy(this.gameObject, 1.5f);
    }


    private void OnDestroy()
    {
        if (spawnOrigin)
            spawnOrigin.ColisPickedUp();

        ColisManager.Instance.RemoveColis();

        ColisArrowManager.Instance.RemoveArrowForColis(this);
    }
}
