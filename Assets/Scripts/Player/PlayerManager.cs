using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    private List<PlayerSpawn> _playerSpawns;

    private bool[] _activePlayers; //players that are currently spawned
    private bool[] _activeControllers; //local controllers that are currently in use
    private Color[] _playerColors;

    private ControlSet _controlSet;

    public GameObject PlayerPrefab;

	void Start()
	{
	    _activePlayers = new bool[4];
        _activeControllers = new bool[4];
	    _playerColors = new Color[4];

	    _playerColors[0] = Color.blue;
        _playerColors[1] = Color.red;
        _playerColors[2] = Color.green;
        _playerColors[3] = Color.yellow;

	    _playerSpawns = new List<PlayerSpawn>(GetComponentsInChildren<PlayerSpawn>());
        _playerSpawns.Sort( (spawn1, spawn2) => (int)spawn1.transform.position.x - (int)spawn2.transform.position.x);

        _controlSet = ControlSet.DefaultControlSet();
	}

	void Update(){

	    for (int i = 1; i <= 4; i++)
	    {
            if (InputHandler.GetPlayerButton(_controlSet.START, i))
            {
                ActivatePlayer(i, GetNextPlayerNumber(), false);
            }
	    }


	}

    public void ActivatePlayer(int controllerNum, int playerNum, bool isMouseKeyboard = true)
    {
        
        if (_activeControllers[controllerNum] || IsPlayerActive(playerNum)) //already active
        {
           
        }
        else
        {
            _activeControllers[controllerNum] = true;

            Vector3 spawnLocation = _playerSpawns[playerNum-1].transform.position;

            GameObject newPlayer = Network.Instantiate(PlayerPrefab, spawnLocation, this.transform.rotation, 0) as GameObject;

            Vector3 color = new Vector3();
            color.x = _playerColors[playerNum - 1].r;
            color.y = _playerColors[playerNum - 1].g;
            color.z = _playerColors[playerNum - 1].b;

            newPlayer.networkView.RPC("InitializePlayer", RPCMode.AllBuffered, playerNum, controllerNum, color);
            

        }
        Debug.Log("Player " + playerNum.ToString() + " spawned");

    }

    public int GetNextPlayerNumber()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (!IsPlayerActive(i))
            {
                return i;
            }
        }
        return -1;
    }

    public bool IsPlayerActive(int playerNum)
    {
        return _activePlayers[playerNum - 1];
    }

    public void SetPlayerActive(int playerNum, bool val=true)
    {
        Debug.Log("Setting player " + playerNum.ToString());
        _activePlayers[playerNum - 1] = val;
    }

}