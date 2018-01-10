using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D[] cols = new CapsuleCollider2D[2];
    private PlayerHit ph;

    [Header("Gameplay")]
    [SerializeField]
    public bool isAlive = true;
    public int state = 0;
    [SerializeField]
    private Sprite[] sprites;

    [Header("Grounded")]
    [SerializeField]
    private bool isGrounded = true;

    [SerializeField]
    private float groundedTimer = 1f;
    private float startGrounderTimer;

    private float horizontal;

    [Header("Speed")]
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float airSpeed;

    [SerializeField]
    private float maxSpeed;

    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float jumpFloat;
    [SerializeField]
    private float enemyBounce;
    [SerializeField]
    private float downForce;

    [Header("Physics")]
    [SerializeField]
    private PhysicsMaterial2D friction;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private float highDrag;
    [SerializeField]
    private float lowDrag;

    private bool hasJumped = false;

    private float raycastOffsetX = 0.2f;
    private float raycastOffsetY = 0f;

    private void Start ()
    {
        Initialize();
    }
	
	private void Update ()
    {
        if (!isAlive)
            ph.DeathAnimation();
        else
        {
            Movement();
            GroundedTimer();
            SetFriction();
            Jumping();
            ph.Cooldown();
        }

        if (isAlive && transform.position.y < -2f)
        {
            ph.Die();
        }
    }

    private void FixedUpdate()
    {
        if(isAlive)
            GroundCheck();
    }

    private void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cols = GetComponents<CapsuleCollider2D>();
        ph = GetComponent<PlayerHit>();

        startGrounderTimer = groundedTimer;

        ph.Initialize();
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < -0.1f)
            sr.flipX = true;
        else if(horizontal > 0.1f)
            sr.flipX = false;

        if (isGrounded && Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector2(horizontal * runSpeed * Time.deltaTime * 60f, 0f), ForceMode2D.Impulse);
            rb.sharedMaterial = friction;
            rb.drag = lowDrag;

        }
        else if (!isGrounded && Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector2(horizontal * airSpeed * Time.deltaTime * 60f, 0f), ForceMode2D.Impulse);
            rb.sharedMaterial = noFriction;
            rb.drag = lowDrag;
        }
        else
        {
            rb.sharedMaterial = friction;
            rb.drag = lowDrag;
        }

        if (horizontal < 0.1f && horizontal > -0.1f && isGrounded)
        {
            rb.sharedMaterial = friction;
            rb.drag = highDrag;
        }
    }

    private void GroundCheck()
    {
        Vector3 rayOrigin = transform.position + new Vector3(0f, raycastOffsetY);
        Vector3 rayDirection = -Vector2.up;
        float rayDistance = 0.55f;
        LayerMask layer = 1 << 0;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        rayOrigin = transform.position + new Vector3(raycastOffsetX, raycastOffsetY);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color colorL = hitLeft ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, colorL);

        rayOrigin = transform.position + new Vector3(-raycastOffsetX, raycastOffsetY);
        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color colorR = hitRight ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, colorR);

        if (hit.collider != null)
            HitGround(hit);
        else if (hitLeft.collider != null)
            HitGround(hitLeft);
        else if (hitRight.collider != null)
            HitGround(hitRight);
        else
            isGrounded = false;
    }

    private void SetFriction()
    {
        if (!isGrounded)
        {
            rb.sharedMaterial = noFriction;
            rb.drag = lowDrag;
            if (rb.velocity.y < 0f)
                groundedTimer = 0f;
        }
        else
            rb.sharedMaterial = friction;
    }

    private void Jumping()
    {
        if (Input.GetKey(KeyCode.Space))
            Jump();
        else
            groundedTimer = 0f;
    }

    private void Downforce()
    {
        if (rb.velocity.y < 0f)
            rb.AddForce(new Vector2(0f, downForce), ForceMode2D.Force);
    }

    private void HitGround(RaycastHit2D hit)
    {
        if (hit.transform.tag == "Ground" || hit.transform.tag == "Obstacles" || hit.transform.tag == "HitFromBelow")
            isGrounded = true;
        else if (hit.transform.tag == "Goomba")
        {
            Goomba goomba = hit.transform.GetComponent<Goomba>();
            if (goomba.isAlive)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, jumpHeight * enemyBounce), ForceMode2D.Impulse);
                goomba.Die();
            }
        }
        else
            isGrounded = false;
    }

    private void GroundedTimer()
    {
        if(!isGrounded && groundedTimer > 0f)
            groundedTimer -= Time.deltaTime;
        else if (isGrounded)
        {
            groundedTimer = startGrounderTimer;
            hasJumped = false;
        }
    }

    private void Jump()
    {
        if(isGrounded && !hasJumped && rb.velocity.y < 0.5f)
        {
            rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            hasJumped = true;
        }
        else if (groundedTimer > 0f)
            rb.AddForce(new Vector2(0f, jumpFloat * Time.deltaTime * 30f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "HitFromBelow" && rb.velocity.y > -0.2f)
        {
            float distanceY = transform.position.y - collision.transform.position.y;
            if(distanceY < -0.9f)
            {
                SpawnBlock spawnBlock = collision.gameObject.GetComponent<SpawnBlock>();
                if (!spawnBlock.isUsed)
                    spawnBlock.HitFromBelow(state);
            }
        }
        else if(collision.transform.tag == "FlagPole")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("EndFlag").GetComponent<EndFlag>().goUp = true;
        }
    }

    public void PowerUp(int powerState)
    {
        if (powerState != 0)
            ChangeState(powerState);
    }

    public void ChangeState(int newState)
    {
        if (newState == state)
            return;

        state = newState;
        sr.sprite = sprites[state];

        switch (state)
        {
            case 0:
                transform.position = transform.position + new Vector3(0f, -0.5f);

                foreach (CapsuleCollider2D col in cols)
                    col.size = new Vector2(0.6f, 1f);

                raycastOffsetY = 0f;
                raycastOffsetX = 0.2f;
                break;
            case 1:
            case 2:
                if(cols[0].size != new Vector2(1f, 2f))
                    transform.position = transform.position + new Vector3(0f, 0.5f);

                foreach (CapsuleCollider2D col in cols)
                    col.size = new Vector2(1f, 2f);

                raycastOffsetY = -0.5f;
                raycastOffsetX = 0.4f;
                break;
        }
    }
}
