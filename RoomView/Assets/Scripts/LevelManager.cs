/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Level Manager
 * \bug No known bugs
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    /*!
     * \brief Scene Manager loads particular scene
     */
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    /*!
     * \brief clears entire scene
     * \details clears scene by reloading it
     */
    public void ResetLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
