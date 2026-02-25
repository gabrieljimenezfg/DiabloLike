using System;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    private Enemy enemy;

    private const string AnimatorAttackKey = "isAttacking";
    private const string AnimatorPatrollingKey = "isPatrolling";
    private const string AnimatorChasingKey = "isChasing";
    private const string AnimatorHit2Key = "Hit2";

    public void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();

        enemy.StartAttacking += OnStartAttacking;
        enemy.StopAttacking += OnStopAttacking;
        enemy.StartChasing += OnStartChasing;
        enemy.StartPatrolling += OnStartPatrolling;
    }

    private void OnStartAttacking(object sender, EventArgs e)
    {
        animator.SetBool(AnimatorAttackKey, true);
        animator.SetBool(AnimatorChasingKey, false);
        animator.SetBool(AnimatorPatrollingKey, false);
    }

    private void OnStopAttacking(object sender, EventArgs e)
    {
        animator.SetBool(AnimatorAttackKey, false);
    }

    private void OnStartPatrolling(object sender, EventArgs e)
    {
        animator.SetBool(AnimatorPatrollingKey, true);
        animator.SetBool(AnimatorChasingKey, false);
    }

    private void OnStartChasing(object sender, EventArgs e)
    {
        animator.SetBool(AnimatorChasingKey, true);
        animator.SetBool(AnimatorPatrollingKey, false);
    }

    public void FirstHitPerformed()
    {
        animator.SetBool(AnimatorHit2Key, true);
    }

    public void SecondHitPerformed()
    {
        animator.SetBool(AnimatorHit2Key, false);
    }
}
