using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float followingSpeed = 10f;

    private float distance;
    private float trueDistance;

    private void FixedUpdate()
    {
        distance = playerController.GetMaxHeight() - transform.position.y + 1.0f;

        if (distance > 0)
        {
            transform.Translate(new Vector2(distance / followingSpeed, 0.0f), Space.Self);
        }

        trueDistance = playerController.transform.position.y - transform.position.y;
        if (trueDistance < -5)
            GameManager.instance.GameOver();
    }

    public float GetDistance() { return distance; }
}
