using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Pickup : MonoBehaviour
{
    [SerializeField]
    private string pickupName = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log(pickupName + " picked up.");
            collision.GetComponent<PlayerController>().Pickup(pickupName);
            Destroy(gameObject);
        }
    }
}
