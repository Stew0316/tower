using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squa : MonoBehaviour
{
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public float lineWidth = 2f;
    public Color color = Color.red;

    void OnGUI()
    {
        Vector2 size = new Vector2(bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);

        // ������������
        DrawLine(new Vector2(topLeft.x, topLeft.y), new Vector2(bottomRight.x, topLeft.y), lineWidth, color); // �ϱ�
        DrawLine(new Vector2(bottomRight.x, topLeft.y), new Vector2(bottomRight.x, bottomRight.y), lineWidth, color); // �ұ�
        DrawLine(new Vector2(bottomRight.x, bottomRight.y), new Vector2(topLeft.x, bottomRight.y), lineWidth, color); // �±�
        DrawLine(new Vector2(topLeft.x, bottomRight.y), new Vector2(topLeft.x, topLeft.y), lineWidth, color); // ���
    }

    void DrawLine(Vector2 pointA, Vector2 pointB, float width, Color color)
    {
        Color originalColor = GUI.color;
        Matrix4x4 matrix = GUI.matrix;

        GUI.color = color;
        float angle = Vector3.Angle(pointB - pointA, Vector2.right);
        if (pointA.y > pointB.y) angle = -angle;
        GUIUtility.RotateAroundPivot(angle, pointA);
        GUI.DrawTexture(new Rect(pointA.x, pointA.y - width / 2, (pointB - pointA).magnitude, width), Texture2D.whiteTexture);

        GUI.matrix = matrix;
        GUI.color = originalColor;
    }
}
