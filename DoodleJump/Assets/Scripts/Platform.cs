using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField]
    private PlatformManager platformManager;

    private SpriteRenderer sr;

    public enum types {STANDARD, GROUND, FRAGILE, MOVINGX, MOVINGY, HIGHJUMP}
    public types type;

    public float jumpHeight;

    private float xSpeed;
    private float ySpeed;
    private Vector2 movement;

    private float moveInterval;

    public void Initialize(int rng)
    {
        sr = GetComponent<SpriteRenderer>();

        xSpeed = 0f;
        ySpeed = 0f;

        moveInterval = 0.5f;

        switch (rng)
        {
            case 0: case 1: case 2: case 3:
                type = types.STANDARD;
                jumpHeight = 10f;
                sr.color = Color.white;
                break;
            case 4:
                type = types.FRAGILE;
                jumpHeight = 0f;
                sr.color = Color.yellow;
                break;
            case 5:
                type = types.MOVINGX;
                jumpHeight = 10f;
                sr.color = Color.blue;
                xSpeed = 0.01f;
                break;
            case 6:
                type = types.MOVINGY;
                jumpHeight = 10f;
                sr.color = Color.cyan;
                ySpeed = 0.01f;
                break;
            case 7:
                type = types.HIGHJUMP;
                jumpHeight = 20f;
                sr.color = Color.red;
                break;
        }

        movement = new Vector2(xSpeed, ySpeed);
    }

    private void Update()
    {
        if (xSpeed != 0f || ySpeed != 0f)
        {
            moveInterval -= Time.deltaTime;

            if (moveInterval > 0)
            {
                transform.Translate(movement, Space.Self);
            }
            else if (moveInterval <= 0)
            {
                moveInterval = 1.0f;
                movement = -movement;
            }

            transform.Translate(movement, Space.Self);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (type != types.GROUND)
        {
           if (other.tag == "Boundary")
                Destroy(gameObject);
        }
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }
}
