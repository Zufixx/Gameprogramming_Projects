using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {

    [SerializeField]
    Transform spawnPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Entered entrance");
            GameObject player = GameObject.Find("Link");
            player.GetComponent<PlayerController>().EnterTransition(transform, spawnPos.position);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
