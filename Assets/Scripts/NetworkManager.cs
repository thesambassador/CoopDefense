using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    private const string typeName = "SamShootyShoot";
    private const string gameName = "SamShootyShootRoom";

    private HostData[] hostList;

    public GameObject playerPrefab;
    public PlayerManager playerManager;

    void Start()
    {
        GameObject playerManagerObject = GameObject.FindGameObjectWithTag("PlayerManager");
        playerManager = playerManagerObject.GetComponent<PlayerManager>();
    }

    void Update()
    {

    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button((new Rect(10, 10, 250, 100)), "Start Server"))
            {
                StartServer();
            }
            if (GUI.Button(new Rect(10, 120, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(10, 240 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }

        }
    }

    //Server-side
    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized!");
        SpawnPlayer();
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        int playerNum = playerManager.GetNextPlayerNumber();

        if (playerNum != -1)
        {
            playerManager.SetPlayerActive(playerNum);
            networkView.RPC("SpawnClientPlayer", player, playerNum);
            Debug.Log("Player connected, given player number " + playerNum);
        }
        else
        {
            Network.CloseConnection(player, true);
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (gameObject.networkView.owner == player)
            {
                PlayerControl playerControl = gameObject.GetComponent<PlayerControl>();
                playerManager.SetPlayerActive(playerControl.PlayerNum, false);
            }
        }
        
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    
    //Client-side
    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        //SpawnPlayer();
    }

    void OnDisconnectedFromServer(NetworkDisconnection dc)
    {
        Application.LoadLevel(0);
    }

    

    [RPC]
    void SpawnClientPlayer(int playerNum)
    {
        Debug.Log("Spawning client with playerNum " + playerNum);
        playerManager.ActivatePlayer(1, playerNum);
    }

    void SpawnPlayer()
    {
        int playerNum = playerManager.GetNextPlayerNumber();
        playerManager.ActivatePlayer(1, playerNum);
        playerManager.SetPlayerActive(playerNum);
    }

	

}