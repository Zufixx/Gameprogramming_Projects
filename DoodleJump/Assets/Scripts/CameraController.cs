using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameManager gameManager;

    private float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {
        distance = playerController.GetMaxHeight() - transform.position.y + 1.0f;

        if (distance > 0)
        {
            transform.Translate(new Vector3(0.0f, distance / 30f, 0.0f), Space.Self);
        }

        if (distance < -5)
        {
            Debug.Log("Game Over");
            gameManager.GameOver();
        }
    }

    public float GetDistance()
    {
        return distance;
    }
}
