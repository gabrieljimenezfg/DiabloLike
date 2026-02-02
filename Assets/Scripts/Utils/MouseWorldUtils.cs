using UnityEngine;

public static class MouseWorldUtils
{
    private static readonly Camera camera = Camera.main;

    private static readonly LayerMask groundLayer =
        LayerMask.GetMask("Ground");

    public static bool TryGetMousePositionOnGround(
        out Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            position = hit.point;
            return true;
        }

        position = default;
        return false;
    }
}