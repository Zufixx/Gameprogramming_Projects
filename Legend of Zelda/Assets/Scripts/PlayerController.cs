﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Knockback))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 0;

    // 0 = static, 1 = down, 2 = left, 3 = up, 4 = right
    [SerializeField]
    private int direction = 1;
    // 0 = none, 1 = walking, 2 = attacking
    [SerializeField]
    private int state = 0;

    public int health = 6;
    public int maxHealth = 6;

    public int rupees = 0;

    [SerializeField]
    private float upFlipTime;
    private float startUpFlipTime;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private Knockback kb;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool startPosSet = false;
    private bool camTransitioning = false;

    [SerializeField]
    [Range(0.1f,10f)]
    private float camTransitionAmount;

    private Transform mainCamera;

    private bool swordGet = false;
    [SerializeField]
    private GameObject sword;

    [SerializeField]
    private Text swordText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text rupeeText;

    private float entranceTimer = 0f;
    private float exitTimer = 0f;
    private Transform entrance;
    private Vector3 caveCam;
    private Vector3 oldCam;
    private bool enterTransition = false;
    private bool exitTransition = false;

    private MusicManager musicManager;
    [SerializeField]
    private AudioClip themeMusic;
    [SerializeField]
    private AudioClip stairSound;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();
        musicManager = GameManager.instance.musicManager;

        mainCamera = Camera.main.transform;

        startUpFlipTime = upFlipTime;

        healthText.text = "Health: " + health.ToString();
        rupeeText.text = "Rupees: " + rupees.ToString();
        swordText.text = "Sword: No";
    }
	
	// Update is called once per frame
	void Update () {
        if (!camTransitioning && !enterTransition && !exitTransition && !kb.knockedBack)
        {
            Movement();

            if(swordGet)
                Attacking();

            MoveAnimation(direction, state);
        }

        CheckTransitions();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (state == 2)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else if (horizontal < -0.1f)
        {
            // Walk Left
            direction = 2;
            state = 1;
            rb.velocity = new Vector2(-speed, 0f);

        }
        else if (horizontal > 0.1f)
        {
            // Walk Right
            direction = 4;
            state = 1;
            rb.velocity = new Vector2(speed, 0f);

        }
        else if (vertical < -0.1f)
        {
            // Walk Down
            direction = 1;
            state = 1;
            rb.velocity = new Vector2(0f, -speed);

        }
        else if (vertical > 0.1f)
        {
            // Walk Up
            direction = 3;
            state = 1;
            rb.velocity = new Vector2(0f, speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            state = 0;
        }
    }

    bool FlipTimer()
    {
        if (upFlipTime > 0f)
        {
            upFlipTime -= Time.deltaTime;
            return false;
        }
        else
        {
            upFlipTime = startUpFlipTime;
            return true;
        }
    }

    private void Attacking()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!sword.activeSelf)
            {
                sword.SetActive(true);
                state = 2;

                Quaternion swordRot = new Quaternion();
                float swordX = 0f;
                float swordY = 0f;

                switch (direction)
                {
                    case 1:
                        // Down
                        swordRot = Quaternion.Euler(0f, 0f, 270f);
                        swordY = -0.7f;
                        sr.flipX = false;
                        break;
                    case 2:
                        // Left
                        swordRot = Quaternion.Euler(0f, 0f, 180f);
                        swordX = -0.7f;
                        sr.flipX = true;
                        break;
                    case 3:
                        // Up
                        swordRot = Quaternion.Euler(0f, 0f, 90f);
                        swordY = 0.7f;
                        sr.flipX = false;
                        break;
                    case 4:
                        // Right
                        swordRot = Quaternion.Euler(0f, 0f, 0f);
                        swordX = 0.7f;
                        sr.flipX = false;
                        break;
                }
                sword.transform.rotation = swordRot;
                sword.transform.position = transform.position + new Vector3(swordX, swordY);

                StartCoroutine(SwordTimer());
            }
        }
        if (state == 2)
        {
            anim.SetInteger("direction", direction);
            anim.SetInteger("state", state);
        }
    }

    private IEnumerator SwordTimer()
    {
        yield return new WaitForSeconds(0.2f);
        sword.SetActive(false);
        state = 0;
        anim.SetInteger("direction", direction);
        anim.SetInteger("state", state);
    }

    private void CheckTransitions()
    {
        if (enterTransition)
        {
            sr.sortingLayerName = "Entering";
            if(musicManager.audioSource.clip != stairSound)
                musicManager.PlayMusic(stairSound, false);
            GetComponent<CircleCollider2D>().enabled = false;
            MoveAnimation(3, 1);
            transform.position = Vector3.Lerp(entrance.position, entrance.position - new Vector3(0f, 1.5f), entranceTimer);
            oldCam = mainCamera.transform.position;

            if (entranceTimer < 1f)
            {
                entranceTimer += Time.deltaTime / 1.5f;
            }
            else
            {
                mainCamera.transform.position = caveCam;
                transform.position = caveCam + new Vector3(0f, -6.5f);
                sr.sortingLayerName = "Absolute Top";
                enterTransition = false;
                GetComponent<CircleCollider2D>().enabled = true;
                entranceTimer = 0f;
                mainCamera.GetComponent<CameraController>().isInCave = true;
                musicManager.SetClip(null);
            }
        }
        if (exitTransition)
        {
            sr.sortingLayerName = "Entering";
            if(musicManager.audioSource.clip != stairSound)
                musicManager.PlayMusic(stairSound, false);
            //Debug.Log("Exiting cave");
            GetComponent<CircleCollider2D>().enabled = false;
            MoveAnimation(1, 1);
            transform.position = Vector3.Lerp(entrance.position - new Vector3(0f, 1.5f), entrance.position - new Vector3(0f, 0.2f), exitTimer);
            mainCamera.transform.position = oldCam;

            if (exitTimer < 1f)
            {
                exitTimer += Time.deltaTime / 1.3f;
            }
            else
            {
                sr.sortingLayerName = "Absolute Top";
                musicManager.PlayMusic(themeMusic, true);
                exitTransition = false;
                GetComponent<CircleCollider2D>().enabled = true;
                entrance.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                exitTimer = 0f;
                mainCamera.GetComponent<CameraController>().isInCave = false;
            }
        }
    }

    void MoveAnimation(int direction, int state)
    {
        anim.SetInteger("direction", direction);
        anim.SetInteger("state", state);

        if (direction == 3 && FlipTimer() && state == 1)
        {
            sr.flipX = !sr.flipX;
        }
        else if (direction == 2)
        {
            sr.flipX = true;
        }
        else if (direction != 3)
        {
            sr.flipX = false;
        }
    }

    public void Pickup(string pickup)
    {
        switch(pickup)
        {
            case "Sword":
                swordGet = true;
                swordText.text = "Sword: Yes";
                break;
        }
    }

    public void LoseHealth(int damage)
    {
        if(health > 1 && !kb.knockedBack)
        {
            health -= damage;
            healthText.text = "Health: " + health.ToString();
        }
        else if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void LoseHealth(int damage, Transform other)
    {
        if (health >= 1 && !kb.knockedBack)
        {
            health -= damage;
            healthText.text = "Health: " + health.ToString();
            kb.KnockbackFromTransform(other);
        }
        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void GetRupee()
    {
        if(rupees < 254)
            rupees++;
        rupeeText.text = "Rupees: " + rupees.ToString();
    }

    public void CamTransitionPlayer(int getDirection, float timer, Vector3 diffPos)
    {
        if (!startPosSet)
        {
            startPos = transform.position;
            startPosSet = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }

        CamTransitionAnim(getDirection);
        camTransitioning = true;

        endPos = startPos + diffPos / camTransitionAmount;
        transform.position = Vector3.Lerp(startPos, endPos, timer);
    }

    private void CamTransitionAnim(int getDirection)
    {
        state = 1;

        direction = getDirection;

        MoveAnimation(direction, 1);
    }

    public void ResetCamTransition()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        startPosSet = false;
        transform.position = endPos;
        camTransitioning = false;
        state = 0;
    }

    public void EnterTransition(Transform getEntrance, Vector3 spawnPos)
    {
        entrance = getEntrance;
        transform.position = entrance.position;
        enterTransition = true;
        caveCam = spawnPos;
        oldCam = mainCamera.transform.position;
    }

    public void ExitTransition()
    {
        transform.position = entrance.position - new Vector3(0f, 1.5f);
        exitTransition = true;
        mainCamera.transform.position = oldCam;
    }
}
