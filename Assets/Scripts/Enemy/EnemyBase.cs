using UnityEngine;
using System.Collections;

public class EnemyBase : StateEntity
{

    public GameObject CurrentTarget;

    public int BasicSpeed = 5;

    public string DefaultState = "MoveTowardsTarget";

    public string OnLeaveSpawnerState;

    protected override void InitBehavior()
    {
        base.InitBehavior();
        AddState("MoveTowardsTarget", StateMoveTowardsTarget);
        SetState(DefaultState);
    }


    protected GameObject LocateNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float minDist = 50000;
        GameObject result = null;

        foreach (GameObject player in players)
        {
            float testDist = Vector2.Distance(player.transform.position, this.transform.position);

            if (testDist < minDist)
            {
                result = player;
                minDist = testDist;
            }
        }
        return result;

    }

    protected void LookAtObject(GameObject lookat, float t = 1)
    {
        var dir = lookat.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), t); 
    }

    protected void LookAtPosition(Vector2 lookat, float t = 1)
    {
        var dir = lookat - (Vector2)transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), t); 
    }

    protected void MoveTowardsLocation(Vector2 target, float speed)
    {
        Vector2 dir = target - (Vector2)transform.position;

        Vector2 moveVector = dir.normalized;
        rigidbody2D.velocity = moveVector * speed;

    }

    protected void StateMoveTowardsTarget()
    {
        LookAtObject(CurrentTarget);
        MoveTowardsLocation(CurrentTarget.transform.position, BasicSpeed);

        if (Vector2.Distance(transform.position, CurrentTarget.transform.position) < .1)
        {
            SetState(OnLeaveSpawnerState);
        }
    }

}