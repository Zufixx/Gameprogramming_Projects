using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    private Vector2 startPosition;
    [SerializeField]
    private float bounceSpeed = 5f;
    [SerializeField]
    private float bounceAmount = 10f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + new Vector2(0.0f, Mathf.Sin(Time.time * bounceSpeed) / bounceAmount);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Boundary")
            Destroy(gameObject);
    }
}
