using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenu; //!< pointer to the main menu
    
    /*!
  	 * \brief Loads main menu
  	 * \details Loads main menu when menu button is pressed
  	 */
    void Start()
    {
        GameObject menu = Instantiate(mainMenu);
        menu.transform.parent = gameObject.transform;
    }
}
