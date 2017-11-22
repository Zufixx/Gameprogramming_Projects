using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float speed;
	
	void Update () {
        float moveY = transform.position.y + -Time.deltaTime * speed;
        transform.position = new Vector2(transform.position.x, moveY);
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Boundary")
            Destroy(gameObject);
    }
}
