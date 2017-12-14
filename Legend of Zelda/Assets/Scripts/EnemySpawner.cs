using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    private Transform mainCamera;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject smokePrefab;

    private GameObject enemy;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float spawnTime;

    private bool enemyIsAlive = false;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.transform;
        CheckProximity();
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void CheckProximity()
    {
        float distance = Vector2.Distance(mainCamera.transform.position, transform.position);
        if(distance <= 7.8)
        {
            SpawnEnemy();
        }
        else
        {
            ResetEnemySpawner();
        }
    }

    public void ResetEnemySpawner()
    {
        DestroyEnemy();
    }

    private void SpawnEnemy()
    {
        if (!enemyIsAlive)
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
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