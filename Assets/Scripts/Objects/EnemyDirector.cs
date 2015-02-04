using UnityEngine;
using System.Collections.Generic;

public class EnemyDirector : MonoBehaviour
{

    public float spawnTime = 1;
    public bool active = false;

    List<SpawnDoor> spawnLocations;

    public GameObject enemyPrefab;


	void Start()
	{
	    spawnLocations = new List<SpawnDoor>(GetComponentsInChildren<SpawnDoor>());
	}

	void Update()
	{
	    if (active)
	    {
	        spawnTime -= Time.deltaTime;

	        if (spawnTime <= 0)
	        {
	            int spawnDoorIndex = Random.Range(0, spawnLocations.Count);
	            Debug.Log(spawnDoorIndex);
	            SpawnDoor door = spawnLocations[spawnDoorIndex];

	            door.SpawnEnemy(enemyPrefab);
	            spawnTime = 2;
	        }
	    }
	    else
	    {
	        if (InputHandler.GetPlayerButton(ControlSet.DefaultControlSet().BACK, 1))
	        {
	            active = true;
	        }
	    }

	}

}