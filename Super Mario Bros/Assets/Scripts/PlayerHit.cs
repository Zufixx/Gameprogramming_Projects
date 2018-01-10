using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D[] cols = new CapsuleCollider2D[2];
    private PlayerController pc;

    [Header("Cooldown")]
    [SerializeField]
    private float cooldownTime = 2f;
    private float startCooldownTime;
    [SerializeField]
    private bool cooldown = false;

    [Header("Death Animation")]
    [SerializeField]
    private float floatUpDelay;
    [SerializeField]
    private float maxHeight;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float floatUpTimer;
    [SerializeField]
    private Sprite dyingSprite;
    private int dieTime = 2;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cols = GetComponents<CapsuleCollider2D>();
        pc = GetComponent<PlayerController>();

        startCooldownTime = cooldownTime;
    }

    public void Cooldown()
    {
        if (cooldown)
        {
            sr.enabled = !sr.enabled;
            cooldownTime -= Time.deltaTime;

            if (cooldownTime <= 0)
            {
                cooldown = false;
                cooldownTime = startCooldownTime;
                sr.enabled = true;
            }
        }
    }

    public void Hit()
    {
        if (pc.state == 0 && !cooldown)
            Die();
        else if (!cooldown)
        {
            pc.ChangeState(0);
            cooldown = true;
        }
    }

    public void Die()
    {
        pc.isAlive = false;

        startPosition = transform.position;
        endPosition = startPosition + new Vector3(0f, maxHeight);
    }

    public void DeathAnimation()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        foreach (CapsuleCollider2D col in cols)
            col.enabled = false;

        sr.sprite = dyingSprite;

        if (dieTime != 2)
            transform.position = Vector3.Lerp(startPosition, endPosition, floatUpTimer);

        if (floatUpTimer < 1f)
            floatUpTimer += Time.deltaTime / floatUpDelay;
        else if (dieTime == 2)
        {
            floatUpTimer = 0f;
            dieTime--;
        }
        else if (dieTime == 1)
        {
            startPosition = endPosition;
            endPosition = new Vector3(transform.position.x, -5f);
            floatUpTimer = 0f;
            dieTime--;
        }
        else
            GameManager.instance.GameOver();
    }
}
