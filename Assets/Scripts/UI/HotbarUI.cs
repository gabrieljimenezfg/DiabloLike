using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    private SkillSystem skillSystem;
    [SerializeField] private SkillSlotUI[] skillSlotsUI;

    [SerializeField] private TextMeshProUGUI healingPotionSlotText;
    [SerializeField] private TextMeshProUGUI manaPotionSlotText;

    [SerializeField] private Image healthOrb, manaOrb;

    private void Start()
    {
        skillSystem = Player.Instance.GetSkillSystem();
        Player.Instance.PlayerHPChanged += OnPlayerOrbStatChanged;
        Player.Instance.PlayerManaChanged += OnPlayerOrbStatChanged;
        Inventory.PotionsAmountChanged += OnPlayerPotionsAmountChanged;
        InitializeSkills();
        UpdatePotionsText();
    }

    private void InitializeSkills()
    {
        for (int i = 0; i < skillSlotsUI.Length; i++)
        {
            var skillInSlot = skillSystem.GetSkillInSlot(i);

            skillSlotsUI[i].Initialize(skillInSlot);
        }
    }

    private void OnPlayerOrbStatChanged(object sender, EventArgs e)
    {
        UpdateOrbsVisual();
    }

    private void Update()
    {
        UpdateSkillSlots();
    }

    private void UpdateOrbsVisual()
    {
        healthOrb.fillAmount = Player.Instance.HP / Player.Instance.MaxHP;
        manaOrb.fillAmount = Player.Instance.Mana / Player.Instance.MaxMana;
    }

    private void UpdateSkillSlots()
    {
        var currentPlayerMana = Player.Instance.Mana;
        for (int i = 0; i < skillSlotsUI.Length; i++)
        {
            var skillInSlot = skillSystem.GetSkillInSlot(i);
            var remainingCooldown = skillSystem.GetSkillCooldown(i);

            skillSlotsUI[i].UpdateSkillState(remainingCooldown, skillInSlot.cooldown, currentPlayerMana,
                skillInSlot.manaCost);
        }
    }

    private void OnPlayerPotionsAmountChanged(object sender, EventArgs e)
    {
        UpdatePotionsText();
    }

    private void UpdatePotionsText()
    {
        var inventory = Player.Instance.GetInventory();

        healingPotionSlotText.text = inventory.HealthPotionsAmountHeld.ToString();
        manaPotionSlotText.text = inventory.ManaPotionsAmountHeld.ToString();
    }
}