using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SkillCooldowns = System.Collections.Generic.Dictionary<SkillSO, float>;

public class SkillSystem : MonoBehaviour
{
    private Player player;
    private SkillSO[] equippedSkills;

    private SkillCooldowns cooldowns = new SkillCooldowns();

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private SkillSO GetSkillInSlot(int slotId)
    {
        return equippedSkills[slotId];
    }

    private bool IsSkillOnCooldown(SkillSO skill)
    {
        return cooldowns.ContainsKey(skill) && cooldowns[skill] > 0;
    }

    public bool TryUseSkill(int slotId)
    {
        var skill = GetSkillInSlot(slotId);
        if (IsSkillOnCooldown(skill)) return false;
        if (player.Mana < skill.manaCost) return false;

        GameObject skillInstance = Instantiate(skill.skillPrefab, Vector3.zero, Quaternion.identity);
        ISkillBehavior behavior = skillInstance.GetComponent<ISkillBehavior>();
        if (behavior == null) return false;

        player.UseMana(skill.manaCost);
        behavior.Execute(player);
        cooldowns[skill] = skill.cooldown;

        return true;
    }

    private void HandleSkillsCooldownReduction()
    {
        foreach (var skill in cooldowns.Keys.ToList())
        {
            if (cooldowns[skill] > 0)
            {
                cooldowns[skill] -= Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        HandleSkillsCooldownReduction();
    }
}