using CoolFramework.Core;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Colis : Trigger, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private float colisDurationInSeconds = 60;
    [SerializeField] private ColisTimerUI colisTimerUI;
    [SerializeField] private ColisCart colisCart;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private ColisType currentColisType;
    public ColisType GetColisType { get { return currentColisType; } }
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

        colisTimerUI.UpdateTimer(colisCurrentDurationInSeconds / colisDurationInSeconds);

        if (colisCurrentDurationInSeconds > colisDurationInSeconds)
            Timeout();
    }

    public override void OnEnter(Movable _movable)
    {
        base.OnEnter(_movable);

        if (!_movable.GetComponent<PlayerController>())
            return;

        Pickup();
    }

    public void Pickup()
    {
        if (spawnOrigin)
        {
            spawnOrigin.ColisPickedUp();
            spawnOrigin = null;
            colisCart.transform.parent = null;
            triggerCollider.enabled = false;
        }
        ColisArrowManager.Instance.CreateNewArrowForColis(this);
        ColisCartManager.Instance.AddNewColisCart(colisCart);

        //Invoke("Release", 5);
    }

    public void Release()
    {
        triggerCollider.enabled = true;
        ColisArrowManager.Instance.RemoveArrowForColis(this);
    }

    public void Timeout()
    {
        SupervisorManager.Instance.RegisterStrike(true);
        ColisArrowManager.Instance.RemoveArrowForColis(this);
        ColisCartManager.Instance.RemoveColisCart(colisCart);
        Destroy(transform.parent.gameObject, .01f);
    }

    public void Delivered()
    {
        ColisArrowManager.Instance.RemoveArrowForColis(this);
        ColisCartManager.Instance.RemoveColisCart(colisCart);
        Destroy(transform.parent.gameObject);
    }


    private void OnDestroy()
    {
        if (spawnOrigin)
            spawnOrigin.ColisPickedUp();

        ColisManager.Instance.RemoveColis();

        ColisArrowManager.Instance.RemoveArrowForColis(this);
    }
}

public enum ColisType
{
    Red,
    Yellow,
    Blue
}
