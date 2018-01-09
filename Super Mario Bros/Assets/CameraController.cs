using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField]
	private Transform player;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float followSpeed = 30f;
    [SerializeField]
    private float offset = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    private void FixedUpdate()
    {
        distance = player.position.x - transform.position.x + offset;
        if (distance > 0f)
        {
            transform.Translate(new Vector2(distance / followSpeed, 0.0f), Space.Self);
        }
    }
}
