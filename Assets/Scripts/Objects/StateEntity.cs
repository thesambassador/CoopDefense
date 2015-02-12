using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateEntity : MonoBehaviour
{

    public string CurrentState;

    public delegate void StateUpdate();

    public StateUpdate UpdateState;

    public Dictionary<string, StateUpdate> StateFunctions;

    //network stuff
    private float lastSyncTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private Quaternion syncStartRotation;
    private Quaternion syncEndRotation;


    void Start()
    {
        InitBehavior();
    }

    protected virtual void InitBehavior()
    {
        
    }
 

    void Awake()
	{
        StateFunctions = new Dictionary<string, StateUpdate>();
	}

	void Update()
	{
	    if (Network.isServer)
	        UpdateState();
	    else
	    {
	        UpdateNetworkPosition();
	    }
	}

    public void SetState(string stateName)
    {
        if (StateFunctions.ContainsKey(stateName))
        {
            UpdateState = StateFunctions[stateName];
            CurrentState = stateName;
        }
        else
        {
            Debug.Log("State " + stateName + " does not exist");
        }
    }

    public void AddState(string stateName, StateUpdate updateFunction)
    {
        StateFunctions.Add(stateName, updateFunction);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo messageInfo)
    {
        Vector3 syncPosition = Vector3.zero;
        Quaternion aimRotation = this.transform.rotation;
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);

            aimRotation = transform.rotation;
            stream.Serialize(ref aimRotation);

        }
        else
        {
            stream.Serialize(ref syncPosition);
            syncTime = 0f;
            syncDelay = Time.time - lastSyncTime;
            lastSyncTime = Time.time;

            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;

            stream.Serialize(ref aimRotation);
            syncStartRotation = transform.rotation;
            syncEndRotation = aimRotation;

        }
    }

    void UpdateNetworkPosition()
    {
        if (syncDelay != 0)
        {
            syncTime += Time.deltaTime;
            transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime/syncDelay);

            transform.rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncTime/syncDelay);
        }

    }

}