using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;

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

    [Header("Physics")]
    [SerializeField]
    private PhysicsMaterial2D friction;
    [SerializeField]
    private PhysicsMaterial2D noFriction;

    private bool hasJumped = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        startGrounderTimer = groundedTimer;
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
        GroundedTimer();

        if (!isGrounded)
            rb.sharedMaterial = noFriction;
	}

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        else
        {
            groundedTimer = 0f;
        }

        if(rb.velocity.y < 0f)
        {
            rb.AddForce(new Vector2(0f, -4), ForceMode2D.Force);
        }

        GroundCheck();
        //UpCheck();
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
        }
        else if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector2(horizontal * airSpeed * Time.deltaTime * 60f, 0f), ForceMode2D.Impulse);
            rb.sharedMaterial = noFriction;
        }
        else
        {
            rb.sharedMaterial = friction;
        }
    }

    private void GroundCheck()
    {
        // Raycasting variables
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = -Vector2.up;
        float rayDistance = 0.55f;
        LayerMask layer = 1 << 0;

        // Shoot ray down from player
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        rayOrigin = transform.position + new Vector3(0.2f, 0f);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color colorL = hitLeft ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, colorL);

        rayOrigin = transform.position + new Vector3(-0.2f, 0f);
        RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color colorR = hitRight ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, colorR);

        // If it hits a platform
        if (hit.collider != null)
            HitGround(hit);
        else if (hitLeft.collider != null)
            HitGround(hitLeft);
        else if (hitRight.collider != null)
            HitGround(hitRight);
        else
            isGrounded = false;
    }

    private void HitGround(RaycastHit2D hit)
    {
        if (hit.transform.tag == "Ground" || hit.transform.tag == "Obstacles" || hit.transform.tag == "HitFromBelow")
        {
            isGrounded = true;
        }
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
        {
            groundedTimer -= Time.deltaTime;
        }
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
        {
            rb.AddForce(new Vector2(0f, jumpFloat), ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "HitFromBelow")
        {
            float distanceX = transform.position.x - collision.transform.position.x;
            float distanceY = transform.position.y - collision.transform.position.y;
            if(distanceX < 0.75f && distanceY < -0.5f)
            {
                SpawnBlock spawnBlock = collision.gameObject.GetComponent<SpawnBlock>();
                if (!spawnBlock.isUsed)
                    spawnBlock.HitFromBelow();
            }
        }
        else if(collision.transform.tag == "Powerup")
        {
            Debug.Log(collision.transform.name);
            Destroy(collision.gameObject);
        }
    }

    private void UpCheck()
    {
        // Raycasting variables
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector2.up;
        float rayDistance = 0.55f;
        LayerMask layer = 1 << 0;

        // Shoot ray down from player
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        if (hit.collider != null)
        { 
            if (hit.transform.tag == "HitFromBelow")
            {
                Debug.Log("Hit from below!");
            }
        }
    }

    public void Hit()
    {
        Debug.Log("Hit!");
    }

    public void PowerUp(bool isOneUp)
    {
        if(isOneUp)
        {
            Debug.Log("1 up!");
        }
        else
        {
            Debug.Log("Big Mario!");
        }
    }
}
