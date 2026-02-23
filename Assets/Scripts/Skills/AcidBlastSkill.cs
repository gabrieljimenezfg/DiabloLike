using System;
using UnityEditor.Scripting;
using UnityEngine;

public class AcidBlastSkill : BaseAreaDamageSkill
{
    [SerializeField] private float blastRange = 100f;
    [SerializeField] private float blastBaseWidth = 25f;
    [SerializeField] private float perMinionWidthIncrease = 0.5f;
    [SerializeField] private float perMinionDamageIncreasePercentage = 0.1f;
    
    private BoxCollider blastHitbox;
    private Player player;
    private int minionsConsumed;
    private bool blasting;

    protected override void Awake()
    {
        blastHitbox = GetComponent<BoxCollider>();
        blastHitbox.enabled = false;
    }

    private void ApplySizeAndDamage()
    {
        var zOffset = blastRange * 0.5f;
        var sizeIncrease = perMinionWidthIncrease * minionsConsumed;
        var xySize = blastBaseWidth + sizeIncrease;
        // visual.localScale = new Vector3(xySize, xySize, blastRange);
        // visual.localPosition = new Vector3(visual.localPosition.x, visual.localPosition.y, zOffset);
        blastHitbox.size = new Vector3(xySize, xySize, blastRange);
        blastHitbox.center = new Vector3(blastHitbox.center.x, blastHitbox.center.y, zOffset);

        skillDamage *= (1f + perMinionDamageIncreasePercentage * (minionsConsumed - 1));
        Debug.Log("Hitting with damage " + skillDamage);
    }

    public override void StartCast()
    {
        blastHitbox.enabled = true;
        ApplySizeAndDamage();
    }

    protected override void Remove()
    {
        player.TogglePositionRotationLock(false);
        base.Remove();
    }

    public override bool TryExecute(Player caster)
    {
        if (TryConsumeMinionsForSkill())
        {
            ApplySizeAndDamage();
            player = caster;
            player.TogglePositionRotationLock(true);
            transform.position = player.CastSpawnPoint.position;
            transform.forward = player.CastSpawnPoint.forward;
            transform.parent = player.CastSpawnPoint.transform;
            return true;
        }

        return false;
    }

    private bool TryConsumeMinionsForSkill()
    {
        minionsConsumed = Minion.MinionsAliveCount;
        Debug.Log("Consuming " + minionsConsumed + " minions");
        if (minionsConsumed == 0)
        {
            return false;
        }

        Minion.RemoveMinionAmount(minionsConsumed);
        return true;
    }
}