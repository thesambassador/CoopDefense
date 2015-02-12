using UnityEngine;
using System.Collections;

public class ControlSet
{
    public string SHOOT1 = "TriggersR";
    public string SHOOT2 = "TriggersL";
    public string MOVE_X = "L_XAxis";
    public string MOVE_Y = "L_YAxis";
    public string AIM_X = "R_XAxis";
    public string AIM_Y = "R_YAxis";
    public string ITEM = "X";
    public string BUILD = "B";
    public string INTERACT = "A";
    public string START = "Start";
    public string BACK = "Back";

    public bool MOUSEAIM = false;

    public static ControlSet DefaultControlSet()
    {
        return new ControlSet();
    }

    public static ControlSet MouseKeyboardControlSet()
    {
        ControlSet result = new ControlSet();

        result.MOVE_X = "KEY_MOVE_X";
        result.MOVE_Y = "KEY_MOVE_Y";
        result.SHOOT1 = "MOUSE_LEFTCLICK";
        result.SHOOT2 = "MOUSE_RIGHTCLICK";
        result.MOUSEAIM = true;
        result.START = "KEY_START";
        result.BACK = "KEY_BACK";

        return result;
    }



}
