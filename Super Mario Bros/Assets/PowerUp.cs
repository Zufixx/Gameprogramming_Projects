using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private Rigidbody2D rb;
    private Transform cam;

    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool goingLeft = true;
    [SerializeField]
    private bool isOneUp = false;

    private float camDistance;
    private bool transitionDone = false;

    [SerializeField]
    private float floatUpDelay = 1f;

    private float floatUpTimer = 0f;
    private Vector3 startPosition;

    private Collider2D col;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").transform;
        Physics2D.IgnoreCollision(cam.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("Mario").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        startPosition = transform.position;
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionDone)
        {
            camDistance = transform.position.x - cam.position.x;
            if (camDistance < -10f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0f, 1f), floatUpTimer);
        }

        if (floatUpTimer < 1f && !transitionDone)
        {
            floatUpTimer += Time.deltaTime / floatUpDelay;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            transitionDone = true;
            col.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (transitionDone)
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

            if (hit.collider != null)
            {
                goingLeft = !goingLeft;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Ground" || collision.transform.tag == "Obstacles")
        {

        }
        else if(collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().PowerUp(isOneUp);
            Destroy(gameObject);
        }
    }
}
