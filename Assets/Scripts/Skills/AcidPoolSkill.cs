using System;
using UnityEngine;
using EntitiesInPoolDamageTimers = System.Collections.Generic.Dictionary<IDamageable, float>;

public class AcidPoolSkill : MonoBehaviour, ISkillBehavior
{
    [SerializeField] private float poolRadius = 5f;
    [SerializeField] private Transform visual;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private float colliderHeight;
    [SerializeField] private float damageTickFrequency = 2f;
    [SerializeField] private float skillDamage;
    [SerializeField] private float aliveTime = 5f;
    private EntitiesInPoolDamageTimers entitiesInPoolDamageTimers = new EntitiesInPoolDamageTimers();

    private void Start()
    {
        ApplyRadius();
    }

    private void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0f)
        {
            RemovePool();
        }
    }

    private void RemovePool()
    {
        Destroy(gameObject);
    }

    private void ApplyRadius()
    {
        var poolDiameter = poolRadius * 2f;
        visual.localScale = new Vector3(poolDiameter, visual.localScale.y, poolDiameter);

        collider.size = new Vector3(poolDiameter, colliderHeight, poolDiameter);
        collider.center = new Vector3(collider.center.x, colliderHeight * 0.5f, collider.center.z);
    }

    private bool CheckIfEntityIsOnRadius(Transform entity)
    {
        var poolCenter = transform.position;

        var poolPosition2D = new Vector2(poolCenter.x, poolCenter.z);
        var entityPosition2D = new Vector2(entity.position.x, entity.position.z);

        var distance = Vector2.Distance(entityPosition2D, poolPosition2D);

        return distance <= poolRadius;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (CheckIfEntityIsOnRadius(other.transform))
            {
                HandleDamageAllEntitiesInsideOverTime(damageable);
            }
            else
            {
                TryRemoveEntityFromPool(other.gameObject);
            }
        }
    }

    private void HandleDamageAllEntitiesInsideOverTime(IDamageable damageable)
    {
        if (!entitiesInPoolDamageTimers.ContainsKey(damageable))
        {
            DealDamage(damageable);
        }
        else
        {
            var isDamageIntervalDone = Time.time >= entitiesInPoolDamageTimers[damageable] + damageTickFrequency;
            if (!isDamageIntervalDone) return;

            DealDamage(damageable);
        }
    }

    private void DealDamage(IDamageable damageable)
    {
        Debug.Log("Damaging " + damageable);
        damageable.TakeDamage(skillDamage);
        SetEntityTimerToCurrentTime(damageable);
    }

    private void SetEntityTimerToCurrentTime(IDamageable damageable)
    {
        entitiesInPoolDamageTimers[damageable] = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        TryRemoveEntityFromPool(other.gameObject);
    }


    private void TryRemoveEntityFromPool(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            entitiesInPoolDamageTimers.Remove(damageable);
        }
    }

    public bool TryExecute(Player caster)
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Ground, out var hit))
        {
            var mousePosition = hit.point;
            transform.position = mousePosition;
            return true;
        }

        return false;
    }
}