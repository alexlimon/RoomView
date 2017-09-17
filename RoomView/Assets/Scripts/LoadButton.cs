/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Load Button
 * \bug No known bugs
 */

﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MenuButton
{

    public SaveLoadUtility slu; //!< pointer to the save load utility
    private string saveGameName; //!< name of a particular room
    private int selectedSaveGameIndex = -99; //!< default saved room index
    public List<SaveGame> saveGames; //!< list of saved games (rooms)
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
     * \brief initializes variables
     * \details This function initializes the save load utility
     */
    void Start()
    {
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
  	 * \brief loadGame coroutine takes precedence over other co routines
  	 * \details stops  all other coroutines and prioritize the load game coroutine
  	 */
    public override void OnSelectButton(object sender, ClickedEventArgs e)
    {
		//slu.LoadGame(gameObject.GetComponent<Text>().text);
		StopAllCoroutines();
		StartCoroutine(loadGame());
	}

  /*!
   * \brief load previusly saved game using custom save load
   */
	private IEnumerator loadGame()
	{
		Debug.Log("Custom SaveEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
		new SaveLoad_Custom().LoadData(this.GetComponent<Text>().text);
		yield return null;
	}
}
