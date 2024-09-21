using UnityEngine;

public static class DebugUtils
{
    public static void DrawCross(Vector3 center, float radius, Color color)
    {
        Debug.DrawLine(center + Vector3.up * radius, center + Vector3.down * radius, color);
        Debug.DrawLine(center + Vector3.right * radius, center + Vector3.left * radius, color);
        Debug.DrawLine(center + Vector3.forward * radius, center + Vector3.back * radius, color);
    }
}