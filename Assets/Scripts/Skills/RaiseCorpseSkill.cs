using UnityEngine;

public class RaiseCorpseSkill : MonoBehaviour, ISkillBehavior
{
    public bool TryExecute(Player caster)
    {
        Debug.Log("[RaiseCorpseSkill] Try Execute");
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Corpse, out var hit))
        {
            Debug.Log("[RaiseCorpseSkill] Found corpse layer");
            if (hit.transform.TryGetComponent<Corpse>(out var corpse))
            {
                Debug.Log("[RaiseCorpseSkill] Found corpse component");
                corpse.SpawnMinion();
                return true;
            }
        }
        
        Debug.Log("[RaiseCorpseSkill] No corpse layer");
        return false;
    }
}