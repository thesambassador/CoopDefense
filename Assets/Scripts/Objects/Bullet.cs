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
	    if (life <= 0 && networkView.isMine)
	    {
            Network.RemoveRPCs(this.networkView.viewID);
	        Network.Destroy(this.gameObject);
	    }
        
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo messageInfo)
    {
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncVelocity = rigidbody2D.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncVelocity);

            rigidbody2D.velocity = syncVelocity;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (networkView.isMine)
        {
            Damageable damagedObject = collision.gameObject.GetComponent<Damageable>();

            if (damagedObject != null)
            {
                damagedObject.networkView.RPC("Damage", RPCMode.All, damage);
            }

            Network.RemoveRPCs(this.networkView.viewID);
            Network.Destroy(this.gameObject);
           
        }

    }

}