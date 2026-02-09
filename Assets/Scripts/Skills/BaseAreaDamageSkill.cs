using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAreaDamageSkill : MonoBehaviour, ISkillBehavior
{
    [SerializeField] protected float damageTickFrequency = 0.5f;
    [SerializeField] protected float skillDamage;
    [SerializeField] protected float aliveTime = 5f;

    private Dictionary<IDamageable, float> entitiesTimers = new();

    protected virtual void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0f)
            Remove();
    }

    protected virtual void Remove()
    {
        Destroy(gameObject);
    }

    protected void HandleDamageOverTime(IDamageable damageable)
    {
        if (!entitiesTimers.TryGetValue(damageable, out var lastTime))
        {
            DealDamage(damageable);
        }
        else if (Time.time >= lastTime + damageTickFrequency)
        {
            DealDamage(damageable);
        }
    }

    private void DealDamage(IDamageable damageable)
    {
        damageable.TakeDamage(skillDamage);
        entitiesTimers[damageable] = Time.time;
    }

    protected void TryRemoveEntity(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            entitiesTimers.Remove(damageable);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            OnDamageableStay(damageable, other);
    }

    protected virtual void OnDamageableStay(IDamageable damageable, Collider _)
    {
        HandleDamageOverTime(damageable);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        TryRemoveEntity(other.gameObject);
    }

    public abstract bool TryExecute(Player caster);
}