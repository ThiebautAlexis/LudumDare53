using CoolFramework.Core;
using UnityEngine;
using UnityEngine.Events;

public class Supervisor : CoolBehaviour, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private UnityEvent onEnableSupervisor;
    [SerializeField] private UnityEvent onDisableSupervisor;

    [SerializeField] private float supervisorDetectionRange = 10;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float durationAfterExitInSeconds = 15;
    private float currentExitTimer = 0;

    private bool hasPlayerNearby = false;


    void IUpdate.Update()
    {
        if (!hasPlayerNearby)
            return;

        currentExitTimer += Time.deltaTime;

        if(currentExitTimer >= durationAfterExitInSeconds)
            DisableSupervisor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnableSupervisor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DisableSupervisor();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, supervisorDetectionRange);
    }

    private void EnableSupervisor()
    {
        hasPlayerNearby = true;
        currentExitTimer = 0;

        onEnableSupervisor?.Invoke();
    }

    private void DisableSupervisor()
    {
        hasPlayerNearby = false;
        currentExitTimer = 0;

        onDisableSupervisor?.Invoke();
    }

    public bool HasSeenPlayer()
    {
        Collider2D _objectFound = Physics2D.OverlapCircle(transform.position, supervisorDetectionRange, playerLayer);

        if(! _objectFound ) 
            return false;

        PlayerDetection _playerFound = _objectFound.GetComponent<PlayerDetection>();

        return _playerFound;
    }
}
