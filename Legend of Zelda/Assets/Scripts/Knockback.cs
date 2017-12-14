using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour {

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public bool knockedBack = false;
    [SerializeField]
    private bool constrainToGrid = true;
    [SerializeField]
    private float knockedBackTimer = 0.5f;
    [SerializeField]
    private float knockbackAmount = 1f;
    [SerializeField]
    private Color knockbackColor = new Color(0.5f, 0.5f, 0.5f);

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        if (knockedBack)
        {
            sr.color = knockbackColor;
            if (knockedBackTimer <= 0f)
            {
                knockedBack = false;
                sr.color = new Color(1f, 1f, 1f);
                knockedBackTimer = 0.5f;
            }
            else
            {
                knockedBackTimer -= Time.deltaTime;
            }
        }
    }

    public void KnockbackFromTransform(Transform other)
    {
        Vector2 diff = other.position - transform.position;
        diff.Normalize();
        if (constrainToGrid)
        {
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                diff = new Vector2(diff.x, 0f);
            else
                diff = new Vector2(0f, diff.y);
        }

        rb.velocity = -diff * knockbackAmount;
        knockedBack = true;
    }
}
