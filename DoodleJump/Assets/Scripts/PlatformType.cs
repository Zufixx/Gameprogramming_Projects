using UnityEngine;

public class PlatformType
{
    public string typeName;
    public float width;
    public float jumpHeight;
    public bool fragile;
    public Color color;
    public float probability;
    public Vector2 speed;
    public float time;
    public float space;

    public PlatformType(
        string typeName,
        float width,
        float jumpHeight,
        bool fragile,
        float colorR,
        float colorG,
        float colorB,
        float colorA,
        float probability,
        float movementX,
        float movementY,
        float time,
        float space)
    {
        this.typeName = typeName;
        this.width = width;
        this.jumpHeight = jumpHeight;
        this.fragile = fragile;
        this.probability = probability;
        this.time = time;
        this.space = space;

        color = new Color(colorR, colorG, colorB, colorA);
        speed = new Vector2(movementX, movementY);

    }
}