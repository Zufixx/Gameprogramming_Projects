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

	private void Start ()
    {
        Initialize();
    }
	
	private void Update ()
    {
        CamDistActivation();
	}

    private void FixedUpdate()
    {
        if (isActivated && isAlive)
        {
            Movement();
            RaycastPathFinding();
        }
    }

    private void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ft = GetComponent<FlipTimer>();
        ft.enabled = false;
        cam = GameObject.Find("Main Camera").transform;
        Physics2D.IgnoreCollision(cam.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void CamDistActivation()
    {
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

    private void Movement()
    {
        if (goingLeft)
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void RaycastPathFinding()
    {
        Vector3 rayOrigin;
        if (goingLeft)
            rayOrigin = transform.position + new Vector3(-0.55f, 0f);
        else
            rayOrigin = transform.position + new Vector3(0.55f, 0f);

        Vector3 rayDirection = rb.velocity;
        float rayDistance = 0.1f;
        LayerMask layer = 1 << 0;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        if (hit.collider != null)
        {
            if (hit.transform.tag != "Player")
                goingLeft = !goingLeft;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Obstacles")
        {

        }
        else if (collision.transform.tag == "Player")
        {
            float distanceY =  collision.transform.position.y - transform.position.y;
            GameObject player = collision.gameObject;
            int state = player.GetComponent<PlayerController>().state;
            if (state == 0 && distanceY < 0.5f)
                collision.gameObject.GetComponent<PlayerHit>().Hit();
            else if(state != 0 && distanceY < 1f)
                collision.gameObject.GetComponent<PlayerHit>().Hit();
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
