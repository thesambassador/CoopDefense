using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateEntity : MonoBehaviour
{

    public string CurrentState;

    public delegate void StateUpdate();

    public StateUpdate UpdateState;

    public Dictionary<string, StateUpdate> StateFunctions;

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
        UpdateState();
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

}