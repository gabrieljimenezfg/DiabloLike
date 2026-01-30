using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int healthPotionsAmountHeld;
    private int manaPotionsAmountHeld;
    private HealingPotionSO healingPotionData;
    private ManaPotionSO manaPotionData;

    /// <summary>
    /// Attempts to pick up a health potion.
    /// </summary>
    /// <returns>
    /// True if a health potion was successfully picked up; 
    /// false if no health potions were available.
    /// </returns>
    public bool PickupHealingPotion()
    {
        if (healthPotionsAmountHeld <= 0) return false;

        healthPotionsAmountHeld--;
        return true;
    }

    /// <summary>
    /// Attempts to pick up a mana potion.
    /// </summary>
    /// <returns>
    /// True if a mana potion was successfully picked up; 
    /// false if no mana potions were available.
    /// </returns>
    public bool PickupManaPotion()
    {
        if (manaPotionsAmountHeld <= 0) return false;

        manaPotionsAmountHeld--;
        return true;
    }

    public float GetHealingPotionHealthAmount()
    {
        return healingPotionData.healingAmount;
    }

    public float GetManaPotionRecoverAmount()
    {
        return manaPotionData.manaRecoverAmount;
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