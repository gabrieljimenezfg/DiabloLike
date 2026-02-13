using UnityEngine;

public class ExplodeMinionSkill : BaseSkill, ISkillBehavior
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    private Vector3 explotionPosition;

    public bool TryExecute(Player caster)
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Minion, out var hit))
        {
            if (hit.transform.TryGetComponent<Minion>(out var minion))
            {
                explotionPosition = minion.transform.position;
                minion.Explode();
                DebugUtils.DrawSphere(explotionPosition, explosionRadius, Color.red);
                return true;
            }
        }

        return false;
    }

    public override void StartCast()
    {
        base.StartCast();
        DealAreaDamage(explotionPosition);
    }

    private void DealAreaDamage(Vector3 explotionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explotionPosition, explosionRadius);

        foreach (var col in colliders)
        {
            if (col.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}