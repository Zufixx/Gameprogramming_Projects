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

    private Vector3 startPos;
    private Vector3 endPos;

    private bool startPosSet = false;
    private bool transitioning = false;
    bool right = false;

    [SerializeField]
    private float transitionAmount;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        startUpFlipTime = upFlipTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (!transitioning)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal < -0.1f)
            {
                // Walk Left
                direction = 1;
                walking = true;
                right = false;
                rb.velocity = new Vector2(-speed, 0f);

            }
            else if (horizontal > 0.1f)
            {
                // Walk Right
                direction = 1;
                walking = true;
                right = true;
                rb.velocity = new Vector2(speed, 0f);

            }
            else if (vertical < -0.1f)
            {
                // Walk Down
                direction = 0;
                walking = true;
                rb.velocity = new Vector2(0f, -speed);

            }
            else if (vertical > 0.1f)
            {
                // Walk Up
                direction = 2;
                walking = true;
                rb.velocity = new Vector2(0f, speed);
            }
            else
            {
                rb.velocity = Vector2.zero;
                walking = false;
            }

            moveAnimation(direction, walking);
        }
    }

    void moveAnimation(int direction, bool walking)
    {
        anim.SetInteger("direction", direction);
        anim.SetBool("walk", walking);

        if (direction == 2 && flipTimer() && walking)
        {
            sr.flipX = !sr.flipX;
        }
        else if(direction == 1 && !right)
        {
            sr.flipX = true;
        }
        else if (direction != 2)
        {
            sr.flipX = false;
        }
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

    public void TransitionPlayer(int getDirection, float timer, Vector3 diffPos)
    {
        if (!startPosSet)
        {
            startPos = transform.position;
            startPosSet = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }

        TransitionAnim(getDirection);
        transitioning = true;

        endPos = startPos + diffPos / transitionAmount;
        transform.position = Vector3.Lerp(startPos, endPos, timer);
    }

    private void TransitionAnim(int getDirection)
    {
        walking = true;

        switch(getDirection)
        {
            case 1:
                direction = 0;
                break;
            case 2:
                direction = 1;
                break;
            case 3:
                direction = 2;
                break;
            case 4:
                direction = 1;
                break;
        }

        moveAnimation(direction, true);
    }

    public void ResetTransition()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        startPosSet = false;
        transform.position = endPos;
        transitioning = false;
        walking = false;
    }
}
