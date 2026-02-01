using System;
using TMPro;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    private SkillSystem skillSystem;
    [SerializeField] private SkillSlotUI[] skillSlotsUI;

    [SerializeField] private TextMeshProUGUI healingPotionSlotText;
    [SerializeField] private TextMeshProUGUI manaPotionSlotText;

    private void Start()
    {
        skillSystem = Player.Instance.GetSkillSystem();
        Inventory.PotionsAmountChanged += OnPlayerPotionsAmountChanged;
        UpdatePotionsText();
    }

    private void Update()
    {
        UpdateSkillSlots();
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

    public void UpdatePotionsText()
    {
        var inventory = Player.Instance.GetInventory();

        healingPotionSlotText.text = inventory.HealthPotionsAmountHeld.ToString();
        manaPotionSlotText.text = inventory.ManaPotionsAmountHeld.ToString();
    }
}