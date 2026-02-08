using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SkillCooldowns = System.Collections.Generic.Dictionary<SkillSO, float>;

public class SkillSystem : MonoBehaviour
{
    // debug starter skill
    [SerializeField] private SkillSO starterSkill;

    // debug raise minion
    [SerializeField] private SkillSO raiseMinionSkill;

    private Player player;
    private SkillSO[] equippedSkills = new SkillSO[4];

    private SkillCooldowns cooldowns = new SkillCooldowns();

    private void Start()
    {
        player = GetComponent<Player>();
        EquipNewSkill(starterSkill);
        EquipNewSkill(raiseMinionSkill);
    }

    private void EquipNewSkill(SkillSO skill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] != null) continue;
            equippedSkills[i] = skill;
            return;
        }
    }

    public SkillSO GetSkillInSlot(int slotId)
    {
        return equippedSkills[slotId];
    }

    public float GetSkillCooldown(int slotId)
    {
        var skill = GetSkillInSlot(slotId);
        if (!cooldowns.ContainsKey(skill)) return 0;

        return cooldowns[skill];
    }

    private bool IsSkillOnCooldown(SkillSO skill)
    {
        return cooldowns.ContainsKey(skill) && cooldowns[skill] > 0;
    }

    public void CastSkill(int slotId)
    {
        Debug.Log("Try use skill in slot id: " + slotId);
        var skill = GetSkillInSlot(slotId);
        if (skill == null) return;

        Debug.Log("Found skill: " + skill.name);

        if (IsSkillOnCooldown(skill)) return;
        if (player.Mana < skill.manaCost) return;

        Debug.Log("Casting skill");

        GameObject skillInstance = Instantiate(skill.skillPrefab, Vector3.zero, Quaternion.identity);
        ISkillBehavior behavior = skillInstance.GetComponent<ISkillBehavior>();
        if (behavior == null) return;

        if (behavior.TryExecute(player))
        {
            player.UseMana(skill.manaCost);
            cooldowns[skill] = skill.cooldown;
        }
        else
        {
            Destroy(skillInstance);
        }
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