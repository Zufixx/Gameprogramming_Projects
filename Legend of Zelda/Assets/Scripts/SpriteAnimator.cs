using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {

    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    float timer;
    float startTimer;
    int index = 0;

    private SpriteRenderer sr;

	void Start () {
        startTimer = timer;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[index];
	}
	
	void Update () {
		if(timer <= 0f)
        {
            index++;
            if (index >= sprites.Length)
                Destroy(gameObject);
            else
                sr.sprite = sprites[index];
            timer = startTimer;
        }
        else
            timer -= Time.deltaTime;
	}
}
