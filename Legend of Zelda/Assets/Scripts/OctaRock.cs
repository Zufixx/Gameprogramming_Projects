using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Knockback))]
public class OctaRock : MonoBehaviour {

    [SerializeField]
    private float durationMin;
    [SerializeField]
    private float durationMax;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int health = 3;
    [SerializeField]
    GameObject smokePrefab;
    [SerializeField]
    GameObject rupeePrefab;

    private float duration;
    private int direction;
    private Vector2 directionVector = Vector2.zero;

    private Rigidbody2D rb;
    private Knockback kb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();

        RandomizeDirection();
    }

    private void Update()
    {
        if (duration >= 0 && !kb.knockedBack)
            duration -= Time.deltaTime;
        else if(!kb.knockedBack)
            RandomizeDirection();
    }

    private void RandomizeDirection()
    {
        duration = Random.Range(durationMin, durationMax);  // Set a random duration to walk in set direction
        direction = Random.Range(0, 5);                     // Get a random direction int

        switch (direction)
        {
            case 0:
                directionVector = new Vector2(0f, 1f);      // Down
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case 1:
                directionVector = new Vector2(-1f, 0f);     // Left
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case 2:
                directionVector = new Vector2(0f, -1f);      // Up
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case 3:
                directionVector = new Vector2(1f, 0f);      // Right
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case 4:
                directionVector = new Vector2(0f, 0f);      // Still
                break;
        }
    }

    private void FixedUpdate()
    {
        if(!kb.knockedBack)
            rb.velocity = directionVector * speed;

        RaycastForward();
    }

    private void RaycastForward()
    {
        // Raycasting forward
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = directionVector;
        float rayDistance = 0.42f;
        LayerMask layer = 1 << 0;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);

        if (hit.collider != null)
        {
            if (!hit.transform.CompareTag("Player"))
            {
                // Flip direction
                directionVector = -directionVector;
                transform.Rotate(Vector3.up * 180f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
            collision.transform.gameObject.GetComponent<PlayerController>().LoseHealth(1, transform);

        if (collision.transform.CompareTag("Sword") && !kb.knockedBack)
        {
            health--;
            if (health <= 0)
            {
                Instantiate(smokePrefab, transform.position, Quaternion.identity);
                if (Random.value > 0.5f)
                    Instantiate(rupeePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
                kb.KnockbackFromTransform(collision.transform.parent);
        }
    }
}
