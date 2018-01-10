using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTimer : MonoBehaviour {

    private SpriteRenderer sr;
    [SerializeField]
    private float flipTime = 1f;
    private float startFlipTime;

	private void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        startFlipTime = flipTime;
	}
	
	private void Update ()
    {
        if (FlipTimerBool())
            sr.flipX = !sr.flipX;
    }

    private bool FlipTimerBool()
    {
        if (flipTime >= 0f)
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
