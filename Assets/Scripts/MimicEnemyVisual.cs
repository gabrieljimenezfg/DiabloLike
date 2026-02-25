using System;
using UnityEngine;

public class MimicEnemyVisual : EnemyVisual
{
    private const string AnimatorOutOfGroundKey = "OutOfGround";

    private MimicEnemy mimicEnemy;
    
    void Start()
    {
        base.Start();
        mimicEnemy = GetComponentInParent<MimicEnemy>();

        mimicEnemy.IsGettingOutOfGround += OnOutOfGround;
    }

    private void OnOutOfGround(object sender, EventArgs e)
    {
        animator.SetTrigger(AnimatorOutOfGroundKey);
    }
}
