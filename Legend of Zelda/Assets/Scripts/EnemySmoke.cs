using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmoke : MonoBehaviour {

    [SerializeField]
    private float lifeTime;

    private GameObject enemyPrefab;

    public void Initialize(GameObject enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
	}
}
