using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private HealingPotionSO healingPotionData;
    private ManaPotionSO manaPotionData;
    private int healthPotionsAmountHeld;
    private int manaPotionsAmountHeld;

    public bool PickupHealingPotion()
    {
        if (healthPotionsAmountHeld <= 0) return false;

        healthPotionsAmountHeld--;
        return true;
    }

    public float GetHealingPotionHealthAmount()
    {
        return healingPotionData.healingAmount;
    }
    
    public float GetManaPotionRecoverAmount()
    {
        return healingPotionData.healingAmount;
    }

    public void Save(ref InventoryState inventoryState)
    {
        inventoryState.healthPotions = healthPotionsAmountHeld;
        inventoryState.manaPotions = manaPotionsAmountHeld;
    }

    public void Load(InventoryState inventoryState)
    {
        healthPotionsAmountHeld = inventoryState.healthPotions;
        manaPotionsAmountHeld = inventoryState.manaPotions;
    }
}