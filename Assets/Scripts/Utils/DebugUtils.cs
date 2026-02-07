using UnityEngine;

public static class DebugUtils
{
    /// <summary>
    /// Draws a line from start to end
    /// </summary>
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        Debug.DrawLine(start, end, color);
    }
    
    /// <summary>
    /// Draws a circle on the XZ plane (horizontal)
    /// </summary>
    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
    }
}