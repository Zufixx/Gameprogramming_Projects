using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField]
    private PlatformManager platformManager;

    private SpriteRenderer sr;

    public enum types {STANDARD, GROUND, FRAGILE, MOVINGX, MOVINGY, HIGHJUMP}
    public types type;

    private float jumpHeight;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        switch (type)
        {
            case types.STANDARD:
                jumpHeight = 10f;
                sr.color = Color.white;
                break;
            case types.GROUND:
                jumpHeight = 10f;
                sr.color = Color.gray;
                break;
            case types.FRAGILE:
                jumpHeight = 0f;
                sr.color = Color.yellow;
                break;
            case types.MOVINGX:
                break;
            case types.MOVINGY:
                break;
            case types.HIGHJUMP:
                jumpHeight = 20f;
                sr.color = Color.blue;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (type != types.GROUND)
        {
            if (other.tag == "Boundary")
                platformManager.RespawnPlatform(gameObject);
        }
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }
}
