/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Menu Button
 * \bug No known bugs
 */
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {


    [HideInInspector]
    public SteamVR_TrackedObject controller; //!< pointer to the controller

    [HideInInspector]
    public SteamVR_TrackedController controllerInput; //!< pointer to the controller inputs
    [HideInInspector]
    public SteamVR_LaserPointer pointer; //!< pointer to the laser pointer at end of controller

    /*!
  	 * \brief initializes variables
  	 * \details This function initializes the pointers to the scene such as controllers,
  	 */
    void Awake () {
        GameObject controllerObject = GameObject.FindGameObjectWithTag("Right Controller") as GameObject;
        if (controllerObject != null & controller == null)
            controller = controllerObject.GetComponent<SteamVR_TrackedObject>();
        else
            Debug.Log("controllerObject returned null");
	    if(controller !=null )
        {
            try
            {
                pointer = controller.GetComponent<SteamVR_LaserPointer>();
                controllerInput = controller.GetComponent<SteamVR_TrackedController>();
            }
            catch
            {
                Debug.LogError(this.name +": Could not find LaserPointer");
            }
        }
	}

    /*!
     * \brief makes menu button clickable when hovered over
     */
    private void OnEnable()
    {
        pointer.PointerIn += OnHover;
        pointer.PointerOut += OffHover;
    }

    /*!
  	 * \brief check to see if laser pointer is projected onto menu option of this button
  	 * \details This function changes color of button (option in menu) and allows it to be selected if hovered
  	 */
    public virtual void OnHover(object sender, PointerEventArgs e)
    {
        if(e.target == GetComponent<Collider>().transform)
        {
            gameObject.GetComponent<Text>().color = Color.blue;
            controllerInput.TriggerClicked += OnSelectButton;
        }
    }

    /*!
  	 * \brief check to see if laser pointer is projected onto menu option of this button
  	 * \details This function changes color of button (option in menu) and allows it to be selected if hovered
  	 */
    public virtual void OffHover(object sender, PointerEventArgs e)
    {
        if(e.target == GetComponent<Collider>().transform)
        {
            gameObject.GetComponent<Text>().color = Color.white;
            controllerInput.TriggerClicked -= OnSelectButton;
        }
    }

    public virtual void OnSelectButton(object sender, ClickedEventArgs e)
    {
        Debug.LogError(this.name + ": No OnSelectButton method has been defined");
    }
}
