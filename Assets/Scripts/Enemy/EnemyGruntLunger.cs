using UnityEngine;
using System.Collections;

public class EnemyGruntLunger : EnemyBase
{

    private int walkSpeed = 3; //normal walk speed
    private int lungeSpeed = 10; // speed that he lunges for you at
    private int lungeDist = 4; //how far he needs to be to trigger the lunging start
    private float lungePause = .5f; //how many frames he pauses for before he lunges
    private float lungeCooldown = .5f;


    public float timer = 0; //the actual timer that is used to wait to lunge

    private Vector2 lungeLocation;

    override protected void InitBehavior()
    {
        OnLeaveSpawnerState = "MoveTowardsPlayer";
        base.InitBehavior();
	    AddState("MoveTowardsPlayer", StateMoveTowardsClosestPlayer);
        AddState("LungePause", StateLungePause);
        AddState("Lunge", StateLunge);
        AddState("Cooldown", StateCooldown);
	}


    void StateMoveTowardsClosestPlayer()
    {
        CurrentTarget = LocateNearestPlayer();
        if (CurrentTarget)
        {
            Vector2 dir = CurrentTarget.transform.position - transform.position;
            LookAtObject(CurrentTarget, .25f);
            Vector2 moveVector = dir.normalized;
            rigidbody2D.velocity = moveVector*walkSpeed;

            if (Vector2.Distance(transform.position, CurrentTarget.transform.position) < lungeDist)
            {
                lungeLocation = CurrentTarget.transform.position;
                SetState("LungePause");
                timer = lungePause;
            }
        }
    }

    void StateLungePause()
    {
        //rigidbody2D.velocity = new Vector2();
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SetState("Lunge");
        }
    }

    void StateLunge()
    {
        if (Vector2.Distance(transform.position, lungeLocation) <= .1)
        {
            SetState("Cooldown");
            timer = lungeCooldown;
        }
        else
        {
            Vector2 dir = lungeLocation - (Vector2) transform.position;
            Vector2 moveVector = dir.normalized;
            rigidbody2D.velocity = moveVector*lungeSpeed;
        }
    }

    void StateCooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetState("MoveTowardsPlayer");
        }
    }



}