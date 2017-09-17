/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Button Controller for Context menu
 */ 
using UnityEngine;
using System.Collections;

public class OMenuClick : MonoBehaviour {

	private UnityEngine.UI.Image activeImage;	//!< pointer to the button (image) gameobject

	private bool isOn = false;					//!< button status

    public SteamVR_TrackedObject controller;
	public Sprite onSprite;						//!< pointer to the on sprite
	public Sprite offSprite;					//!< pointer to the off sprite
    private SteamVR_LaserPointer pointer;		//!< pointer to the laser pointer


    /*!
	 * \brief Runs upon gameobject spawn (at start) and initializes image GO and pointer
	 */
    void Start () {
		activeImage = GetComponent<UnityEngine.UI.Image>(); // get the image component
        pointer = controller.GetComponent<SteamVR_LaserPointer>();
    }

	/*!
	 * \brief Toggles the button on/off
	 */
	public void swapSprite(){
		if (isOn){
			activeImage.sprite = offSprite;
			isOn = false;
		}
		else{
			activeImage.sprite = onSprite;
			isOn = true;
		}
	}

	/*!
	 * \brief sets the off sprite and flag
	 */
	public void turnOff() {
		if (!isOn)
			return;

		activeImage.sprite = offSprite;
		isOn = false;
	}

	/*!
	 * \brief sets the on sprite and flag
	 */
	public void turnOn() {
		if (isOn)
			return;

		activeImage.sprite = onSprite;
		isOn = true;
	}
}
