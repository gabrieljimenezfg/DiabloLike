using System;
using UnityEngine;
using EntitiesInBlastDamageTimers = System.Collections.Generic.Dictionary<IDamageable, float>;

public class AcidBlastSkill : MonoBehaviour, ISkillBehavior
{
    [SerializeField] private float blastRange = 100f;
    [SerializeField] private float blastBaseWidth = 25f;
    [SerializeField] private Transform visual;
    [SerializeField] private float damageTickFrequency = 0.5f;
    [SerializeField] private float baseAliveTime = 5f;
    [SerializeField] private float baseSkillDamage;
    [SerializeField] private float xyIncreaseFactor = 0.02f;
    private BoxCollider blastHitbox;

    private EntitiesInBlastDamageTimers entitiesInBlastDamageTimers = new EntitiesInBlastDamageTimers();

    private void Awake()
    {
        blastHitbox = GetComponent<BoxCollider>();
        ApplySize();
    }

    private void Update()
    {
        baseAliveTime -= Time.deltaTime;
        if (baseAliveTime <= 0f)
        {
            RemoveBlast();
        }
    }

    private void RemoveBlast()
    {
        // Destroy(gameObject);
    }

    private void ApplySize()
    {
        var zOffset = blastRange * 0.5f;
        var xySize = blastBaseWidth * xyIncreaseFactor;

        visual.localScale = new Vector3(xySize, xySize, blastRange);
        visual.localPosition = new Vector3(visual.localPosition.x, visual.localPosition.y, zOffset);

        blastHitbox.size = new Vector3(xySize, xySize, blastRange);
        blastHitbox.center = new Vector3(blastHitbox.center.x, blastHitbox.center.y, zOffset);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            HandleDamageAllEntitiesInsideOverTime(damageable);
        }
    }

    private void HandleDamageAllEntitiesInsideOverTime(IDamageable damageable)
    {
        if (!entitiesInBlastDamageTimers.ContainsKey(damageable))
        {
            DealDamage(damageable);
        }
        else
        {
            var isDamageIntervalDone = Time.time >= entitiesInBlastDamageTimers[damageable] + damageTickFrequency;
            if (!isDamageIntervalDone) return;

            DealDamage(damageable);
        }
    }

    private void DealDamage(IDamageable damageable)
    {
        damageable.TakeDamage(baseSkillDamage);
        SetEntityTimerToCurrentTime(damageable);
    }

    private void SetEntityTimerToCurrentTime(IDamageable damageable)
    {
        entitiesInBlastDamageTimers[damageable] = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        TryRemoveEntityFromBlast(other.gameObject);
    }

    private void TryRemoveEntityFromBlast(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            entitiesInBlastDamageTimers.Remove(damageable);
        }
    }

    private void RotatePlayerTowardsMouse(Player player)
    {
        var mousePosition = MouseWorldUtils.GetMouseWorldPositionOnPlane(player.transform.position);
        Vector3 direction = (mousePosition - player.transform.position).normalized;
        direction.y = 0f;
        player.SetLookDirection(direction);
    }

    public bool TryExecute(Player player)
    {
        RotatePlayerTowardsMouse(player);
        
        transform.position = player.CastSpawnPoint.position;
        transform.forward = player.CastSpawnPoint.forward;
        
        return true;
    }
}