using UnityEngine;

public enum MouseRayTargetLayer
{
    Ground,
    Enemy,
    Corpse,
}

public static class MouseWorldUtils
{
    private static readonly Camera camera = Camera.main;

    private static readonly LayerMask groundLayer =
        LayerMask.GetMask("Ground");

    private static readonly LayerMask enemyLayer =
        LayerMask.GetMask("Highlightable");

    private static readonly LayerMask corpseLayer =
        LayerMask.GetMask("Corpse");

    private static LayerMask GetMask(MouseRayTargetLayer targetLayer)
    {
        switch (targetLayer)
        {
            default:
            case MouseRayTargetLayer.Ground:
                return groundLayer;
            case MouseRayTargetLayer.Enemy:
                return enemyLayer;
            case MouseRayTargetLayer.Corpse:
                return corpseLayer;
        }
    }

    public static bool TryGetMousePositionOnTargetLayer(
        MouseRayTargetLayer targetLayer,
        out RaycastHit hit)
    {
        var targetLayerMask = GetMask(targetLayer);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask);
    }
}