using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSequence : MonoBehaviour {

    [SerializeField]
    private float interval = 1f;
    private float startInterval = 0f;
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer sr;
    private int index = 0;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        SpriteTimer();
	}

    private void Initialize()
    {
        startInterval = interval;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[index];
    }

    private void SpriteTimer()
    {
        interval -= Time.deltaTime;
        if (interval <= 0f)
        {
            if (index < sprites.Length - 1)
                index++;
            else
                index = 0;

            sr.sprite = sprites[index];
            interval = startInterval;
        }
    }
}
