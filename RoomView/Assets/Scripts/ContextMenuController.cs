using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuController : MonoBehaviour {

    public bool UI_Active = false;

    private Component[] buttons;            // collection of buttons as Component in UI
    private OMenuClick selectedButton;     // script of the selected button
    private int buttonPressed = 0;          // BUTTONCODE of the selected button to identify

    // Codes to identify the button selected
    public static int UNSELECTED = 100;
    public static int MOVE = 101;
    public static int ROTATE = 102;
    public static int CLONE = 103;
    public static int DELETE = 104;

    public SteamVR_TrackedController controller;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
