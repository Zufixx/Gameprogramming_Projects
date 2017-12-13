using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // 0 = static, 1 = down, 2 = left, 3 = up, 4 = right
    public int direction;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 diffPos;

    private float timer = 0f;
    [SerializeField]
    private float moveTime;

    private bool startPosSet = false;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;

    public bool isInCave = false;
    [SerializeField]
    private Transform caveCamPos;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(direction == 1 && isInCave)
        {
            return;
        }
        else if(direction != 0)
        {
            TransitionCamera();
            playerController.CamTransitionPlayer(direction, timer, diffPos);
            timer += Time.deltaTime / moveTime;

            if (timer >= 1f)
                ResetTransition();
        }
    }

    private void TransitionCamera()
    {
        if (!startPosSet)
        {
            startPos = transform.position;
            startPosSet = true;
        }

        endPos = startPos + diffPos;
        transform.position = Vector3.Lerp(startPos, endPos, timer);
    }

    private void ResetTransition()
    {
        Debug.Log("Reset");
        transform.position = endPos;
        startPosSet = false;
        direction = 0;
        timer = 0f;
        playerController.ResetCamTransition();
    }

    public void SetPos(int getDirection)
    {
        float diffX = 0f;
        float diffY = 0f;
        switch(getDirection)
        {
            case 1:
                diffY = -offsetY;
                break;
            case 2:
                diffX = -offsetX;
                break;
            case 3:
                diffY = offsetY;
                break;
            case 4:
                diffX = offsetX;
                break;
        }
        diffPos = new Vector3(diffX, diffY);
        direction = getDirection;
    }
}
