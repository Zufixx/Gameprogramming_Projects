using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField]
    private Transform cameraObject;

    [SerializeField]
    private GameObject platform;

    private float platformY;
    private int prevRng;

    private void Start()
    {
        platformY = 0.0f;
        while (platformY < cameraObject.position.y + 6.0f)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        if (platformY < cameraObject.position.y + 6.0f)
            SpawnPlatform();
    }

    public void SpawnPlatform()
    {
        int rng = Random.Range(0, 8);
        if (rng == 1 && prevRng == 1)
            rng = 0;
        else if (rng == prevRng)
            rng = Random.Range(0, 8);



        GameObject currentPlatform;
        currentPlatform = Instantiate(platform, new Vector2(Random.Range(-1.8f, 1.8f), platformY), Quaternion.identity);
        Platform platformScript = currentPlatform.GetComponent<Platform>();
        platformScript.Initialize(rng);

        if (rng == 4)
            platformY += 0.5f;
        else if (rng == 7)
            platformY += 2.0f;
        else
            platformY += 1.0f;

        prevRng = rng;

    }

    public void RespawnPlatform(GameObject platform)
    {
        platform.transform.position = new Vector2(Random.Range(-1.8f, 1.8f), cameraObject.position.y + 7);
    }
}
