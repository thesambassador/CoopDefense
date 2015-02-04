using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    private List<PlayerSpawn> _playerSpawns;

    private bool[] _activePlayers;
    private Color[] _playerColors;

    private ControlSet _controlSet;

    public GameObject PlayerPrefab;

	void Start()
	{
	    _activePlayers = new bool[4];
	    _playerColors = new Color[4];

	    _playerColors[0] = Color.blue;
        _playerColors[1] = Color.red;
        _playerColors[2] = Color.green;
        _playerColors[3] = Color.yellow;

	    _playerSpawns = new List<PlayerSpawn>(GetComponentsInChildren<PlayerSpawn>());

        _controlSet = ControlSet.DefaultControlSet();
	}

	void Update(){

	    for (int i = 1; i <= 4; i++)
	    {
            if (InputHandler.GetPlayerButton(_controlSet.START, i))
            {
                ActivatePlayer(i);
            }
	    }


	}

    void ActivatePlayer(int playerNum)
    {

        if (_activePlayers[playerNum]) //already active
        {

        }
        else
        {
            _activePlayers[playerNum] = true;

            GameObject newPlayer = Instantiate(PlayerPrefab) as GameObject;
            Vector3 spawnLocation = _playerSpawns[playerNum].transform.position;
            newPlayer.transform.position = spawnLocation;

            PlayerControl playerControl = newPlayer.GetComponent<PlayerControl>();
            playerControl.PlayerNum = playerNum;
            playerControl.color = _playerColors[playerNum];



        }

    }

}