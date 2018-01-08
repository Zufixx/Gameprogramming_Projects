using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualGrid : MonoBehaviour
{
    public float spriteWidth = 16f;
    public float spriteHeight = 16f;
    public float pixelsPerUnit = 16f;

    public Color color = Color.white;

    private void OnDrawGizmos()
    {
        float width = spriteWidth / pixelsPerUnit;
        float height = spriteHeight / pixelsPerUnit;
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = color;

        // X Lines
        for (float y = pos.y - Screen.height; y < pos.y + Screen.height; y += height)
        {
            Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / height) * height), 
                            new Vector3(1000000.0f, Mathf.Floor(y / height) * height));
        }

        // Y Lines
        for (float x = pos.x - Screen.width; x < pos.x + Screen.width; x += width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, 1000000.0f),
                            new Vector3(Mathf.Floor(x / width) * width, -1000000.0f));
        }
    }

}
