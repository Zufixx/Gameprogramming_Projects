using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private Rigidbody2D rb;
    private Transform cam;
    private Collider2D col;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool goingLeft = true;
    [Header("Powerup Type")]
    [SerializeField]
    private int state = 0;

    private float camDistance;
    private bool transitionDone = false;

    [Header("Transition")]
    [SerializeField]
    private float floatUpDelay = 1f;
    [SerializeField]
    private float maxHeight = 1f;

    private float floatUpTimer = 0f;
    private Vector3 startPosition;

    [Header("Raycast")]
    [SerializeField]
    private float rayOffsetX = 0.55f;
    [SerializeField]
    private float rayDistance = 0.1f;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Transition();
    }

    private void FixedUpdate()
    {
        if (transitionDone && state != 2)
        {
            Movement();
            RaycastPathFinding();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().PowerUp(state);
            Destroy(gameObject);
        }
    }

    private void Initialize()
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

    private void Transition()
    {
        if (transitionDone)
        {
            camDistance = transform.position.x - cam.position.x;
            if (camDistance < -10f)
                Destroy(gameObject);
        }
        else
            transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0f, maxHeight), floatUpTimer);

        if (floatUpTimer < 1f && !transitionDone)
            floatUpTimer += Time.deltaTime / floatUpDelay;
        else
        {
            if (state != 2)
                rb.constraints = RigidbodyConstraints2D.None;

            transitionDone = true;
            col.enabled = true;
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
            rayOrigin = transform.position + new Vector3(-rayOffsetX, 0f);
        else
            rayOrigin = transform.position + new Vector3(rayOffsetX, 0f);
        Vector3 rayDirection = rb.velocity.normalized;
        rayDistance = 0.1f;
        LayerMask layer = 1 << 0;


        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        if (hit.collider != null)
            goingLeft = !goingLeft;
    }
}
