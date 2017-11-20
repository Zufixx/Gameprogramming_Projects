﻿using System.Collections;
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

    [SerializeField]
    private float maxHeight;

    [SerializeField]
    private PlatformManager platformManager;

    [SerializeField]
    private GameObject bullet;

    private enum types { };
    private types type;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        //types = platformManager.types;
    }
	
	// Update is called once per frame
	void Update () {
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

        /* Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0.0f, jumpHeight);
        }
        */

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

        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        // Raycasting variables
        rayOrigin = transform.position;
        //rayOrigin = transform.position - new Vector3(0.0f, 0.4f, 0.0f);
        rayDirection = -Vector2.up;
        rayDistance = 0.42f;
        LayerMask layer = 1 << 0;

        // Shoot ray down from player
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);

        // Debug Drawing
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        //Debug.Log(hit);

        // If it hits a platform
        if (hit.collider != null && rb.velocity.y <= 0.0f)
        {
            Platform platform = hit.transform.gameObject.GetComponent<Platform>();

            /*
            if (platform.type == Platform.types.FRAGILE)
            {
                Destroy(platform.gameObject);
                return;
            }
            else
            {
            */
                // Jump with the height set on the platform
                jumpHeight = platform.GetJumpHeight();
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            //}


            //Debug.Log("A hit!");
        }
    }

    public float GetMaxHeight()
    {
        return maxHeight;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Boundary")
            GameManager.instance.GameOver();
    }
}
