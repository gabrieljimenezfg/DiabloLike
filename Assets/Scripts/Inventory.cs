using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int healthPotions;
    private int manaPotions;
    private HealingPotionSO healingPotion;
    private ManaPotionSO manaPotion;

    public bool PickupHealingPotion()
    {
        if (healthPotions <= 0) return false;

        healthPotions--;
        return true;
    }

    public float GetHealingPotionHealthAmount()
    {
        return healingPotion.healingAmount;
    }
    
    public float GetManaPotionRecoverAmount()
    {
        return healingPotion.healingAmount;
    }

    public void Save(ref InventoryState inventoryState)
    {
        inventoryState.healthPotions = healthPotions;
        inventoryState.manaPotions = manaPotions;
    }

    public void Load(InventoryState inventoryState)
    {
        healthPotions = inventoryState.healthPotions;
        manaPotions = inventoryState.manaPotions;
    }
}