/*!
 * \author HighFiveUTA Member
 * \author [Contributed] Luis Diaz Jr
 * \author [Doxygen] Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage RoomSelectionMenu
 * \brief allows one to select preset room
 */
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectionMenu : MonoBehaviour {

	/*!
	 * \brief initalization of varables
	 */
	void Start () {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
	}

	// Update is called once per frame
	void Update () {

	}
}
