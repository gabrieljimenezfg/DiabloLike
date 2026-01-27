using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int healthPotions;
    private int manaPotions;

    public bool PickupHealingPotion()
    {
        if (healthPotions <= 0) return false;
        
        healthPotions--;
        return true;
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