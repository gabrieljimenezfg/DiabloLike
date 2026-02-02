using UnityEngine;

public class AcidPoolSkill : MonoBehaviour, ISkillBehavior
{
    public void Execute(Player player)
    {
        if (MouseWorldUtils.TryGetMousePositionOnGround(out var mousePosition))
        {
            Debug.Log("mouse pos " + mousePosition);
            transform.position = mousePosition;
        }
    }
}