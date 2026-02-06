using System;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Enemy enemyTarget;
    private Player player;
    
    [SerializeField] private float attackingCooldownMax;
    private float attackingCooldownTimer;
    [SerializeField] private float targetUpdateInterval;
    private float targetUpdateTimer;
    
    [SerializeField] private float damage;

    [SerializeField]
    private float detectionRadius;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Player.Instance;
    }

    private void Update()
    {
        attackingCooldownTimer -= Time.deltaTime;
        targetUpdateTimer -= Time.deltaTime;

        HandleTargetUpdating();
        
        if (!enemyTarget)
        {
            StayNearPlayer();
        }
        else
        {
            ChaseAndAttackTarget();
        }
    }

    private void StayNearPlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }

    private void ChaseAndAttackTarget()
    {
        navMeshAgent.SetDestination(enemyTarget.transform.position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Attack();
        }
    }

    private void HandleTargetUpdating()
    {
        if (targetUpdateTimer > 0) return;
        
        // Target logic
    }

    private void Attack()
    {
        if (attackingCooldownTimer <= 0)
        {
            enemyTarget.TakeDamage(damage);
            attackingCooldownTimer = attackingCooldownMax;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyTarget != null) return;

        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemyTarget = enemy;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyTarget.gameObject == other.gameObject)
        {
            enemyTarget = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}