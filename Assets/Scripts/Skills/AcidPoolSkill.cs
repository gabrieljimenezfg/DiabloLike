using System;
using UnityEngine;

public class AcidPoolSkill : MonoBehaviour, ISkillBehavior
{
    private const float height = 0.1f;
    [SerializeField] private float poolRadius = 5f;
    [SerializeField] private Transform visual;
    [SerializeField] private BoxCollider collider;

    private void Start()
    {
        ApplyRadius();
    }

    private void ApplyRadius()
    {
        var poolDiameter = poolRadius * 2f;
        visual.localScale = new Vector3(poolDiameter, visual.localScale.y, poolDiameter);

        collider.size = new Vector3(poolDiameter, collider.size.y, poolDiameter);
    }

    private bool CheckIfEntityIsOnRadius(Transform entity)
    {
        var poolCenter = transform.position;

        var poolPosition2D = new Vector2(poolCenter.x, poolCenter.z);
        var entityPosition2D = new Vector2(entity.position.x, entity.position.z);

        float distance = Vector2.Distance(entityPosition2D, poolPosition2D);

        if (distance <= poolRadius)
        {
            Debug.Log("Burning!");
            return true;
        }

        Debug.Log("Safe!");
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (CheckIfEntityIsOnRadius(other.transform))
            {
                // TODO: damage ticks instead of constant damage
                damageable.TakeDamage(10f);
            }
        }
    }

    public void Execute(Player player)
    {
        if (MouseWorldUtils.TryGetMousePositionOnGround(out var mousePosition))
        {
            Debug.Log("mouse pos " + mousePosition);
            transform.position = mousePosition;
        }
    }
}