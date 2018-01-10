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
    private float offset;

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        distance = player.position.x - transform.position.x + offset;
        if (distance > 0.2f)
            transform.Translate(new Vector2(distance / followSpeed, 0.0f), Space.Self);
    }
}
