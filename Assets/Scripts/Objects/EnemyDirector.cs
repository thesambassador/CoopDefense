using UnityEngine;
using System.Collections.Generic;

public class EnemyDirector : MonoBehaviour
{

    public float spawnTime = 1;
    public float spawnTimer = 0f;
    public bool activated = false;

    List<SpawnDoor> spawnLocations;

    public GameObject enemyPrefab;


	void Start()
	{
	    spawnLocations = new List<SpawnDoor>(GetComponentsInChildren<SpawnDoor>());
	}

	void Update()
	{
	    if (Network.isServer)
	    {
            if (activated)
            {
                spawnTimer -= Time.deltaTime;

                if (spawnTimer <= 0)
                {
                    int spawnDoorIndex = Random.Range(0, spawnLocations.Count);
                    Debug.Log(spawnDoorIndex);
                    SpawnDoor door = spawnLocations[spawnDoorIndex];

                    SpawnEnemy(door, enemyPrefab);
                    spawnTimer = spawnTime;

                    if(spawnTime > 1)
                        spawnTime -= .1f;
                }

            }

	        if (Input.GetKeyDown(KeyCode.K))
	        {
	            this.activated = true;
	        }

	    }
	}

    void SpawnEnemyRandom()
    {
        int spawnDoorIndex = Random.Range(0, spawnLocations.Count);
        SpawnDoor door = spawnLocations[spawnDoorIndex];
        SpawnEnemy(door, enemyPrefab);
    }

    void SpawnEnemy(SpawnDoor door, GameObject enemyPrefab)
    {

        Vector3 spawnPosition = door.transform.position;
        spawnPosition.y += door.SpawnOffset.y;
        spawnPosition.x += door.SpawnOffset.x;
        spawnPosition.z -= 1;

        GameObject result = Network.Instantiate(enemyPrefab, spawnPosition, this.transform.rotation, 0) as GameObject;

        //Now set the enemy's state to leave the spawn, and set their target location
        EnemyBase enemyBehavior = result.GetComponent<EnemyBase>();
        enemyBehavior.CurrentTarget = door.SpawnLeave;
    
    }

    void Activate()
    {
        activated = true;
    }

}