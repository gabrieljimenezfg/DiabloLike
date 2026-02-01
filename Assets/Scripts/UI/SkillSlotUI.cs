using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotText;
    [SerializeField] private Image cooldownOverlayImage;
    [SerializeField] private Image notEnoughManaImage;

    public void UpdateSkillState(float remainingCooldown, float skillCooldown, float currentPlayerMana,
        float skillManaCost)
    {
        var isOnCooldown = remainingCooldown > 0f;
        var hasEnoughMana = currentPlayerMana >= skillManaCost;

        cooldownOverlayImage.gameObject.SetActive(isOnCooldown);
        if (isOnCooldown)
        {
            cooldownOverlayImage.fillAmount = remainingCooldown / skillCooldown;
        }

        var shouldShowNotEnoughMana = !isOnCooldown && !hasEnoughMana;
        var shouldShowSlotText = !isOnCooldown && hasEnoughMana;

        notEnoughManaImage.gameObject.SetActive(shouldShowNotEnoughMana);
        slotText.gameObject.SetActive(shouldShowSlotText);
    }
}