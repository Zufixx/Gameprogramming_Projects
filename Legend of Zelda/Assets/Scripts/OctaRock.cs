using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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

    private float duration;
    private int direction;
    private Vector2 directionVector = Vector2.zero;

    private Rigidbody2D m_rb;
    private SpriteRenderer m_sr;

    private bool knockedBack;
    private float knockedBackTimer = 0.5f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sr = GetComponent<SpriteRenderer>();
        Randomize();
    }

    private void Update()
    {
        if (duration >= 0 && !knockedBack)
            duration -= Time.deltaTime;
        else if(!knockedBack)
            Randomize();

        if(knockedBack)
        {
            if(knockedBackTimer <= 0f)
            {
                knockedBack = false;
                m_sr.color = new Color(1f, 1f, 1f);
                knockedBackTimer = 0.5f;
            }
            else
            {
                directionVector = Vector2.zero;
                knockedBackTimer -= Time.deltaTime;
                m_sr.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void Randomize()
    {
        duration = Random.Range(durationMin, durationMax);

        direction = Random.Range(0, 5);

        Debug.Log("Octarock direction: " + direction.ToString());

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
        m_rb.velocity = directionVector * speed;

        // Raycasting variables
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = directionVector;
        float rayDistance = 0.42f;
        LayerMask layer = 1 << 0;

        // Shoot ray down from player
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, layer);

        // Debug Drawing
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(rayOrigin, rayDirection, color);

        // If it hits a platform
        if (hit.collider != null)
        {
            if (!hit.transform.CompareTag("Player"))
            {
                Debug.Log("Collided with wall");
                directionVector = -directionVector;
                transform.Rotate(Vector3.up * 180f);
            }
        }
    }

    private void Knockback()
    {
        knockedBack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.gameObject.GetComponent<PlayerController>().LoseHealth(1);
        }
        if (collision.transform.CompareTag("Sword") && !knockedBack)
        {
            health--;
            if (health <= 0)
            {
                Instantiate(smokePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
                Knockback();
        }
    }
}
