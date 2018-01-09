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
    private int powerState;
    [SerializeField]
    private GameObject[] spawnPrefabs;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ss = GetComponent<SpriteSequence>();

        if(hidden)
            sr.enabled = false;
    }

    public void HitFromBelow(int playerState)
    {
        if (!isUsed) {
            if (hidden)
                sr.enabled = true;

            if(destroyOnHit && playerState != 0)
            {
                Destroy(gameObject);
            }
            else if (coinBlock && !destroyOnHit)
            {
                if (coinAmount >= 1)
                {
                    coinAmount--;
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    if (coinAmount <= 0)
                        Used();
                }
            }
            else if(!destroyOnHit)
            {
                if(powerState == 1 && playerState == 1 || powerState == 1 && playerState == 2)
                {
                    Instantiate(spawnPrefabs[2], transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(spawnPrefabs[powerState], transform.position, Quaternion.identity);
                }
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
