using UnityEngine;

public class RaiseCorpseSkill : MonoBehaviour, ISkillBehavior
{
    private Corpse actualCorpse;

    public bool TryExecute(Player caster)
    {
        Debug.Log("[RaiseCorpseSkill] Try Execute");
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Corpse, out var hit))
        {
            Debug.Log("[RaiseCorpseSkill] Found corpse layer");
            if (hit.transform.TryGetComponent<Corpse>(out var corpse))
            {
                Debug.Log("[RaiseCorpseSkill] Found corpse component");
                actualCorpse = corpse;
                return true;
            }
        }
        
        Debug.Log("[RaiseCorpseSkill] No corpse layer");
        return false;
    }

    public void StartCast()
    {
        actualCorpse.SpawnMinion();
    }
}