/*!
 * \author HighFiveUTA Member
 * \author [Contributed] Luis Diaz Jr
 * \author [Doxygen] Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Save Button Controller
 * \brief Controls the save function
 */ 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MenuButton {

    public SaveLoadUtility slu;						//!< pointer to the SaveLoadUtility
    private string saveGameName;					//!< filename
    private int selectedSaveGameIndex = -99;		//!< [unused] savegame index
    public List<SaveGame> saveGames;				//!< [unused] list of saved games
    private char[] newLine = "\n\r".ToCharArray();

	
    private Regex regularExpression = new Regex("^[a-zA-Z0-9_\"  *\"]*$"); // A regular expression is a pattern that could be matched against an input text. 
                                                                           /*Regular expression, contains only upper and lowercase letters, numbers, and underscores.

                                                                                 * ^ : start of string
                                                                                [ : beginning of character group
                                                                                a-z : any lowercase letter
                                                                                A-Z : any uppercase letter
                                                                                0-9 : any digit
                                                                                _ : underscore
                                                                                ] : end of character group
                                                                                * : zero or more of the given characters
                                                                                $ : end of string

                                                                            */
	
	/*!
	 * \brief Runs upon gameobject spawn (at start) and initializes variables
	 */
    void Start () {
        if (slu == null)
        {
            slu = GetComponent<SaveLoadUtility>();
            if (slu == null)
            {
                Debug.Log("[SaveLoadMenu] Start(): Warning! SaveLoadUtility not assigned!");
            }
        }
    }
	
	/*!
	 * \brief Runs upon when save slot is selected in the GUI
	 * \details Stops any previous saves if running, starts the save function [Async]
	 */
    public override void OnSelectButton(object sender, ClickedEventArgs e)
    {
		StopAllCoroutines();
		StartCoroutine(saveGame());
        //slu.SaveGame(gameObject.GetComponent<Text>().text);
		
    }

	/*!
	 * \brief [Async]Starts a save
	 */
	private IEnumerator saveGame()
	{
		Debug.Log("Custom SaveEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
		new SaveLoad_Custom().SaveData(this.GetComponent<Text>().text);
		yield return null;
	}
}
