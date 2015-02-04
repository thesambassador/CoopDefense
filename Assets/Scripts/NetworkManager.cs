using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    private const string typeName = "SamShootyShoot";
    private const string gameName = "SamShootyShootRoom";

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized!");
    }

	void Start(){
	
	}

	void Update(){
	
	}

}