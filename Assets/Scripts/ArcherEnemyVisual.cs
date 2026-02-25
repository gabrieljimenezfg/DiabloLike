using System;
using UnityEngine;

public class ArcherEnemyVisual : EnemyVisual
{
    private string AnimatorReviveKey = "Revive";
    private ArcherEnemy archerEnemy;

    private void Start()
    {
        base.Start();
        archerEnemy = GetComponentInParent<ArcherEnemy>();

        archerEnemy.StartReviving += OnStartReviving;
    }

    private void OnStartReviving(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorReviveKey);
    }
}
