using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField]
    private Transform cameraObject;

    public void RespawnPlatform(GameObject platform)
    {
        platform.transform.position = new Vector2(Random.Range(-1.8f, 1.8f), cameraObject.position.y + 7);
    }
}
