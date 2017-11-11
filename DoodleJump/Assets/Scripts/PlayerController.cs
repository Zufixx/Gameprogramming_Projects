using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float moveHorizontal;

    public float speed;
    public float jumpHeight;

    private Vector2 rayOrigin;
    private Vector2 rayDirection;
    private float rayDistance;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0.0f);
        rb.AddForce(movement * speed);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0.0f, jumpHeight);
        }
        rayOrigin = transform.position - new Vector3(0.0f, 0.4f, 0.0f);

        if(transform.position.x >= 3.15f)
        {
            transform.position = new Vector2(-3.14f, transform.position.y);
        }
        else if (transform.position.x <= -3.15f)
        {
            transform.position = new Vector2(3.14f, transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        rayDirection = -Vector2.up;
        rayDistance = 0.02f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        //Debug.Log(hit);

        if (hit.collider != null && rb.velocity.y <= 0.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //Debug.Log("A hit!");
        }
    }
}
