/*!
 * \author HighFiveUTA Member
 * \author [Contributed] Luis Diaz Jr
 * \author [Doxygen] Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Pause Button Controller
 * \brief Controls the pause function
 */
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PButton : MenuButton
{

  /*!
	 * \brief actions available
	 */
    public enum action
    {
        reset, load, save, exit
    }

    public action selectedAction; //!< action to be done

    private GameObject levelManager; //!< pointer to level manager
    private LoadSubMenu subMenu; //!< pointer to load submenu

    /*!
  	 * \brief Runs upon when main menu is pulled up
  	 */
    public void Start()
    {
        levelManager = GameObject.Find("LevelManager");
        subMenu = gameObject.AddComponent<LoadSubMenu>();
    }

    /*!
  	 * \brief Loads respective menu depending on which botton was clicked
     * \details hides and shows either the load menu or save menu depending on which button was clicked
  	 */
    public override void OnSelectButton(object sender, ClickedEventArgs e)
    {
        switch (selectedAction)
        {
            case action.reset:
                Debug.Log("Cleared the scene");
                levelManager.GetComponent<LevelManager>().ResetLevel();
                break;

            case action.load:
                subMenu.hideMenus = new string[] {"Pause Menu"};
                subMenu.showMenus = new string[] {"Load Menu"};
                subMenu.OnSelectButton(sender, e);
                break;

            case action.save:
                subMenu.hideMenus = new string[] { "Pause Menu" };
                subMenu.showMenus = new string[] { "Save Menu" };
                subMenu.OnSelectButton(sender, e);
                break;

            case action.exit:
                levelManager.GetComponent<LevelManager>().LoadLevel("Start");
                break;

            default:
                break;
        }
    }

}
