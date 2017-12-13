using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTimer : MonoBehaviour {

    private SpriteRenderer sr;
    [SerializeField]
    private float flipTime = 1f;
    private float startFlipTime;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        startFlipTime = flipTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (flipTimer())
            sr.flipX = !sr.flipX;
    }

    public bool flipTimer()
    {
        if (flipTime > 0f)
        {
            flipTime -= Time.deltaTime;
            return false;
        }
        else
        {
            flipTime = startFlipTime;
            return true;
        }
    }
}
