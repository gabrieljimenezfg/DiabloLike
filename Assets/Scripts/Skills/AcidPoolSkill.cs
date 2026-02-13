using UnityEngine;

public class AcidPoolSkill : BaseAreaDamageSkill
{
    [SerializeField] private float poolRadius = 5f;
    [SerializeField] private Transform visual;
    [SerializeField] private BoxCollider poolCollider;
    [SerializeField] private float colliderHeight;

    //private void Start() => ApplyRadius();

    private void ApplyRadius()
    {
        var d = poolRadius * 2f;
        visual.localScale = new Vector3(d, visual.localScale.y, d);
        poolCollider.size = new Vector3(d, colliderHeight, d);
        poolCollider.center = new Vector3(poolCollider.center.x, colliderHeight * 0.5f, poolCollider.center.z);
    }

    public override void StartCast()
    {
        base.StartCast();
        ApplyRadius();
    }

    protected override void OnDamageableStay(IDamageable damageable, Collider other)
    {
        if (IsWithinRadius(other.transform))
            HandleDamageOverTime(damageable);
        else
            TryRemoveEntity(other.gameObject);
    }

    private bool IsWithinRadius(Transform entity)
    {
        var p = transform.position;
        return Vector2.Distance(new Vector2(p.x, p.z), new Vector2(entity.position.x, entity.position.z)) <= poolRadius;
    }

    public override bool TryExecute(Player caster)
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var hit))
        {
            transform.position = hit.point;
            return true;
        }
        return false;
    }
}