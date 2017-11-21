using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField]
    private PlatformManager platformManager;

    private SpriteRenderer sr;

    public string typeName;
    public bool fragile;
    public float jumpHeight;

    private float xSpeed;
    private float ySpeed;
    private Vector2 movement;

    private float moveInterval;
    private float startInterval;

    public void Initialize(PlatformType pf)
    {
        sr = GetComponent<SpriteRenderer>();

        typeName = pf.typeName;
        fragile = pf.fragile;
        jumpHeight = pf.jumpHeight;
        sr.color = pf.color;
        movement = pf.speed * 0.02f;
        moveInterval = pf.time;
        startInterval = pf.time;
    }

    private void FixedUpdate()
    {
        #region Platform Movement
        if (movement.x != 0f || movement.y != 0f)
        {
            moveInterval -= Time.deltaTime;

            if (moveInterval > 0)
            {
                transform.Translate(movement * Time.deltaTime, Space.Self);
            }
            else if (moveInterval <= 0)
            {
                moveInterval = startInterval * 2;
                movement = -movement;
            }

            transform.Translate(movement, Space.Self);
        }
        #endregion
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Boundary" && transform.tag != "Ground")
                Destroy(gameObject);
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }
}
