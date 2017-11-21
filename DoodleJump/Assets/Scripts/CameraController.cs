﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private PlayerController playerController;

    private float distance;
    private float trueDistance;

    private void FixedUpdate()
    {
        #region Distance to Max Height
        distance = playerController.GetMaxHeight() - transform.position.y + 1.0f;

        if (distance > 0)
        {
            transform.Translate(new Vector3(0.0f, distance / 10f, 0.0f), Space.Self);
        }
        #endregion

        #region Game Over if fall off
        trueDistance = playerController.transform.position.y - transform.position.y;
        if (trueDistance < -5)
            GameManager.instance.GameOver();
        #endregion
    }

    public float GetDistance()
    {
        return distance;
    }
}
