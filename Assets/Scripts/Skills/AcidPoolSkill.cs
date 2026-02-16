using UnityEngine;

public class AcidPoolSkill : BaseAreaDamageSkill
{
    [SerializeField] private float poolDiameter = 5f;
    [SerializeField] private Transform visual;
    [SerializeField] private BoxCollider poolCollider;
    [SerializeField] private float colliderHeight;

    //private void Start() => ApplyRadius();

    private void ApplyRadius()
    {
        visual.localScale = new Vector3(visual.localScale.x * poolDiameter, visual.localScale.y, visual.localScale.z * poolDiameter);
        poolCollider.size = new Vector3(poolDiameter, colliderHeight, poolDiameter);
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
        return Vector2.Distance(new Vector2(p.x, p.z), new Vector2(entity.position.x, entity.position.z)) <= poolDiameter / 2;
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