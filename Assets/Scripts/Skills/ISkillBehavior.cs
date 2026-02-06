using UnityEngine;

public interface ISkillBehavior
{
    bool TryExecute(Player player);
}