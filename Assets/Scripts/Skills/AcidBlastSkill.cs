using UnityEngine;

public class AcidBlastSkill : BaseAreaDamageSkill
{
    [SerializeField] private float blastRange = 100f;
    [SerializeField] private float blastBaseWidth = 25f;
    [SerializeField] private float xyIncreaseFactor = 0.02f;
    [SerializeField] private Transform visual;
    private BoxCollider blastHitbox;
    private Player player;

    private void Awake()
    {
        blastHitbox = GetComponent<BoxCollider>();
        ApplySize();
    }

    private void ApplySize()
    {
        var zOffset = blastRange * 0.5f;
        var xySize = blastBaseWidth * xyIncreaseFactor;
        visual.localScale = new Vector3(xySize, xySize, blastRange);
        visual.localPosition = new Vector3(visual.localPosition.x, visual.localPosition.y, zOffset);
        blastHitbox.size = new Vector3(xySize, xySize, blastRange);
        blastHitbox.center = new Vector3(blastHitbox.center.x, blastHitbox.center.y, zOffset);
    }

    protected override void Remove()
    {
        player.TogglePositionRotationLock(false);
        base.Remove();
    }

    public override bool TryExecute(Player caster)
    {
        player = caster;
        player.TogglePositionRotationLock(true);
        transform.position = player.CastSpawnPoint.position;
        transform.forward = player.CastSpawnPoint.forward;
        transform.parent = player.CastSpawnPoint.transform;
        return true;
    }
}