using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{


	/************Properties*************/

	//Time to wait before next spawn
	[SerializeField] 
	private float spawnTime = 1;
	
	[SerializeField]
	private GameObject enemyPrefab = null;

	[SerializeField]
	private int enemiesPerSpawn = 1;

	//target that we want instantiated enemy to follow
	[SerializeField]
	private GameObject target = null;

	public float SpawnRate{get; set;}

	public GameObject EnemyPrefab{get; set;}

	public int EnemiesPerSpawn{get; set;}


    // Start is called before the first frame update
    void Start()
    {
    	if (target == null)
            target = GameObject.FindGameObjectWithTag("Player");
        
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*Spawn numEnemies enemies around the spawn point and set their targets.*/
    IEnumerator SpawnEnemy(){
    	while (true){
    		yield return new WaitForSeconds(spawnTime);

	        for (int i = 0; i < enemiesPerSpawn; i++)
	        {
	            // Create a new enemy 
	            Debug.Log("here");
	            GameObject newEnemy = Instantiate(enemyPrefab, (Vector2)transform.position, Quaternion.identity);
	            // Set enemy target
	            Debug.Log(newEnemy);

	            newEnemy.GetComponent<Enemy>().Target = target;
	        }
    	}
    }
}
