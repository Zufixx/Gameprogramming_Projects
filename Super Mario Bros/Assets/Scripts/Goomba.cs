using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private FlipTimer ft;
    private Transform cam;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool goingLeft = true;
    [SerializeField]
    public bool isActivated = false;
    [SerializeField]
    private float camDistance = 0f;

    public bool isAlive = true;

    [SerializeField]
    private Sprite deadSprite;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ft = GetComponent<FlipTimer>();
        ft.enabled = false;
        cam = GameObject.Find("Main Camera").transform;
        Physics2D.IgnoreCollision(cam.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update () {
        camDistance = transform.position.x - cam.position.x;
        if (!isActivated && camDistance < 10f)
        {
            isActivated = true;
            ft.enabled = true;
        }
        else if (camDistance < -10f)
        {
            Destroy(gameObject);
        }
	}

    private void FixedUpdate()
    {
        if (isActivated && isAlive)
        {
            if (goingLeft)
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            // Raycasting variables
            Vector3 rayOrigin;
            if (goingLeft)
                rayOrigin = transform.position + new Vector3(-0.55f, 0f);
            else
            {
                rayOrigin = transform.position + new Vector3(0.55f, 0f);
            }
            Vector3 rayDirection = rb.velocity;
            float rayDistance = 0.1f;
            LayerMask layer = 1 << 0;

            // Shoot ray down from player
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
            Color color = hit ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, rayDirection, color);

            if(hit.collider != null)
            {
                if(hit.transform.tag == "Player")
                    hit.transform.gameObject.GetComponent<PlayerController>().Hit();
                else
                    goingLeft = !goingLeft;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Obstacles")
        {

        }
        else if (collision.transform.tag == "Player")
        {
            float distanceY = Mathf.Abs(transform.position.y - collision.transform.position.y);
            if (distanceY < 0.5f)
            {
                collision.gameObject.GetComponent<PlayerController>().Hit();
            }
        }
    }

    public void Die()
    {
        isAlive = false;
        GetComponent<Collider2D>().enabled = false;
        sr.sprite = deadSprite;
        ft.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(gameObject, 0.4f);
    }
}
