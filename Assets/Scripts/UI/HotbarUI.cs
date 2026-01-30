using System;
using TMPro;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healingPotionSlotText;
    [SerializeField] private TextMeshProUGUI manaPotionSlotText;

    private void Start()
    {
        Player.Instance.PlayerUsedPotion += OnPlayerUsedPotion;
        UpdateVisual();
    }

    private void OnPlayerUsedPotion(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        var inventory = Player.Instance.GetInventory();

        healingPotionSlotText.text = inventory.HealthPotionsAmountHeld.ToString();
        manaPotionSlotText.text = inventory.ManaPotionsAmountHeld.ToString();
    }
}