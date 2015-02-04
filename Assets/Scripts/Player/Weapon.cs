using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    //Time (in frames) between shots
    public int cooldown = 10;
    public int curCooldown = 0;

    //How much damage each projectile does
    public int damage = 1;

    //Can the player hold down the button to shoot this
    public bool automatic = true;

    //how much random spread/inaccuracy is added per shot
    public double spreadGain = 0;

    //maximum spread of shots
    public double spreadMax = 0;

    //name of the projectile prefab that will be shot
    public string projectileName;

    //speed of the bullet
    public float bulletSpeed = 15;


    private GameObject bulletPrefab;
    private PlayerControl player;

    public Weapon(PlayerControl playerRef, string projectileName)
    {
        bulletPrefab = Resources.Load(projectileName) as GameObject;
        player = playerRef;
    }

    public override void OnEquip()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUnequip()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUse()
    {
        if (curCooldown == 0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab) as GameObject;
            bullet.GetComponent<Bullet>().damage = damage;
            bullet.transform.position = player.transform.position;
            bullet.rigidbody2D.velocity = player.aimVector * bulletSpeed;
            curCooldown = cooldown;
        }
    }

    public override void PassiveEffect()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if (curCooldown > 0) curCooldown--;
    }
}