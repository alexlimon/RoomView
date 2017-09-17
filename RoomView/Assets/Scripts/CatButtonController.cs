/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-26-2017
 *
 * \mainpage Catalog Button Controller
 * \bug The buttons may load in garbage sprites occasionally
 */ 
using UnityEngine;
using System.Collections;

public class CatButtonController : MonoBehaviour {

	public Sprite offSprite;								//!< pointer to the offSprite in Assets
	public Sprite onSprite;									//!< pointer to the onSprite in Assets
	public GameObject Preview;								//!< pointer to the gameOject holding the image preview
	public CatalogManager catalogManager;					//!< pointer to the catalog manager
	[Range(1, 8)]
	[Header("1-6 = cat buttons; 7 = back, 8 = forward")]
	public int buttonID; 									//!< ID for the button

	private bool isOn = false;  // button status			//!< on/off status flag
    private bool buttonSelected = false;					//!< on/off flag for sprites
	private UnityEngine.UI.Image backImage;					//!< pointer to the image object behind this button's preview image object (used for collision)
	private bool isSelected = false;						//!< on/off status flag

    public SteamVR_LaserPointer pointer;					//!< pointer for laserpointer
    public SteamVR_TrackedController controller;			//!< pointer for the controller


    /*!
	 * \brief Runs upon gameobject spawn (at start) and initializes listeners in the controller
	 * \details This function initializes the pointerpointer to the backImage that is used for collision. and initioalizes listeners
	 */
    void Start () {
		backImage = GetComponent<UnityEngine.UI.Image>();

        pointer.PointerIn += OnObjectHover;
        pointer.PointerOut += OffObjectHover;
    }
	
	/*!
	 * \brief Switches the button sprite and flags to ON. Adds click listener. 
	 * \details Runs when the pointer detects a collision with this gameObject.
	 */
	public virtual void OnObjectHover(object sender, PointerEventArgs e) {
        if ((e.target == GetComponent<Collider>().transform))
        {
            controller.TriggerClicked += OnSelectClick;
            isSelected = true;
            buttonSelected = true;
            turnOn();
        }
	}

	/*!
	 * \brief Runs the click. If ID < 6 spawn the desired object, else scroll.
	 * \details Runs when the controller detects a trigger click when this button is set to on
	 */
    public virtual void OnSelectClick(object sender, ClickedEventArgs e)
    {
        if (buttonSelected)
        {
  
            if (buttonID <= 6)
            {
                catalogManager.spawnByCatalogButton(buttonID, new Vector3(0, -100));
                isSelected = false;
                buttonSelected = false;
                turnOff();
                catalogManager.catOff();
            }
            else
            {
                if (buttonID == 7)
                    catalogManager.scrollBackward();
                else
                    catalogManager.scrollForward();
            }
        }
    }

	/*!
	 * \brief Switches the button sprite and flags to OFF. Removes click listener. 
	 * \details Runs when the pointer stops detecting a collision with this gameObject.
	 */
    public virtual void OffObjectHover(object sender, PointerEventArgs e) {
        if ((e.target == GetComponent<Collider>().transform))
        {
            controller.TriggerClicked -= OnSelectClick;
            isSelected = false;
            buttonSelected = false;
            turnOff();
        }
	}

	/*!
	 * \brief Switches the button sprite and flag to OFF.
	 */
	public void turnOff() {
		if (!isOn)
			return;

		backImage.sprite = offSprite;
		isOn = false;
	}

	/*!
	 * \brief Switches the button sprite and flag to ON.
	 */
	public void turnOn() {
		if (isOn)
			return;

		backImage.sprite = onSprite;
		isOn = true;
	}

	/*!
	 * \brief Switches ON/OFF status for toggle functionality
	 */
	public void swapSprite() {
		if (isOn) {
			backImage.sprite = offSprite;
			isOn = false;
		}
		else {
			backImage.sprite = onSprite;
			isOn = true;
		}
	}
}
