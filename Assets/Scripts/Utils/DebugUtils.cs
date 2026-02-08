using UnityEngine;

public static class DebugUtils
{
    public enum Plane
    {
        XY,
        XZ,
        YZ
    }

    /// <summary>
    /// Draws a line from start to end
    /// </summary>
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
    {
        Debug.DrawLine(start, end, color, duration);
    }
    
    /// <summary>
    /// Draws a circle on the XZ plane (horizontal)
    /// </summary>
    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32, float duration = 0f)
    {
        DrawCircleOnPlane(center, radius, Plane.XZ, color, segments, duration);
    }
    
    /// <summary>
    /// Draws a wireframe sphere
    /// </summary>
    public static void DrawSphere(Vector3 center, float radius, Color color, int segments = 16, float duration = 1f)
    {
        DrawCircleOnPlane(center, radius, Plane.XY, color, segments, duration);
        DrawCircleOnPlane(center, radius, Plane.XZ, color, segments, duration);
        DrawCircleOnPlane(center, radius, Plane.YZ, color, segments, duration);
    }
    
    private static void DrawCircleOnPlane(Vector3 center, float radius, Plane plane, Color color, int segments, float duration)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = GetPointOnPlane(center, radius, 0f, plane);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = GetPointOnPlane(center, radius, angle, plane);
            Debug.DrawLine(prevPoint, newPoint, color, duration);
            prevPoint = newPoint;
        }
    }
    
    private static Vector3 GetPointOnPlane(Vector3 center, float radius, float angle, Plane plane)
    {
        float cos = Mathf.Cos(angle) * radius;
        float sin = Mathf.Sin(angle) * radius;
        
        return plane switch
        {
            Plane.XY => center + new Vector3(cos, sin, 0),
            Plane.XZ => center + new Vector3(cos, 0, sin),
            Plane.YZ => center + new Vector3(0, cos, sin),
            _ => center
        };
    }
}