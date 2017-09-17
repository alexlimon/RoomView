/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Teleport Controller
 * \bug No known bugs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour {

    private SteamVR_TrackedObject controller; //!< pointer to the controller
    private SteamVR_TrackedController controllerInput; //!< pointer to the controller input buttons
    private SteamVR_Teleporter teleporter; //!< pointer to the teleporter
    private SteamVR_LaserPointer pointer; //!< pointer to the laser pointer

  /*!
	 * \brief initializes variables
	 * \details This function initializes the pointers to the scene such as controllers, laser pointer, teleporter
	 */
	void Awake () {
        controller = GetComponent<SteamVR_TrackedObject>();
        controllerInput = GetComponent<SteamVR_TrackedController>();
        teleporter = GetComponent<SteamVR_Teleporter>();
        pointer = GetComponent<SteamVR_LaserPointer>();
	}

  /*!
	 * \brief initializes variables
	 * \details This function initializes the pointers to the scene such as controllers, laser pointer, teleporter
	 */
    void OnEnable()
    {
        controllerInput.PadClicked += EnableTeleport;
        controllerInput.PadUnclicked += DisableTeleport;
    }

    /*!
    * \brief enables teleporting
    */
    void Start()
    {
        teleporter.teleportOnClick = false;
        pointer.pointer.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update () {

	   }

     /*!
   	 * \brief enables teleporting
   	 */
    private void EnableTeleport (object sender, ClickedEventArgs e)
    {
        teleporter.teleportOnClick = true;
        pointer.pointer.GetComponent<Renderer>().enabled = true;
    }

    /*!
    * \brief disables teleporting
    */
    private void DisableTeleport (object sender, ClickedEventArgs e)
    {
        teleporter.teleportOnClick = false;
        pointer.pointer.GetComponent<Renderer>().enabled = false;
    }
}
