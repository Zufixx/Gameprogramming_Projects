using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

    [SerializeField]
    private float floatUpDelay = 1f;
    [SerializeField]
    private float maxHeight = 4f;

    private float floatUpTimer = 0f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        endPosition = startPosition + new Vector3(0f, maxHeight);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition, floatUpTimer);

        if (floatUpTimer < 1f)
        {
            floatUpTimer += Time.deltaTime / floatUpDelay;
        }
        else if(endPosition == startPosition + new Vector3(0f, maxHeight))
        {
            Vector3 temp = startPosition;
            startPosition = endPosition;
            endPosition = temp + new Vector3(0f, 1f);
            floatUpTimer = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
