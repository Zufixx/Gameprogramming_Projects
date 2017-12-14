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

	// Use this for initialization
	void Start () {
        startTimer = timer;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[index];
	}
	
	// Update is called once per frame
	void Update () {
		if(timer <= 0f)
        {
            index++;
            Debug.Log("Sprite index: " + index.ToString());
            if (index >= sprites.Length)
            {
                Debug.Log("Destroy smoke");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Switch sprite");
                sr.sprite = sprites[index];
            }
            timer = startTimer;
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
