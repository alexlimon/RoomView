/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Context Menu Button Controller
 */ 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContextMenuButton : MenuButton {

	
    public enum action		
    {
        move, rotate, clone, del
    }								//!< enum that defines four actions to perform

    public Sprite onSprite;     	//!< pointer to on sprite
    public Sprite offSprite;    	//!< pointer to off sprite
    public action selectedAction;	//!< holds the currently selected action

    private GameObject snappedObject;			//!< pointer to the object this menu will hover around
    private static bool isContextMenu; 			//!< flag to detect if this menu is currently active
    private bool isOn = false;					//!< button status
    private UnityEngine.UI.Image buttonImage;	//!< pointer to image gameobject

	/*!
	 * \brief Runs upon gameobject spawn (at start) and initializes variables, sets listeners, and snapped object
	 */
    private void Start()
    {
        buttonImage = GetComponent<UnityEngine.UI.Image>();
        controllerInput.TriggerClicked += OnTriggerCloseContextMenu;
        snappedObject = transform.parent.gameObject.GetComponent<UI_Follower>().snappedObject;
    }

	/*!
	 * \brief sets the off sprite and flag
	 */
    public void turnOff()
    {
        if (!isOn)
            return;

        buttonImage.sprite = offSprite;
        isOn = false;
    }

	/*!
	 * \brief sets the on sprite and flag
	 */
    public void turnOn()
    {
        if (isOn)
            return;

        buttonImage.sprite = onSprite;
        isOn = true;
    }

	/*!
	 * \brief removes the snapped object and removes highlight from deselected object
	 */
    private void DeselectFurniture()
    {
        snappedObject.GetComponent<Furniture>().isSelected = false;
        Furniture.isFurnitureSelected = false;
    }

	/*!
	 * \brief sets the flag to start a move in selected object
	 */
    private void MoveFurniture()
    {
        snappedObject.layer = 2;
        snappedObject.GetComponent<Furniture>().isMove = true;
    }

	/*!
	 * \brief Begins the listener to rotate the selected object
	 * \details Hides the context menu button while keeping it in the scene. This prevents event listener from being lost before the user is done rotating the object.
	 */
    private void RotateFurniture()
    {
        foreach(Transform child in transform.parent)
        {
            child.gameObject.SetActive(false);
        }
        controllerInput.PadClicked += OnPadClickedRotate;
        controllerInput.PadUnclicked += OnPadUnclickedStopRotate;
    }

	/*!
	 * \brief Clones the selected(snapped) object and sets the flags to begin placement(move flags)
	 */
    private void CloneFurniture()
    {
        DeselectFurniture();
        GameObject cloneObject = Instantiate(snappedObject);
        cloneObject.GetComponent<Furniture>().isClone = true;
        cloneObject.GetComponent<Furniture>().isSelected = true;
        cloneObject.GetComponent<Furniture>().fromCatalog = false;
        cloneObject.layer = 2;
        cloneObject.GetComponent<Furniture>().isMove = true;
        //cloneObject.GetComponent<Furniture>().materialArray = snappedObject.GetComponent<Furniture>().materialArray;
        Furniture.isFurnitureSelected = true;
    }

	/*!
	 * \brief Closes the context menu
	 * \details Closes when clicked off the menu or at a different object
	 */
    private void OnTriggerCloseContextMenu(object sender, ClickedEventArgs e)
    {
        if (!isContextMenu)//If not pointed at another context menu button
        {
            snappedObject.GetComponent<Furniture>().isRotate = false;
            DeselectFurniture();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

	/*!
	 * \brief Sets button to ON(hovered)
	 * \details Sets HOVERED flags and turns on the sprite
	 */
    public override void OnHover(object sender, PointerEventArgs e)
    {
        if (e.target == GetComponent<Collider>().transform)
        {
            isContextMenu = true;
            turnOn();
            controllerInput.TriggerClicked += OnSelectButton;
        }
    }
	
	/*!
	 * \brief Sets button to OFF
	 * \details Sets HOVERED flags and turns off the sprite
	 */
    public override void OffHover(object sender, PointerEventArgs e)
    {
        if (e.target == GetComponent<Collider>().transform)
        {
            isContextMenu = false;
            turnOff();
            controllerInput.TriggerClicked -= OnSelectButton;
        }
    }
	
	/*!
	 * \brief Performs selected action when trigger is clicked
	 * \details Once clicked, it checks the selected action and performs that action.
	 */
    public override void OnSelectButton(object sender, ClickedEventArgs e)
    {
        Debug.Log(selectedAction + "context menu button pressed");
        switch (selectedAction)
        {
            case action.move:
                //TODO move furniture code
                MoveFurniture();
                Destroy(gameObject.transform.parent.gameObject);
                break;

            case action.rotate:
                RotateFurniture();
                break;

            case action.clone:
                //TODO clone furniture code
                CloneFurniture();
                Destroy(gameObject.transform.parent.gameObject);
                break;

            case action.del:
                DeselectFurniture();
                Destroy(snappedObject);
                Destroy(gameObject.transform.parent.gameObject);
                break;
        }
    }

	/*!
	 * \brief Sets rotate flags when touchpad pad is pressed
	 */
    private void OnPadClickedRotate(object sender, ClickedEventArgs e)
    {
        snappedObject.GetComponent<Furniture>().isRotate = true;
    }

	/*!
	 * \brief Sets rotate flags when touchpad pad is released
	 */
    private void OnPadUnclickedStopRotate(object sender, ClickedEventArgs e)
    {
        snappedObject.GetComponent<Furniture>().isRotate = false;
    }

	/*!
	 * \brief Runs when the menu is removed from the scene
	 * \details removes all listeners to prevent them from using resources
	 */
    private void OnDestroy()
    {
        Debug.Log("Running OnDestroy for Context Menu Button: " + selectedAction);
        
        pointer.PointerIn -= OnHover;
        pointer.PointerOut -= OffHover;
        controllerInput.TriggerClicked -= OnTriggerCloseContextMenu;
        controllerInput.TriggerClicked -= OnSelectButton;
        controllerInput.PadClicked -= OnPadClickedRotate;
        controllerInput.PadUnclicked -= OnPadUnclickedStopRotate;
    }
}
