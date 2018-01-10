using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFlag : MonoBehaviour {

    public bool goUp = false;

    private Vector3 startPosition;
    [SerializeField]
    private float endHeightDiff;
    [SerializeField]
    private float floatUpDelay = 1f;
    private float floatUpTimer = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update ()
    {
        if (goUp)
            VerticalLerp();
    }

    private void VerticalLerp()
    {
        transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0f, endHeightDiff), floatUpTimer);

        if (floatUpTimer < 1f)
            floatUpTimer += Time.deltaTime / floatUpDelay;
    }
}
