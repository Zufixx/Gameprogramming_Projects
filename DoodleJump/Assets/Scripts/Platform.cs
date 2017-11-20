using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField]
    private PlatformManager platformManager;

    private SpriteRenderer sr;

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
                jumpHeight = 10f;
                sr.color = Color.green;
                break;
            case 4:
                jumpHeight = 0f;
                sr.color = Color.yellow;
                break;
            case 5:
                jumpHeight = 10f;
                sr.color = Color.blue;
                xSpeed = 0.02f;
                break;
            case 6:
                jumpHeight = 10f;
                sr.color = Color.cyan;
                ySpeed = 0.02f;
                break;
            case 7:
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
                transform.Translate(movement * Time.deltaTime, Space.Self);
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
        /*
        if (type != types.GROUND)
        {
           if (other.tag == "Boundary")
                Destroy(gameObject);
        }
        */
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }
}
