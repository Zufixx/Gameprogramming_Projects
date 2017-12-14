using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    private Transform mainCamera;
    [SerializeField]
    private bool active = false;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject smokePrefab;
    private GameObject smoke;

    private GameObject enemy;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float spawnTime;

    private bool enemyIsAlive = false;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.transform;
        CheckProximity();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void CheckProximity()
    {
        float distance = Vector2.Distance(mainCamera.transform.position, transform.position);
        if(distance <= 7.8)
        {
            Debug.Log("Enemy Spawner activated");
            active = true;
            SpawnEnemy();
        }
        else
        {
            Debug.Log("Enemy Spawner deactivated");
            active = false;
            ResetEnemySpawner();
        }
    }

    public void ResetEnemySpawner()
    {
        active = false;
        DestroyEnemy();
    }

    private void SpawnEnemy()
    {
        if (!enemyIsAlive)
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
            enemyIsAlive = true;
        }
    }

    public void DestroyEnemy()
    {
        if (enemy != null)
        {
            Instantiate(smokePrefab, enemy.transform.position, Quaternion.identity);
            Destroy(enemy);
            enemyIsAlive = false;
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    private Transform mainCamera;
    [SerializeField]
    private bool active = false;
    private GameObject spawnedEnemy;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject smokePrefab;
    private GameObject smoke;

    private GameObject enemy;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float spawnTime;
    private float startSpawnTime;

    private bool enemyIsAlive = false;
    private bool countdown = false;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.transform;
        CheckProximity();
        startSpawnTime = spawnTime;
	}
	
	// Update is called once per frame
	void Update () {

        if (countdown && spawnTime >= 0f)
        {
            spawnTime -= Time.deltaTime;
        }
        else if(!enemyIsAlive)
        {
            countdown = false;
            Destroy(smoke);
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            enemyIsAlive = true;
        }
    }

    public void CheckProximity()
    {
        float distance = Vector2.Distance(mainCamera.transform.position, transform.position);
        if(distance <= 7.8)
        {
            Debug.Log("Enemy Spawner activated");
            active = true;
            SpawnEnemy();
        }
        else
        {
            Debug.Log("Enemy Spawner deactivated");
            active = false;
            ResetEnemySpawner();
        }
    }

    private void ResetEnemySpawner()
    {
        countdown = false;
        spawnTime = startSpawnTime;
        if (enemyIsAlive)
            DestroyEnemy();
        if (smoke != null)
            Destroy(smoke);
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawning Enemy");
        enemyIsAlive = true;
        GameObject smoke;
        smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
        countdown = true;
    }

    public void DestroyEnemy()
    {
        Debug.Log("DestroyEnemy()");
        if (enemyIsAlive && enemy != null)
        {
            Debug.Log("Destroy with smoke");
            GameObject smoke;
            smoke = Instantiate(smokePrefab, enemy.transform.position, Quaternion.identity);
            Destroy(smoke, spawnTime);
            Destroy(enemy);
            enemyIsAlive = false;
        }
        else if(enemy == null)
        {
            Debug.Log("enemy == null");
            enemyIsAlive = false;
        }
        else
        {
            enemyIsAlive = false;
        }
    }
}
*/
