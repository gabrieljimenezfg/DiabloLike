using UnityEngine;

public class ArcherEnemyVisual : EnemyVisual
{
    private void Start()
    {
        base.Start();
        enemy = GetComponentInParent<ArcherEnemy>();
    }
}
