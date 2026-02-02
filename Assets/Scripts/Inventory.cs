using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int healthPotionsAmountHeld = 5;
    private int manaPotionsAmountHeld = 5;
    private int maxHealthPotionsAmountHeld = 10;
    private int maxManaPotionsAmountHeld = 10;
    [SerializeField] private PotionSO healingPotionData;
    [SerializeField] private PotionSO manaPotionData;

    public static event EventHandler PotionsAmountChanged;

    public int HealthPotionsAmountHeld => healthPotionsAmountHeld;
    public int ManaPotionsAmountHeld => manaPotionsAmountHeld;

    /// <summary>
    /// Attempts to pick up a health potion.
    /// </summary>
    /// <returns>
    /// True if a health potion was successfully picked up; 
    /// false if no health potions were available.
    /// </returns>
    public bool TryConsumeHealingPotion()
    {
        if (healthPotionsAmountHeld <= 0) return false;

        healthPotionsAmountHeld--;
        PotionsAmountChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Attempts to pick up a mana potion.
    /// </summary>
    /// <returns>
    /// True if a mana potion was successfully picked up; 
    /// false if no mana potions were available.
    /// </returns>
    public bool TryConsumeManaPotion()
    {
        if (manaPotionsAmountHeld <= 0) return false;

        manaPotionsAmountHeld--;
        PotionsAmountChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public float GetHealingPotionHealthAmount()
    {
        return healingPotionData.recoverAmount;
    }

    public float GetManaPotionRecoverAmount()
    {
        return manaPotionData.recoverAmount;
    }

    public bool TryPickupHealingPotion()
    {
        if (healthPotionsAmountHeld >= maxHealthPotionsAmountHeld) return false;

        healthPotionsAmountHeld++;
        PotionsAmountChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public bool TryPickupManaPotion()
    {
        if (manaPotionsAmountHeld >= maxManaPotionsAmountHeld) return false;

        manaPotionsAmountHeld++;
        PotionsAmountChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public void Save(ref InventoryState inventoryState)
    {
        inventoryState.healthPotions = healthPotionsAmountHeld;
        inventoryState.manaPotions = manaPotionsAmountHeld;
        inventoryState.maxHealthPotions = maxHealthPotionsAmountHeld;
        inventoryState.maxManaPotions = maxManaPotionsAmountHeld;
    }

    public void Load(InventoryState inventoryState)
    {
        healthPotionsAmountHeld = inventoryState.healthPotions;
        manaPotionsAmountHeld = inventoryState.manaPotions;
        maxHealthPotionsAmountHeld = inventoryState.maxHealthPotions;
        maxManaPotionsAmountHeld = inventoryState.maxManaPotions;
    }
}