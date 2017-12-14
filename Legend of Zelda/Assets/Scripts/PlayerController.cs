using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 0;

    // 0 = down, 1 = sides, 2 = up
    // 0 = static, 1 = down, 2 = left, 3 = up, 4 = right
    [SerializeField]
    private int direction = 1;
    // 0 = none, 1 = walking, 2 = attacking
    [SerializeField]
    private int state = 0;

    public int health = 6;
    public int maxHealth = 6;

    public int rupees = 0;

    [SerializeField]
    private float upFlipTime;
    private float startUpFlipTime;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool startPosSet = false;
    private bool camTransitioning = false;
    bool right = false;

    [SerializeField]
    [Range(0.1f,10f)]
    private float camTransitionAmount;

    private bool enterTransition = false;

    private Transform mainCamera;

    [SerializeField]
    private bool swordGet = false;
    [SerializeField]
    private GameObject sword;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main.transform;

        startUpFlipTime = upFlipTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (!camTransitioning && !enterTransition)
        {
            Movement();

            if(swordGet)
                Attacking();

            MoveAnimation(direction, state);
        }
        if (enterTransition)
        {

        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(state == 2)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else if (horizontal < -0.1f)
        {
            // Walk Left
            direction = 2;
            state = 1;
            right = false;
            rb.velocity = new Vector2(-speed, 0f);

        }
        else if (horizontal > 0.1f)
        {
            // Walk Right
            direction = 4;
            state = 1;
            right = true;
            rb.velocity = new Vector2(speed, 0f);

        }
        else if (vertical < -0.1f)
        {
            // Walk Down
            direction = 1;
            state = 1;
            rb.velocity = new Vector2(0f, -speed);

        }
        else if (vertical > 0.1f)
        {
            // Walk Up
            direction = 3;
            state = 1;
            rb.velocity = new Vector2(0f, speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            state = 0;
        }
    }

    void MoveAnimation(int direction, int state)
    {
        anim.SetInteger("direction", direction);
        anim.SetInteger("state", state);

        if (direction == 3 && flipTimer() && state == 1)
        {
            sr.flipX = !sr.flipX;
        }
        else if (direction == 2)
        {
            sr.flipX = true;
        }
        else if (direction != 3)
        {
            sr.flipX = false;
        }
    }

    bool flipTimer()
    {
        if (upFlipTime > 0f)
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

    public void CamTransitionPlayer(int getDirection, float timer, Vector3 diffPos)
    {
        if (!startPosSet)
        {
            startPos = transform.position;
            startPosSet = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }

        CamTransitionAnim(getDirection);
        camTransitioning = true;

        endPos = startPos + diffPos / camTransitionAmount;
        transform.position = Vector3.Lerp(startPos, endPos, timer);
    }

    private void CamTransitionAnim(int getDirection)
    {
        state = 1;

        direction = getDirection;

        MoveAnimation(direction, 1);
    }

    public void ResetCamTransition()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        startPosSet = false;
        transform.position = endPos;
        camTransitioning = false;
        state = 0;
    }

    public void EnterTransition()
    {

    }

    private void Attacking()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (!sword.activeSelf)
            {
                sword.SetActive(true);
                state = 2;

                Quaternion swordRot = new Quaternion();
                float swordX = 0f;
                float swordY = 0f;

                switch (direction)
                {
                    case 1:
                        // Down
                        swordRot = Quaternion.Euler(0f, 0f, 270f);
                        swordY = -0.7f;
                        sr.flipX = false;
                        break;
                    case 2:
                        // Left
                        swordRot = Quaternion.Euler(0f, 0f, 180f);
                        swordX = -0.7f;
                        sr.flipX = true;
                        break;
                    case 3:
                        // Up
                        swordRot = Quaternion.Euler(0f, 0f, 90f);
                        swordY = 0.7f;
                        sr.flipX = false;
                        break;
                    case 4:
                        // Right
                        swordRot = swordRot = Quaternion.Euler(0f, 0f, 0f);
                        swordX = 0.7f;
                        sr.flipX = false;
                        break;
                }
                sword.transform.rotation = swordRot;
                sword.transform.position = transform.position + new Vector3(swordX, swordY);

                StartCoroutine(swordTimer());
            }
        }
        if(state == 2)
        {
            anim.SetInteger("direction", direction);
            anim.SetInteger("state", state);
        }
    }

    private IEnumerator swordTimer()
    {
        yield return new WaitForSeconds(0.2f);
        sword.SetActive(false);
        state = 0;
        anim.SetInteger("direction", direction);
        anim.SetInteger("state", state);
    }

    public void Pickup(string pickup)
    {
        switch(pickup)
        {
            case "Sword":
                swordGet = true;
                break;
        }
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
    }
}
