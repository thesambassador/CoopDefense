using UnityEngine;
using System.Collections;

public class InputHandler {

	public static float GetPlayerAxis(string axisName, int playerNum){
        return Input.GetAxis(axisName + "_" + playerNum.ToString());
    }

    public static bool GetPlayerButton(string buttonName, int playerNum)
    {
        return Input.GetButton(buttonName + "_" + playerNum.ToString());
    }

    
}
