using UnityEngine;
using System.Collections;

public class SpawnDoor : MonoBehaviour
{
    private bool _open = true;

    public Sprite OpenSprite;
    public Sprite ClosedSprite;

    public GameObject SpawnLeave;

    public bool Open
    {
        get { return _open; }
        set
        {
            _open = value;

            SpriteRenderer render = this.renderer as SpriteRenderer;

            if (_open)
            {
                render.sprite = OpenSprite;  
            }
            else
            {
                render.sprite = ClosedSprite;
            }

        }
    }

    public Vector2 SpawnOffset;

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject result = Instantiate(enemyPrefab) as GameObject;

        Vector3 spawnPosition = this.transform.position;
        spawnPosition.y += SpawnOffset.y;
        spawnPosition.x += SpawnOffset.x;
        spawnPosition.z = result.transform.position.z;

        result.transform.position = spawnPosition;

        //Now set the enemy's state to leave the spawn, and set their target location

        

        EnemyBase enemyBehavior = result.GetComponent<EnemyBase>();
        enemyBehavior.CurrentTarget = SpawnLeave;


        return result;
    }

    void Start()
    {
        Open = true;
    }

	void Update(){
	    
	}

}