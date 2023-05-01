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
    [SerializeField] private SpriteRenderer triggerSprite; 

    public ColisType GetColisType { get { return currentColisType; } }
    private float colisCurrentDurationInSeconds;
    private ColisSpawner spawnOrigin;

    private ColisCartManager _manager; 

    protected override void OnInit()
    {
        base.OnInit();
        if(transform.parent != null)
            spawnOrigin = GetComponentInParent<ColisSpawner>();

        ColisArrowManager.Instance.CreateNewCallerArrowForColis(this);
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

        if (_movable.TryGetComponent(out _manager))
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        if (spawnOrigin)
        {
            spawnOrigin.ColisPickedUp();
            spawnOrigin = null;
        }
        colisCart.transform.parent = null;
        triggerCollider.enabled = false;
        triggerSprite.enabled = false;
        ColisArrowManager.Instance.RemoveCallerArrowForColis(this);
        ColisArrowManager.Instance.CreateNewArrowForColis(this);
        _manager.AddNewColisCart(colisCart);
    }

    public void Release()
    {
        ColisArrowManager.Instance.CreateNewCallerArrowForColis(this);
        triggerCollider.enabled = true; 
        ColisArrowManager.Instance.RemoveArrowForColis(this);
    }

    public void Timeout()
    {
        SupervisorManager.Instance.RegisterStrike(true);
        ColisArrowManager.Instance.RemoveArrowForColis(this);
        if(_manager) _manager.RemoveColisCart(colisCart);
        Destroy(transform.parent.gameObject, .01f);
    }

    public void Delivered()
    {
        ColisArrowManager.Instance.RemoveArrowForColis(this);
        _manager.RemoveColisCart(colisCart);
        Destroy(transform.parent.gameObject);
    }


    private void OnDestroy()
    {
        if (spawnOrigin)
            spawnOrigin.ColisPickedUp();

        ColisManager.Instance.RemoveColis();
        ColisArrowManager.Instance.RemoveCallerArrowForColis(this);
        ColisArrowManager.Instance.RemoveArrowForColis(this);
    }
}

public enum ColisType
{
    Red,
    Yellow,
    Blue
}
