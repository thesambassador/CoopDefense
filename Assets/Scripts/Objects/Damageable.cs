using System;
using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{

    public int hp;


    public Color _startColor = Color.white;
    private SpriteRenderer _spriteRenderer;


	void Start()
	{
	    _spriteRenderer = this.GetComponent<SpriteRenderer>();
	    _spriteRenderer.material.color = _startColor;


	}

	void Update()
	{

        if (_spriteRenderer.material.color != _startColor)
	    {
	        renderer.material.color = Color.Lerp(renderer.material.color, _startColor, .1f);
	    }
        

	    if (hp <= 0)
	    {
            Destroy(this.gameObject);
	    }
	}

    public void Damage(int damage)
    {
        hp -= damage;

        _spriteRenderer.material.color = Color.red;
        Debug.Log(_spriteRenderer.material.color);

    }

}