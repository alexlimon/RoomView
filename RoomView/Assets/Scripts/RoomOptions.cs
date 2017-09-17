/*!
 * \author HighFiveUTA Member
 * \author [Contributed] Luis Diaz Jr
 * \author [Doxygen] Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Room Options
 * \brief Controls the pause function
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOptions : MonoBehaviour {

  public SteamVR_TrackedController controllerInput; //!< pointer to controller inputs
	public CatalogManager prefabCatalog; //!< pointer to furniture catalog

	[HideInInspector]
	public bool optionsDisplaying = false;


	private SteamVR_LaserPointer pointer; //!< pointer to SteamVr laser posinter
    private GameObject menuManager; //!< pointer to menu manager
    private bool isSelected = false; //!< [unused] is button selected
    private bool buttonSelected = false; //!< [unused] is button selected
    private bool catManIsActive; //!< [unused] is catalog active

    void Awake()
    {
        controllerInput.MenuButtonClicked += displayInRoomMenu;
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    public virtual void displayInRoomMenu(object sender, ClickedEventArgs e)
    {

        if (optionsDisplaying)
        {
            optionsOff();
        }
        else
        {
            optionsOn();
        }
    }
    public void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }

    public virtual void onSelectClick(object sender, ClickedEventArgs e)
    {

    }
    public virtual void offSelectClick(object sender, ClickedEventArgs e)
    {


    }
    public void optionsOn()
    {
		if (prefabCatalog.isActive)
		{
			prefabCatalog.catOff();
			catManIsActive = true;
		}

		else
		{
			catManIsActive = false;
		}
		gameObject.SetActive(true);


        transform.position = Camera.main.transform.position + Camera.main.transform.forward * 6.0f;

        optionsDisplaying = true;
        raycastIgnoreOtherObjects();
    }
    public void optionsOff()
    {
        transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        gameObject.SetActive(false);
        optionsDisplaying = false;
        raycastHitOtherObjects();

        if (catManIsActive)
        {
			prefabCatalog.catOn();
        }
    }


    //Move all GameObjects to IgnoreRaycast layer
    private void raycastIgnoreOtherObjects()
    {
        GameObject[] sceneObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject gObject in sceneObjects)
        {
            GameObject parentObject;
            Transform parentTransform = gObject.transform;
            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;
            }
            parentObject = parentTransform.gameObject;
            if (parentObject != gameObject)
            {
                gObject.layer = 2;
            }
        }
    }

    //Move all GameObjects back to default layer
    private void raycastHitOtherObjects()
    {
        GameObject[] sceneObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject gObject in sceneObjects)
        {
            GameObject parentObject;
            Transform parentTransform = gObject.transform;
            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;
            }
            parentObject = parentTransform.gameObject;
            if (parentObject != gameObject)
            {
				if (parentObject.GetComponent<Furniture>() != null && parentObject.GetComponent<Furniture>().isMove)
				{
					gObject.layer = 2;
				}

				else
				{
					gObject.layer = 0;
				}
            }
        }
    }

}
