using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public float lineWidth = 2f;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(topLeft.x, topLeft.y, 0);
        positions[1] = new Vector3(bottomRight.x, topLeft.y, 0);
        positions[2] = new Vector3(bottomRight.x, bottomRight.y, 0);
        positions[3] = new Vector3(topLeft.x, bottomRight.y, 0);
        positions[4] = positions[0]; // 回到起点

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
