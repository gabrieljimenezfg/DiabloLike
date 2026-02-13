using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    private static List<Minion> minionsAlive = new List<Minion>();
    private NavMeshAgent navMeshAgent;
    private Enemy enemyTarget;

    [SerializeField] private float attackingCooldownMax;
    private float attackingCooldownTimer;
    [SerializeField] private float targetUpdateInterval;
    private float targetUpdateTimer;

    [SerializeField] private float damage;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float maxDistanceFromPlayer;
    private float resumeCombatDistance;
    private bool isReturningToPlayer;

    public static int MinionsAliveCount => minionsAlive.Count;

    public event EventHandler IsAttacking;

    //public Vector3 TargetPosition => navMeshAgent.destination;

    public static void RemoveMinionAmount(int minionAmount)
    {
        for (int i = 0; i < minionAmount; i++)
        {
            minionsAlive[i].Consume();
        }
    }

    private void OnEnable()
    {
        minionsAlive.Add(this);
    }

    private void OnDestroy()
    {
        minionsAlive.Remove(this);
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        /*
         * if we have a detection radius of 10 meters, and a max distance from player of 15 meters
         * when we go above 15, we want to go back to the player, but to avoid being stuck in a state
         * of back and forth between chasing and going to the player, we make sure we go back to at least enough
         * distance so that the detection radius wont detect the enemy
         * this is why we require detection radius to be smaller than max distance from player
         */
        resumeCombatDistance = maxDistanceFromPlayer - detectionRadius;
    }

    private void Update()
    {
        attackingCooldownTimer -= Time.deltaTime;
        targetUpdateTimer -= Time.deltaTime;

        var distanceToPlayerSqr = (transform.position - Player.Instance.transform.position).sqrMagnitude;

        if (distanceToPlayerSqr > maxDistanceFromPlayer * maxDistanceFromPlayer)
        {
            isReturningToPlayer = true;
        }
        else if (distanceToPlayerSqr < resumeCombatDistance * resumeCombatDistance)
        {
            isReturningToPlayer = false;
        }

        if (isReturningToPlayer)
        {
            enemyTarget = null;
            StayNearPlayer();
        }
        else
        {
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
            IsAttacking?.Invoke(this, EventArgs.Empty);
            //Attack();
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

    // si loopear todos los colliders se pone pesado podemos hacer un enemy layer y solo buscar ahi
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

    public void Attack()
    {
        if (attackingCooldownTimer <= 0)
        {
            enemyTarget.TakeDamage(damage);
            attackingCooldownTimer = attackingCooldownMax;
        }
    }

    private void Consume()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        Destroy(gameObject, 0.2f);
    }

    /*
     * DEBUG
     */

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnValidate()
    {
        if (detectionRadius >= maxDistanceFromPlayer)
        {
            Debug.LogWarning(
                $"detectionRadius ({detectionRadius}) debe ser menor que maxDistanceFromPlayer ({maxDistanceFromPlayer})");
            detectionRadius = maxDistanceFromPlayer - 1f;
        }
    }

    // call it a the end of Update() to visualize distances
    private void DebugDrawLeash()
    {
        Vector3 playerPos = Player.Instance.transform.position;

        // Line to player
        DebugUtils.DrawLine(transform.position, playerPos, isReturningToPlayer ? Color.red : Color.white);

        // Draw circles (approximation with lines)
        DebugUtils.DrawCircle(playerPos, maxDistanceFromPlayer, Color.red);
        DebugUtils.DrawCircle(playerPos, resumeCombatDistance, Color.green);
        DebugUtils.DrawCircle(transform.position, detectionRadius, Color.yellow);
    }
}