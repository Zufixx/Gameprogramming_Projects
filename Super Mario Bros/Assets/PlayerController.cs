using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField]
    bool isGrounded = true;

    [SerializeField]
    float horizontal = 0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
        GroundCheck();
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
	}

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < -0.1f)
            sr.flipX = true;
        else if(horizontal > 0.1f)
            sr.flipX = false;
        
		rb.velocity = new Vector2(horizontal * 10f, rb.velocity.y);
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

        // Debug Drawing
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        // If it hits a platform
        if (hit.collider != null)
        {
            if (hit.transform.tag == "Ground")
                isGrounded = true;
            else
                isGrounded = false;
        }
        else
            isGrounded = false;
    }

    private void Jump()
    {
        if(isGrounded)
            rb.AddForce(new Vector2(rb.velocity.x, 10f), ForceMode2D.Impulse);
    }
}
