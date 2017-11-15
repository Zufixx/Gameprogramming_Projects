using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Boundary")
            Destroy(gameObject);
    }
}
