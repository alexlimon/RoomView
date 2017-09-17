/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage The Filter Button Controller
 * \bug The buttons may not show the proper sprites, check them in the inspector
 */ 
using UnityEngine;
using System.Collections;

public class FilterButton : MonoBehaviour {

	public CatalogManager manager;						//!< pointer to the catalog
	public Sprite offSprite;							//!< points to the offSprite in assets
	public Sprite onSprite;								//!< points to the onSprite in assets
	public bool isOn = false;  							//!< button status
	public bool roomFilterActive = false;				//!< flag if a roomType filter is active
	public bool typeFilterActive = false;				//!< flag if a objType filter is active
	[Range(9, 27)]
	[Header("9-16 room filters; 17-27 type filters")]
	public int buttonID;								//!< this button's ID
	public ObjectCategory.ROOMCODE buttonRoomCode;		//!< this button's roomcode (filter)
	public ObjectCategory.OBJECTTYPE buttonTypeCode;	//!< this buttons objectCode (filter)
    public SteamVR_LaserPointer pointer;				//!< pointer to the laserpointer
    public SteamVR_TrackedController controller;		//!< pointer to the controller
    public CatalogManager catalogManager;				//!< [duplicate] pointer to the catalog

    private UnityEngine.UI.Image buttonImage;			//!< pointer to the image gameobject
	private bool isSelected = false;					//!< hovered flag
    private bool buttonSelected = false;				//!< selected flag
    private static ObjectCategory.ROOMCODE  filterRoomCode = ObjectCategory.ROOMCODE.ALL;		//!< global filter value [room] for comparison between all buttons
	private static ObjectCategory.OBJECTTYPE filterTypeCode = ObjectCategory.OBJECTTYPE.ALL;	//!< global filter value [object] for comparison between all buttons

	/*!
	 * \brief Runs upon gameobject reEnable and initializes variables
	 * \details This function initializes its own filter values.
	 */
	void OnEnable() {
		if(buttonImage == null) {
			buttonImage = GetComponent<UnityEngine.UI.Image>();
		}

		if (buttonID == 16) {
			filterRoomCode = ObjectCategory.ROOMCODE.ALL;
			//manager.FiltRoomCode = filterRoomCode;
			isOn = false;
			roomFilterActive = true;
			isSelected = false;
			turnOn();
		}
		else if (buttonID == 26) {
			filterTypeCode = ObjectCategory.OBJECTTYPE.ALL;
			//manager.FiltTypeCode = filterTypeCode;
			isOn = false;
			typeFilterActive = true;
			isSelected = false;
			turnOn();
		}
		else {
			isOn = true;
			roomFilterActive = false;
			typeFilterActive = false;
			isSelected = false;
			turnOff();
		}
		
	}

	/*!
	 * \brief Runs upon gameobject spawn (at start) and initializes listeners
	 * \details This function initializes the pointers to the scene gameObjects and listeners
	 */
	void Start() {
		if (buttonImage == null) {
			buttonImage = GetComponent<UnityEngine.UI.Image>();
		}

        pointer.PointerIn += OnObjectHover;
        pointer.PointerOut += OffObjectHover;
    }

	/*!
	 * \brief checks to see if the button is still on after a different filter has been selected. Turns them off
	 * \details Runs after every frame. 
	 */
    void LateUpdate()
    {
        if (buttonID < 17)
        {
            if (isOn && filterRoomCode != buttonRoomCode && !isSelected)
            {
                roomFilterActive = false;
                turnOff();
            }
        }
        else
        {
            if (isOn && filterTypeCode != buttonTypeCode && !isSelected)
            {
                typeFilterActive = false;
                turnOff();
            }
        }

    }

	/*!
	 * \brief Turns on the button when hovered
	 * \details Runs when the pointer detects a collision. changes the sprite and flags to ON
	 */
    public virtual void OnObjectHover(object sender, PointerEventArgs e)
    {
        if ((e.target == GetComponent<Collider>().transform))
        {
            controller.TriggerClicked += OnSelectClick;
            isSelected = true;
            buttonSelected = true;
            turnOn();
        }
    }

	/*!
	 * \brief Runs the selected filter
	 * \details Sets the appropriate filter
	 */
    public virtual void OnSelectClick(object sender, ClickedEventArgs e)
    {
        if (buttonSelected)
        {
            isSelected = false;

            if (buttonID < 17)
            {
                if (!roomFilterActive)
                    roomFilterActive = true;
                else
                    return;
            }
            else
            {
                if (!typeFilterActive)
                    typeFilterActive = true;
                else
                    return;
            }



            switch (buttonID)
            {

                // cases for room filters
                case 9:
                    filterRoomCode = ObjectCategory.ROOMCODE.KITCHEN;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.KITCHEN;
                    roomFilterActive = true;
                    break;
                case 10:
                    filterRoomCode = ObjectCategory.ROOMCODE.BATHROOM;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.BATHROOM;
                    roomFilterActive = true;
                    break;
                case 11:
                    filterRoomCode = ObjectCategory.ROOMCODE.BEDROOM;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.BEDROOM;
                    roomFilterActive = true;
                    break;
                case 12:
                    filterRoomCode = ObjectCategory.ROOMCODE.LIVING;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.LIVING;
                    roomFilterActive = true;
                    break;
                case 13:
                    filterRoomCode = ObjectCategory.ROOMCODE.DINING;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.DINING;
                    roomFilterActive = true;
                    break;
                case 14:
                    filterRoomCode = ObjectCategory.ROOMCODE.REC;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.REC;
                    roomFilterActive = true;
                    break;
                case 15:
                    filterRoomCode = ObjectCategory.ROOMCODE.OUTDOORS;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.OUTDOORS;
                    roomFilterActive = true;
                    break;
                case 16:
                    filterRoomCode = ObjectCategory.ROOMCODE.ALL;
                    manager.FiltRoomCode = ObjectCategory.ROOMCODE.ALL;
                    roomFilterActive = true;
                    break;


                // cases for type filters
                case 17:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.APPLIANCE;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 18:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.COMFORT;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 19:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.DECORATION;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 20:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.ELECTRONIC;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 21:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.KIDS;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 22:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.LIGHTING;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 23:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.PLUMBING;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 24:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.SURFACES;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 25:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.MISC;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;
                case 26:
                    filterTypeCode = ObjectCategory.OBJECTTYPE.ALL;
                    manager.FiltTypeCode = filterTypeCode;
                    typeFilterActive = true;
                    break;

                default:
                    break;
            }
        }
    }

	/*!
	 * \brief Turns off the button when not hovered
	 * \details Runs when the pointer no longer detects a collision. changes the sprite and flags to OFF
	 */
    public virtual void OffObjectHover(object sender, PointerEventArgs e)
    {
        if ((e.target == GetComponent<Collider>().transform))
        {
            controller.TriggerClicked -= OnSelectClick;
            isSelected = false;
            buttonSelected = false;
            turnOff();
        }
    }

	/*!
	 * \brief sets the off sprite and flag
	 */
	public void turnOff() {
		if (!isOn)
			return;

		if (roomFilterActive || typeFilterActive)
			return;

		buttonImage.sprite = offSprite;
		isOn = false;
	}

	/*!
	 * \brief sets the on sprite and flag
	 */
	public void turnOn() {
		if (isOn)
			return;

		buttonImage.sprite = onSprite;
		isOn = true;
	}
}
