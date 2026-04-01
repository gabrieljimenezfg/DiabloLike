using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using SkillCooldowns = System.Collections.Generic.Dictionary<SkillSO, float>;

public class SkillSystem : MonoBehaviour
{
    // debug starter skill
    [SerializeField] private SkillSO starterSkill;
    [SerializeField] private AudioClip skill01;
    // debug raise minion
    [SerializeField] private SkillSO raiseMinionSkill;
    [SerializeField] private AudioClip skill02;
    // debug explode minion
    [SerializeField] private SkillSO explodeMinionSkill;
    [SerializeField] private AudioClip skill03;
    // debug acid blast
    [SerializeField] private SkillSO acidBlastSkill;
    [SerializeField] private AudioClip skill04;
    private Player player;
    private SkillSO[] equippedSkills = new SkillSO[4];

    private SkillCooldowns cooldowns = new SkillCooldowns();

    public class CastedSkillEventArgs : EventArgs
    {
        public int slotId;
        public ISkillBehavior behavior;
        public SkillSO skillSO;
        public Vector3 playerCastingPosition;
    }

    public static EventHandler<CastedSkillEventArgs> CastedSkill;

    private void Awake()
    {
        EquipNewSkill(starterSkill);
        EquipNewSkill(raiseMinionSkill);
        EquipNewSkill(explodeMinionSkill);
        EquipNewSkill(acidBlastSkill);
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void AcquireNewSkill(int skill)
    {
        switch(skill)
        {
            case 0:
                    EquipNewSkill(starterSkill);
                break;
            case 1:
                    EquipNewSkill(raiseMinionSkill);
                    break;
            case 2:
                    EquipNewSkill(explodeMinionSkill);
                    break;
            case 3:
                    EquipNewSkill(acidBlastSkill);
                    break;
            default:
                break;
        }
    }
    private void EquipNewSkill(SkillSO skill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            Debug.Log("Checking slot " + i);
            if (equippedSkills[i] != null)
            {
                Debug.Log("Found equipped skill on slot " + i);
                Debug.Log(equippedSkills[i].skillName);
                continue;
            }

            ;
            Debug.Log("Equipping skill " + skill.skillName + " on slot " + i);
            equippedSkills[i] = skill;
            return;
        }
    }

    public void ClearSkills()
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            Debug.Log("Checking slot " + i);
            if (equippedSkills[i] != null)
            {
                Debug.Log("Found equipped skill on slot " + i);
                Debug.Log("Deleting skill on slot" + i);
                Debug.Log(equippedSkills[i].skillName);
                equippedSkills[i] = null;
                continue;
            }
        }
    }

    public SkillSO GetSkillInSlot(int slotId)
    {
        return equippedSkills[slotId];
    }

    public float GetSkillCooldown(int slotId)
    {
        var skill = GetSkillInSlot(slotId);
        if (!skill)
        {
            Debug.LogError($"Couldn't find skill {slotId}");
            return 0;
        }

        ;
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

        var skillInstance = Instantiate(skill.skillPrefab, Vector3.zero, Quaternion.identity);
        var baseSkill = skillInstance.GetComponent<BaseSkill>();
        ISkillBehavior behavior = skillInstance.GetComponent<ISkillBehavior>();
        if (behavior == null) return;

        if (behavior.TryExecute(player))
        {
            player.UseMana(skill.manaCost);
            cooldowns[skill] = skill.cooldown;
            switch (slotId)
            {
                case 0:
                    AudioManager.instance.PlaySFX(skill01, transform.position);
                    break;
                case 1:
                    AudioManager.instance.PlaySFX(skill02, transform.position);
                    break;
                case 2:
                    AudioManager.instance.PlaySFX(skill03, transform.position);
                    break;
                case 3:
                    AudioManager.instance.PlaySFX(skill04, transform.position);
                    break;
                default:
                    break;
            }
            CastedSkill?.Invoke(this, new CastedSkillEventArgs
            {
                slotId = slotId,
                behavior = behavior,
                skillSO = baseSkill.SkillData,
                playerCastingPosition = player.transform.position
            });
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