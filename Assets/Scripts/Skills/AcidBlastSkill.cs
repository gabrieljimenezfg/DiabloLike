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
    private Player player;

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
        player.TogglePositionRotationLock(false);
        Destroy(gameObject);
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

    public bool TryExecute(Player caster)
    {
        player = caster;
        player.TogglePositionRotationLock(true);
        
        transform.position = player.CastSpawnPoint.position;
        transform.forward = player.CastSpawnPoint.forward;
        transform.parent = player.CastSpawnPoint.transform; 
        
        return true;
    }
}