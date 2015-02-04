using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public int life = 180;
    public int damage;

	void Start(){
	
	}

	void FixedUpdate()
	{
	    life --;
	    if (life <= 0)
	    {
	        Destroy(this.gameObject);
	    }
        
	}

    void OnCollisionEnter2D(Collision2D collision)
    {

        Damageable damagedObject = collision.gameObject.GetComponent<Damageable>();

        if (damagedObject != null)
        {
            damagedObject.Damage(damage);
        }

        Destroy(this.gameObject);
        
    }

}