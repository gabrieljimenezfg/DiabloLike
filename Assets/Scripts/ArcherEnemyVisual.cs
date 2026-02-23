using System;
using UnityEngine;

public class ArcherEnemyVisual : EnemyVisual
{
    private string AnimatorReviveKey = "Revive";
    private ArcherEnemy enemy;

    private void Start()
    {
        base.Start();
        enemy = GetComponentInParent<ArcherEnemy>();

        enemy.StartReviving += OnStartReviving;
    }

    private void OnStartReviving(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorReviveKey);
    }
}
