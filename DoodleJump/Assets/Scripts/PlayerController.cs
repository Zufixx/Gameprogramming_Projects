using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float moveHorizontal;

    [SerializeField]
    private float speed;
    private float jumpHeight;

    private Vector2 rayOrigin;
    private Vector2 rayDirection;
    private float rayDistance;

    private float maxHeight;

    [SerializeField]
    private GameObject bullet;

    [Header("PowerUp Settings")]
    public bool powerUp = false;
    [SerializeField]
    private float powerUpTimer = 3f;
    private float powerUpTimerStart;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        powerUpTimerStart = powerUpTimer;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.gameOver)
        {
            // Movement forces
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            rb.AddForce(movement * speed);

            // Flip character when moving side to side
            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                gameObject.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }

            // Teleport from side to side
            if (transform.position.x >= 3.15f)
            {
                transform.position = new Vector2(-3.14f, transform.position.y);
            }
            else if (transform.position.x <= -3.15f)
            {
                transform.position = new Vector2(3.14f, transform.position.y);
            }

            // Set max height
            if (maxHeight < transform.position.y)
                maxHeight = transform.position.y;

            if (Input.GetButtonDown("Fire1"))
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
        }

        if(powerUp && powerUpTimer >= 0f)
        {
            rb.AddForce(new Vector2(0f, 15f));
            powerUpTimer -= Time.deltaTime;
            sr.color = Color.red;
        }
        else if (powerUpTimer < 0f)
        {
            powerUp = false;
            powerUpTimer = powerUpTimerStart;
            sr.color = Color.white;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.gameOver)
        {
            // Raycasting variables
            rayOrigin = transform.position;
            rayDirection = -Vector2.up;
            rayDistance = 0.42f;
            LayerMask layer = 1 << 0;

            // Shoot ray down from player
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);

            // Debug Drawing
            Color color = hit ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, rayDirection, color);

            // If it hits a platform
            if (hit.collider != null && rb.velocity.y <= 0.0f)
            {
                if (hit.transform.tag == "Platform" || hit.transform.tag == "Ground")
                {
                    Platform platform = hit.transform.gameObject.GetComponent<Platform>();

                    if (platform.fragile && platform.jumpHeight <= 0f)
                    {
                        Destroy(platform.gameObject);
                        return;
                    }
                    else if (platform.fragile)
                    {
                        // Jump with the height set on the platform
                        jumpHeight = platform.GetJumpHeight();
                        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

                        Destroy(platform.gameObject);
                        return;
                    }
                    else
                    {
                        // Jump with the height set on the platform
                        jumpHeight = platform.GetJumpHeight();
                        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && !powerUp)
        {
            GameManager.instance.GameOver();
            sr.enabled = false;
        }
        else if (collision.transform.tag == "Powerup")
        {
            //Debug.Log("POWER-UP");
            powerUp = true;
            Destroy(collision.transform.gameObject);
        }
    }

    public float GetMaxHeight() { return maxHeight; }
}
