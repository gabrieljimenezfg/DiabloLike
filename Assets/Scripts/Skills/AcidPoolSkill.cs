using UnityEngine;

public class AcidPoolSkill : MonoBehaviour, ISkillBehavior
{
    public void Execute(Player player)
    {
        Debug.Log("Acid Pool Skill");
    }
}