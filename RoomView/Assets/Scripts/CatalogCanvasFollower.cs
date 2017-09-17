/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage The Catalog Canvas Follower
 */ 
using UnityEngine;
using System.Collections;

public class CatalogCanvasFollower : MonoBehaviour {

	public GameObject cam;	//!< pointer to the main camera (player camera)
    
	/*!
	 * \brief Sets the canvas to always face the user
	 * \details This function runs every frame and flips the forward vector to point at the camera properly.
	 */
    void Update () {
		// not exactly sure why this works but it flips look at
		transform.LookAt(2 * transform.position - cam.transform.position);
       
	}
}
