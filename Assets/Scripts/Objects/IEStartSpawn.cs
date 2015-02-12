using UnityEngine;
using System.Collections;

public class IEStartSpawn : InteractableObject
{

    public GameObject Director;

	void Start(){
	
	}

	void Update(){
	
	}

    public override void Activate()
    {
        Director.SendMessage("Activate");
    }
}