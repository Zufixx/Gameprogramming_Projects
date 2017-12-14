using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().LoseHealth(1);
        }
        else if(collision.transform.CompareTag("Sword"))
        {

        }
    }
}
