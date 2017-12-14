using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Rupee : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Sword"))
        {
            collision.transform.parent.gameObject.GetComponentInParent<PlayerController>().GetRupee();
            Destroy(gameObject);
        }
    }
}
