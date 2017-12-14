using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Rupee : MonoBehaviour {

    private bool collectible = false;

    private void Start()
    {
        StartCoroutine(RupeeTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Sword") && collectible)
        {
            collision.transform.parent.gameObject.GetComponentInParent<PlayerController>().GetRupee();
            Destroy(gameObject);
        }
    }

    IEnumerator RupeeTimer()
    {
        yield return new WaitForSeconds(0.5f);
        collectible = true;
    }
}
