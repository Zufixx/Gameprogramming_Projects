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
    private int rayMask;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        rayDirection = -Vector2.up;
        rayDistance = 10000.0f;
        rayMask = 2;
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
	}

    private void FixedUpdate()
    {
        rayOrigin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, rayMask);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        if (hit.collider != null)
        {
            rb.velocity = new Vector2(0.0f, jumpHeight);
            Debug.Log("A hit!");
        }
    }
}
