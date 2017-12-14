using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoundary : MonoBehaviour
{
    [SerializeField]
    private int direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            transform.parent.gameObject.GetComponent<CameraController>().SetPos(direction);
    }
}
