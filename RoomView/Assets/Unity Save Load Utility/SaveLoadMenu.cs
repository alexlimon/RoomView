using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SaveLoadMenu : MonoBehaviour
{

    public SaveLoadUtility slu;
    public bool showMenu = true;
    public bool showSave = false;
    public bool showLoad = false;
    private string saveGameName;
    private int selectedSaveGameIndex = -99;
    public List<SaveGame> saveGames;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (showLoad == true)
            {
                showLoad = false;
                showMenu = true;
                return;
            }

            if (showSave == true)
            {
                if (selectedSaveGameIndex != -99)
                {
                    selectedSaveGameIndex = -99;
                    Debug.Log("sdf");
                }
                else
                {
                    showSave = false;
                    showMenu = true;
                }
                return;
            }

            if (showMenu == true)
            {
                showMenu = false;
                return;
            }
            else
            {
                if (showLoad == false || showSave == false)
                {
                    showMenu = true;
                    return;
                }
            }
        }



        //The classic hotkeys for quicksaving and quickloading
        if (Input.GetKeyDown(KeyCode.F5))
        {
            slu.SaveGame(slu.quickSaveName);//Use this for quicksaving, which is basically just using a constant savegame name.
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            slu.LoadGame(slu.quickSaveName);//Use this for quickloading, which is basically just using a constant savegame name.

        }
    }


    void OnGUI()
    {

        if (showMenu == false && showLoad == false && showSave == false)
        {
            if (GUILayout.Button("Menu"))
            {
                showMenu = true;
                return;
            }
        }

        if (showMenu == true)
        {
            GUILayout.BeginVertical(GUILayout.MinWidth(300)); // BeginVertical - All controls rendered inside this element will be placed vertically below each other. 
                                                              // The group must be closed with a call to EndVertical.

            if (GUILayout.Button("Save"))
            { // Button - Make a single press button. The user clicks them and something happens immediately. Returns TRUE when clicked
                showMenu = false;
                showLoad = false;
                saveGames = SaveLoad.GetSaveGames(slu.saveGamePath, slu.usePersistentDataPath); // public List<SaveGame> saveGames; 
                showSave = true;

                return;
            }

            if (GUILayout.Button("Load"))
            {
                showSave = false;
                showMenu = false;
                saveGames = SaveLoad.GetSaveGames(slu.saveGamePath, slu.usePersistentDataPath); // public List<SaveGame> saveGames;
                if (saveGames.Count >= 0)
                {
                    showLoad = true;
                }
                else
                {
                    showMenu = true;
                }
                return;
            }

            if (GUILayout.Button("Close"))
            {
                showSave = false;
                showMenu = false;
                showLoad = false;
                return;
            }

            if (GUILayout.Button("Exit to Windows"))
            {
                Application.Quit();
                return;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        if (showLoad == true)
        { // new menu created to show option to load current saved files. Menu screen after main menu. Shown when user clicks load from main menu
            GUILayout.BeginVertical(GUILayout.MinWidth(300));

            foreach (SaveGame saveGame in saveGames)
            {
                if (GUILayout.Button(saveGame.savegameName + " (" + saveGame.saveDate + ")"))
                { // if the button of saved file valid, load it
                    slu.LoadGame(saveGame.savegameName);
                    showLoad = false;
                    return;
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back", GUILayout.MaxWidth(100)))
            { // back button at the bottom of the load menu, if pressed, goes back to main menu
                showLoad = false;
                showMenu = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        if (showSave == true)
        { // new menu created to show option to save & current saved files. Menu screen after main menu. Shown when user clicks save from main menu

            GUILayout.BeginVertical(GUILayout.MinWidth(300));

            for (int i = -1; i < saveGames.Count; i++)
            { // public List<SaveGame> saveGames;
              // in case there isn't any saved games already, the i is set to -1 so the "New" save button option is available

                if (i == selectedSaveGameIndex)
                { // private int selectedSaveGameIndex = -99; inital value.

                    GUILayout.BeginHorizontal(GUILayout.MinWidth(300));

                    string str = "Room: " + (i + 1); //GUILayout.TextField(saveGameName, GUILayout.MinWidth(200)); // creates the text field a user can edit
                                                                                             // str = user input (file name "to-be" saved)
                                                                                             // saveGameName - 	Text to edit. The return value of this function (TextField) should be assigned back to the string (saveGameName)

                    if (regularExpression.IsMatch(str))
                    { //Indicates whether the regular expression specified in the Regex constructor finds a match in a specified input string.
                      // I think all the isMatch is checking for from the "str = user input string" is whether it has the specific valid characters
                      // described at the top of the file
                        if (str.IndexOfAny(newLine) != -1)
                        {
                            //New Line detected
                            if (i >= 0)
                            {
                                SaveLoad.DeleteFile(slu.saveGamePath, saveGames[i].savegameName);
                            }
                            slu.SaveGame(saveGameName); // game is saved
                            selectedSaveGameIndex = -99; // resets 
                            return;
                        }
                        else
                        {
                            saveGameName = str; //All OK, copy
                        }
                    }
                    else
                    {
                        Debug.Log("Irregular expression detected");
                    }

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Save", GUILayout.MaxWidth(50)))
                    {
                        if (i >= 0)
                        {
                            SaveLoad.DeleteFile(slu.saveGamePath, saveGames[i].savegameName);
                        }
                        slu.SaveGame(saveGameName);
                        selectedSaveGameIndex = -99;
                        saveGames = SaveLoad.GetSaveGames(slu.saveGamePath, slu.usePersistentDataPath);
                        return;
                    }

                    if (GUILayout.Button("Cancel", GUILayout.MaxWidth(50)))
                    {
                        selectedSaveGameIndex = -99;
                        return;
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (i == -1)
                    {
                        if (GUILayout.Button("(New)"))
                        {
                            selectedSaveGameIndex = i; // initially selectedSaveGameIndex = -99, this is right after main menu save button is clicked.
                            saveGameName = "";         // i = - 1, again, the user has now clicked the "New" button in this conditional 
                            return; // returns top of for loop
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(saveGames[i].savegameName + " (" + saveGames[i].saveDate + ")"))
                        {
                            selectedSaveGameIndex = i;
                            saveGameName = saveGames[i].savegameName;
                            return;
                        }
                    }
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back", GUILayout.MaxWidth(100)))
            {
                if (selectedSaveGameIndex != -99)
                {
                    selectedSaveGameIndex = -99;
                }
                else
                {
                    showSave = false;
                    showMenu = true;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

        } // end of if(showSave == true)
    }
}

