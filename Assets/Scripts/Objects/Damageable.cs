using System;
using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{

    public int hp;

    public Color _startColor = Color.white;
    public SpriteRenderer SpriteRenderer;

    public float DamageCooldown = 5;

    private float _damageCooldownTimer;


	void Start()
	{

        SpriteRenderer.material.color = _startColor;
	}

	void Update()
	{
	    if (_damageCooldownTimer > 0)
	    {
	        _damageCooldownTimer -= Time.deltaTime;
	    }

        if (SpriteRenderer.material.color != _startColor)
	    {
            SpriteRenderer.material.color = Color.Lerp(SpriteRenderer.material.color, _startColor, .1f);
	        SpriteRenderer.color = SpriteRenderer.material.color;
	    }

	    if (hp <= 0)
	    {
	        if (Network.isServer)
	        {
                Network.RemoveRPCs(networkView.viewID);
                Network.Destroy(this.gameObject);
	        }
	        else
	        {
	            this.collider2D.enabled = false;
	            this.renderer.enabled = false;
	        }
	    }
	}

    [RPC]
    public void Damage(int damage)
    {
        if (_damageCooldownTimer <= 0)
        {
            hp -= damage;

            SpriteRenderer.material.color = Color.red;
            Debug.Log(SpriteRenderer.material.color);
            _damageCooldownTimer = DamageCooldown;
        }

    }

}