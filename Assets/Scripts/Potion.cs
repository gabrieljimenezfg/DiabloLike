using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private PotionSO potionSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Inventory>(out var inventory))
        {
            if (TryAddPotionToPlayer(inventory, potionSO))
            {
                DestroyPotion();
            }
        }
    }

    private bool TryAddPotionToPlayer(Inventory inventory, PotionSO potionSO)
    {
        switch (potionSO.potionType)
        {
            case PotionType.Health:
                return inventory.TryPickupHealingPotion();
            case PotionType.Mana:
                return inventory.TryPickupManaPotion();
            default:
                return false;
        }
    }

    private void DestroyPotion()
    {
        Destroy(gameObject);
    }
}