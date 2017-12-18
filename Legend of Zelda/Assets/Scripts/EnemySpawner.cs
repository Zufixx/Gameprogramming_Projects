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

    private bool enemyIsAlive = false;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.transform;
        CheckProximity();
        GetComponent<SpriteRenderer>().enabled = false;
	}

    public void CheckProximity()
    {
        float distanceX = Mathf.Abs(transform.position.x - mainCamera.transform.position.x);
        float distanceY = Mathf.Abs(transform.position.y - (mainCamera.transform.position.y - 1.5f));

        if (distanceX <= 8f && distanceY <= 5.5f)
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