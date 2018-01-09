using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour {

    private SpriteRenderer sr;
    private SpriteSequence ss;

    [Header("Fragile Block")]
    [SerializeField]
    private bool destroyOnHit = false;

    [Header("Spawn Block")]
    [SerializeField]
    private Sprite usedSprite;
    [SerializeField]
    public bool isUsed;
    [SerializeField]
    private bool hidden;

    [Header("Coin Block")]
    [SerializeField]
    private bool coinBlock;
    [SerializeField]
    private int coinAmount;
    [SerializeField]
    private GameObject coinPrefab;

    [Header("Powerup Block")]
    [SerializeField]
    private GameObject spawnPrefab;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ss = GetComponent<SpriteSequence>();

        if(hidden)
            sr.enabled = false;
    }

    public void HitFromBelow()
    {
        if (!isUsed) {
            if (hidden)
                sr.enabled = true;

            if(destroyOnHit)
            {
                Destroy(gameObject);
            }
            else if (coinBlock)
            {
                if (coinAmount >= 1)
                {
                    coinAmount--;
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    if (coinAmount <= 0)
                        Used();
                }
            }
            else
            {
                Instantiate(spawnPrefab, transform.position, Quaternion.identity);
                Used();
            }
        }
    }

    private void Used()
    {
        isUsed = true;
        sr.sprite = usedSprite;
        if (ss != null)
            ss.enabled = false;
    }
}
