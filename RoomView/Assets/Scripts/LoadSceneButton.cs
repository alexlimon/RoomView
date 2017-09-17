/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Load Scene Button
 * \bug No known bugs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MenuButton {

    public string levelName; //!< name of room
    public LevelManager levelManager; //!< pointer to the levelManager

    /*!
  	 * \brief Loads a particular scene when Load scene button is pressed
  	 * \details Level manager loads a particular scene depending on whichever was selected from the menu
  	 */
    public override void OnSelectButton(object sender, ClickedEventArgs e)
    {
        levelManager.LoadLevel(levelName);
    }
}
