using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 0;

    // 0 = down, 1 = sides, 2 = up
    private int direction = 0;
    private bool walking = false;

    [SerializeField]
    private float upFlipTime;
    private float startUpFlipTime;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        startUpFlipTime = upFlipTime;
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(horizontal < -0.1f)
        {
            // Walk Left
            direction = 1;
            walking = true;
            rb.velocity = new Vector2(-speed, 0f);
            moveAnimation(direction, walking);
            sr.flipX = true;

        }
        else if (horizontal > 0.1f)
        {
            // Walk Right
            direction = 1;
            walking = true;
            rb.velocity = new Vector2(speed, 0f);
            moveAnimation(direction, walking);
            sr.flipX = false;

        }
        else if (vertical < -0.1f)
        {
            // Walk Down
            direction = 0;
            walking = true;
            rb.velocity = new Vector2(0f, -speed);
            moveAnimation(direction, walking);
            sr.flipX = false;

        }
        else if (vertical > 0.1f)
        {
            // Walk Up
            direction = 2;
            walking = true;
            rb.velocity = new Vector2(0f, speed);
            moveAnimation(direction, walking);

            if(flipTimer())
            {
                sr.flipX = !sr.flipX;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            walking = false;
            moveAnimation(direction, walking);
        }
    }

    void moveAnimation(int direction, bool walking)
    {
        anim.SetInteger("direction", direction);
        anim.SetBool("walk", walking);
    }

    bool flipTimer()
    {
        if(upFlipTime > 0f)
        {
            upFlipTime -= Time.deltaTime;
            return false;
        }
        else
        {
            upFlipTime = startUpFlipTime;
            return true;
        }
    }
}
