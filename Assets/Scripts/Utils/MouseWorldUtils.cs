using UnityEngine;
using UnityEngine.InputSystem;

public enum MouseRayTargetLayer
{
    All,
    Ground,
    Enemy,
    Corpse,
    Minion,
}

public static class MouseWorldUtils
{
    private static Camera camera => Camera.main;

    private static readonly LayerMask groundLayer =
        LayerMask.GetMask("Ground");

    private static readonly LayerMask enemyLayer =
        LayerMask.GetMask("Enemy");

    private static readonly LayerMask corpseLayer =
        LayerMask.GetMask("Corpse");
    
    private static readonly LayerMask minionLayer =
        LayerMask.GetMask("Minion");

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
            case MouseRayTargetLayer.Minion:
                return minionLayer;
            case MouseRayTargetLayer.All:
                return ~0;
        }
    }

    public static bool TryGetMousePositionOnTargetLayer(
        MouseRayTargetLayer targetLayer,
        out RaycastHit hit)
    {
        var targetLayerMask = GetMask(targetLayer);
        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask);
    }
    
    public static bool TryGetFirstHighlightable(out Highlightable highlightable)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(mousePosition);
        var hits = Physics.RaycastAll(ray);

        foreach (var raycastHit in hits)
        {
            if (raycastHit.collider.TryGetComponent(out highlightable))
            {
                return true;
            }
        }

        highlightable = null;
        return false;
    }

    public static Vector3 GetMouseWorldPositionOnPlane(Vector3 position)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = camera.ScreenPointToRay(mousePosition);
        var groundPlane = new Plane(Vector3.up, position);
    
        if (groundPlane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);
    
        return Vector3.zero;
    }
}