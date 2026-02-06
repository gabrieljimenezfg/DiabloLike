using System;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Enemy enemyTarget;

    [SerializeField] private float attackingCooldownMax;
    private float attackingCooldownTimer;
    [SerializeField] private float targetUpdateInterval;
    private float targetUpdateTimer;

    [SerializeField] private float damage;

    [SerializeField] private float detectionRadius;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void ChaseAndAttackTarget()
    {
        ChaseEnemy();

        if (CheckIfWithinAttackRange())
        {
            Attack();
        }
    }

    private bool CheckIfWithinAttackRange()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    private void ChaseEnemy()
    {
        navMeshAgent.SetDestination(enemyTarget.transform.position);
    }

    private void HandleTargetUpdating()
    {
        if (targetUpdateTimer > 0) return;
        
        targetUpdateTimer = targetUpdateInterval;

        var closestEnemy = FindClosestEnemy();

        if (!closestEnemy) return;

        if (!enemyTarget)
        {
            enemyTarget = closestEnemy;
        }
        else
        {
            float currentDistanceSqr = (transform.position - enemyTarget.transform.position).sqrMagnitude;
            float newDistanceSqr = (transform.position - closestEnemy.transform.position).sqrMagnitude;

            const float meterThreshold = 2f;
            if (newDistanceSqr < currentDistanceSqr - meterThreshold)
            {
                enemyTarget = closestEnemy;
            }
        }
    }

    // si loopear todos los colliders se pone pesado ponemos hacer un enemy layer y solo buscar ahi
    private Enemy FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        Enemy closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<Enemy>(out var enemy))
            {
                float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private void Attack()
    {
        if (attackingCooldownTimer <= 0)
        {
            enemyTarget.TakeDamage(damage);
            attackingCooldownTimer = attackingCooldownMax;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}