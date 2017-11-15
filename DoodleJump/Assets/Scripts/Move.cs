using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    [SerializeField]
    private Vector2 moveSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(moveSpeed * Time.deltaTime, Space.Self);
	}
}
