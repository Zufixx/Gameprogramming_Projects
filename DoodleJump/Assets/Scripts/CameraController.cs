using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float distance = player.transform.position.y - transform.position.y;

        if (distance > 0)
        {
            transform.Translate(new Vector3(0.0f, distance / 60f, 0.0f), Space.Self);
        }

        if (distance < -5)
        {
            Debug.Log("Game Over");
        }
	}
}
