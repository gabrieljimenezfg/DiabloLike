using System;
using UnityEngine;

public class FirstBossVisual : EnemyVisual
{
    private FirstBoss firstBoss;
    private const string AnimatorAttackPhaseKey = "AttackPhase";
    private const string AnimatorAttackKey = "isAttacking";

    void Start()
    {
        base.Start();
        firstBoss = GetComponentInParent<FirstBoss>();

        firstBoss.ChangeAttackPhase += OnChangeAttackPhase;
        firstBoss.StartWaiting += OnStartWaiting;
    }

    private void OnChangeAttackPhase(object sender, int _attackPhase)
    {
        animator.SetInteger(AnimatorAttackPhaseKey, _attackPhase);
    }

    public void ResetAttackPhase()
    {
        animator.SetInteger(AnimatorAttackPhaseKey, 0);
    }

    private void OnStartWaiting(object sender, EventArgs e)
    {
        animator.SetBool(AnimatorAttackKey, false);
    }
}
