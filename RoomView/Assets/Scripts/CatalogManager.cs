/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage The Catalog Manager 
 * \bug The buttons may glitch and take on the image of whatever sprite was loaded in last in the catalog onscreen
 */ 
using UnityEngine;
using System.Collections;
using System.IO;

public class CatalogManager : MonoBehaviour
{
	public Canvas catalogCanvas;						//!< pointer to the canvas that holds the catalog GUI gameObjects
	public SteamVR_TrackedController controllerInput;	//!< pointer to the controller
	public RoomOptions roomOptions;
	[HideInInspector]
	public bool isActive = false;            	//!< flag to show if UI is on screen

	private string[] catalogNames;            	//!< holds names of all prefabs in the prefabs folder (full path name)
	private Sprite[] currentCatSprites;      	//!< currently loaded sprites (up to six)
	private ObjectCategory[] categories;        //!< holds filter codes for all prefabs
	private int catalogSize;                    //!< size of catalog array (count of all objects available for placement)
	private int catalogStart;                  	//!< display start index
	private int catalogStop;                    //!< display stop index
	private UnityEngine.UI.Image[] previews;    //!< pointer gameobject[] for the prefab previews
	private UnityEngine.UI.Image[] backs;      	//!< pointer gameobject[] for the prefab preview buttons

	private ObjectCategory.ROOMCODE filtRoomCode = ObjectCategory.ROOMCODE.ALL;      //!< filter code: room type
	private ObjectCategory.OBJECTTYPE filtTypeCode = ObjectCategory.OBJECTTYPE.ALL;  //!< filter code: object type
    private int[] indexInFilteredCatalog;      	//!< array to store indexes of gameobjects in "catalog names" that match the current filters
    private int filteredCatalogSize;            //!< size of the filtered index array
    private int filtCatalogStart;              	//!< display start index of the filtered index array
    private int filtCatalogStop;                //!< display stop index of the filtered index array
    private bool filterActive = false;        	//!< flag for filters
    private UnityEngine.UI.Text titleText;    	//!< placeholder for UI title text




	/*!
	 * \brief Runs upon gameobject spawn (at start) and initializes variables
	 * \details This function initializes the pointers to the scene gameObjects, loads the sprites\prefabs from memory, and sets the controller listeners.
	 */
	public void Start()
	{
		titleText = GameObject.Find("Title Text").GetComponent<UnityEngine.UI.Text>();
		titleText.text = "Catalog > Room: All	Category: All";
		//UnityEditor.AssetPreview.SetPreviewTextureCacheSize(100);

		catalogNames = System.IO.Directory.GetFiles("Assets/Resources/Prefabs", "*.prefab");
		catalogSize = catalogNames.Length;
		print(catalogSize);
		currentCatSprites = new Sprite[6];
		categories = new ObjectCategory[catalogSize];
		categories = new ObjectCategory[catalogSize];

		StartCoroutine(getObjectCodes());


		indexInFilteredCatalog = new int[catalogSize];
		catalogStart = 0;

		if (catalogSize > 6)
			catalogStop = 6;
		else
			catalogStop = catalogSize;

		previews = new UnityEngine.UI.Image[6];
		backs = new UnityEngine.UI.Image[6];
		previews[0] = GameObject.Find("Preview1").GetComponent<UnityEngine.UI.Image>();
		previews[1] = GameObject.Find("Preview2").GetComponent<UnityEngine.UI.Image>();
		previews[2] = GameObject.Find("Preview3").GetComponent<UnityEngine.UI.Image>();
		previews[3] = GameObject.Find("Preview4").GetComponent<UnityEngine.UI.Image>();
		previews[4] = GameObject.Find("Preview5").GetComponent<UnityEngine.UI.Image>();
		previews[5] = GameObject.Find("Preview6").GetComponent<UnityEngine.UI.Image>();
		backs[0] = GameObject.Find("Back1").GetComponent<UnityEngine.UI.Image>();
		backs[1] = GameObject.Find("Back2").GetComponent<UnityEngine.UI.Image>();
		backs[2] = GameObject.Find("Back3").GetComponent<UnityEngine.UI.Image>();
		backs[3] = GameObject.Find("Back4").GetComponent<UnityEngine.UI.Image>();
		backs[4] = GameObject.Find("Back5").GetComponent<UnityEngine.UI.Image>();
		backs[5] = GameObject.Find("Back6").GetComponent<UnityEngine.UI.Image>();

		StartCoroutine(LoadObjectPreviewsCo()); //LoadAllObjectPreviews();
												//ShowObjectPreviews();

		catalogCanvas.gameObject.SetActive(false);
		controllerInput.MenuButtonClicked += displayCatalog;
	}

	/*!
	 * \brief Populates the categories array with each object's filter codes
	 * \details [Async Task] This function grabs a copy of the Categories component in each prefab to help with sorting.
	 */
	IEnumerator getObjectCodes()
	{
		for (int i = 0; i < catalogSize; i++)
			categories[i] = Resources.Load<ObjectCategory>("Prefabs/" + catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25));

		yield return null;
	}

	/*!
	 * \brief Loads up to 6 previews into memory to be displayed
	 * \details [Async Task] The function uses the start/stop index variables to decide which previews to load from memory
	 */
	IEnumerator LoadObjectPreviewsCo()
	{
		Texture2D texture;
		Sprite newSprite;
		GameObject[] frigThisArray = new GameObject[6];

		for (int i = catalogStart; i < catalogStop; i++)
		{
			print(catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25));
			frigThisArray[i % 6] = Resources.Load<GameObject>("Prefabs/" + catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25));
			UnityEditor.AssetPreview.GetAssetPreview(frigThisArray[i % 6]);

		}

		Debug.Log("Waiting for load");
		yield return new WaitForSeconds(0.5f);

		for (int i = catalogStart; i < catalogStop; i++)
		{
			texture = null;
			texture = UnityEditor.AssetPreview.GetAssetPreview(frigThisArray[i % 6]);
			//Instantiate(frigThisArray[i%6]);
			if (texture == null)
			{
				Debug.Log("Loading object: " + "Prefabs/" + catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25));
				yield return new WaitForSeconds(0.1f);
				i--;
				continue;
			}
			newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			currentCatSprites[i % 6] = newSprite;
			//Instantiate(Resources.Load<GameObject>("Prefabs/" + catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25)));
		}

		ShowCurrentObjectPreviews();
		Debug.Log("Done Loading");
		yield return null;
	}

	/*!
	 * \brief [Filter] Loads up to 6 previews into memory to be displayed
	 * \details [Async Task] The function uses the filter start/stop index variables to decide which previews to load from memory
	 */
	IEnumerator LoadFilteredObjectPreviewsCo()
	{
		Texture2D texture;
		Sprite newSprite;

		print(filtCatalogStart);
		print(filtCatalogStop);
		for (int i = filtCatalogStart; i < filtCatalogStop; i++)
		{
			print(catalogNames[indexInFilteredCatalog[i]].Substring(25, catalogNames[indexInFilteredCatalog[i]].Length - 7 - 25));
			UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/" + catalogNames[indexInFilteredCatalog[i]].Substring(25, catalogNames[indexInFilteredCatalog[i]].Length - 7 - 25)));
		}

		Debug.Log("Waiting for load");
		yield return new WaitForSeconds(0.5f);
		

		for (int i = filtCatalogStart; i < filtCatalogStop; i++)
		{
			texture = null;
			texture = UnityEditor.AssetPreview.GetAssetPreview(Resources.Load<GameObject>("Prefabs/" + catalogNames[indexInFilteredCatalog[i]].Substring(25, catalogNames[indexInFilteredCatalog[i]].Length - 7 - 25)));
			if (texture == null)
			{

				Debug.Log("waiting");
				yield return new WaitForSeconds(0.1f);
				i--;
				continue;
			}

			newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			currentCatSprites[i % 6] = newSprite;
		}

		Debug.Log("Done Loading");
		ShowCurrentFilteredPreviews();
		yield return null;
	}

	/*!
	 * \brief Loads the previews into the UI (for filters)
	 * \details uses the start/stop indexes to only load the needed buttons and sprites
	 */
	private void ShowCurrentFilteredPreviews()
	{
		int previewsToShow;
		previewsToShow = filtCatalogStop % 6;
		

		if(filteredCatalogSize == 0) {
			toggleCatButtons(-1);
			return;
		}
		else
			toggleCatButtons(previewsToShow);

		for (int i = 0, showing = filtCatalogStart; showing < filtCatalogStop; i++, showing++)
		{
			previews[i].sprite = currentCatSprites[i];
		}
	}

	/*!
	 * \brief Loads the previews into the UI (for filters)
	 * \details uses the start/stop indexes to only load the needed buttons and sprites
	 */
	private void ShowCurrentObjectPreviews()
	{
		int previewsToShow = catalogStop % 6;
		toggleCatButtons(previewsToShow);

		for (int i = 0, showing = catalogStart; showing < catalogStop; i++, showing++)
		{
			previews[i].sprite = currentCatSprites[i];
		}
	}

	/*!
	 * \brief shows/hides the catalog buttons depending on available objects
	 * \param previewsToShow number of buttons to show/hide
	 * \details This function uses the switch statement on previews to show to activate/deactive the preview/back image gameobjects from view
	 */
	private void toggleCatButtons(int previewsToShow)
	{
		switch (previewsToShow)
		{
			case -1:
				backs[0].gameObject.SetActive(false);
				backs[1].gameObject.SetActive(false);
				backs[2].gameObject.SetActive(false);
				backs[3].gameObject.SetActive(false);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[1].gameObject.SetActive(false);
				previews[2].gameObject.SetActive(false);
				previews[3].gameObject.SetActive(false);
				previews[4].gameObject.SetActive(false);
				previews[5].gameObject.SetActive(false);
				break;

			case 1:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(false);
				backs[2].gameObject.SetActive(false);
				backs[3].gameObject.SetActive(false);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(false);
				previews[2].gameObject.SetActive(false);
				previews[3].gameObject.SetActive(false);
				previews[4].gameObject.SetActive(false);
				previews[5].gameObject.SetActive(false);
				break;

			case 2:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(true);
				backs[2].gameObject.SetActive(false);
				backs[3].gameObject.SetActive(false);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(true);
				previews[2].gameObject.SetActive(false);
				previews[3].gameObject.SetActive(false);
				previews[4].gameObject.SetActive(false);
				previews[5].gameObject.SetActive(false);
				break;

			case 3:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(true);
				backs[2].gameObject.SetActive(true);
				backs[3].gameObject.SetActive(false);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(true);
				previews[2].gameObject.SetActive(true);
				previews[3].gameObject.SetActive(false);
				previews[4].gameObject.SetActive(false);
				previews[5].gameObject.SetActive(false);
				break;

			case 4:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(true);
				backs[2].gameObject.SetActive(true);
				backs[3].gameObject.SetActive(true);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(true);
				previews[2].gameObject.SetActive(true);
				previews[3].gameObject.SetActive(true);
				previews[4].gameObject.SetActive(false);
				previews[5].gameObject.SetActive(false);
				break;

			case 5:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(true);
				backs[2].gameObject.SetActive(true);
				backs[3].gameObject.SetActive(true);
				backs[4].gameObject.SetActive(true);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(true);
				previews[2].gameObject.SetActive(true);
				previews[3].gameObject.SetActive(true);
				previews[4].gameObject.SetActive(true);
				previews[5].gameObject.SetActive(false);
				break;

			case 0:
				backs[0].gameObject.SetActive(true);
				backs[1].gameObject.SetActive(true);
				backs[2].gameObject.SetActive(true);
				backs[3].gameObject.SetActive(true);
				backs[4].gameObject.SetActive(true);
				backs[5].gameObject.SetActive(true);
				previews[0].gameObject.SetActive(true);
				previews[1].gameObject.SetActive(true);
				previews[2].gameObject.SetActive(true);
				previews[3].gameObject.SetActive(true);
				previews[4].gameObject.SetActive(true);
				previews[5].gameObject.SetActive(true);
				break;

			default:
				backs[0].gameObject.SetActive(false);
				backs[1].gameObject.SetActive(false);
				backs[2].gameObject.SetActive(false);
				backs[3].gameObject.SetActive(false);
				backs[4].gameObject.SetActive(false);
				backs[5].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				previews[0].gameObject.SetActive(false);
				break;
		}
	}



	/*!
	 * \brief Hides the catalog from view, resets the filter flag and active flag
	 * \details This function deactivates the catalog preview gameObject and hides it 100 units below the scene. isActive/Filter flags reset. start/stop indexes reset to ensure the prefabs load from index 0.
	 */
	public void catOff()
	{
		Debug.Log("Catalog off");
		catalogCanvas.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
		catalogCanvas.gameObject.SetActive(false);

		catalogStart = 0;
		if (catalogSize > 6)
			catalogStop = 6;
		else
			catalogStop = catalogSize;

		//ShowObjectPreviews();
		filterActive = false;

		isActive = false;
		raycastHitOtherObjects();

	}

	/*!
	 * \brief Shows the catalog, resets filter codes, title text, and active flags
	 * \details This function activates the catalog preview gameObject and moves it in front of the user. isActive flag set. Prefab sprites are loaded from memory.
	 */
	public void catOn()
	{
		StartCoroutine(LoadObjectPreviewsCo());

		filtRoomCode = ObjectCategory.ROOMCODE.ALL;
		filtTypeCode = ObjectCategory.OBJECTTYPE.ALL;
		titleText.text = "Catalog > Room: " + filtRoomCode + "	Category: " + filtTypeCode;

		Debug.Log("Catalog on");
		catalogCanvas.gameObject.SetActive(true);
		catalogCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;

		isActive = true;

		raycastIgnoreOtherObjects();
	}

	/*!
	 * \brief Scrolls UI list to show next available objects (stops if none available)
	 * \details This function scrolls to the next available prefabs, by updating start/stop indexes, and loads their preview sprites into view from memory.
	 */
	public void scrollForward()
	{
		// check meed to scroll
		if (!isActive)
		{
			Debug.LogWarning("Catalog off. Cannot Scroll");
			return;
		}
		else if (filterActive)
		{
			filtScrollForward();
			return;
		}
		else if (catalogStop == catalogSize)
		{

			Debug.LogWarning("End of catalog reached");
			return;
		}

		Debug.Log("Scrolling forward");

		// remove buttons from view
		toggleCatButtons(-1);


		if (catalogStop + 6 < catalogSize)
		{
			catalogStart = catalogStop;
			catalogStop += 6;
		}
		else
		{
			catalogStart = catalogStop;
			catalogStop = catalogSize;
		}

		StopAllCoroutines();
		StartCoroutine(LoadObjectPreviewsCo());
		//ShowObjectPreviews();
	}

	/*!
	 * \brief Scrolls UI list to show previous objects (stops if at the start of list)
	 * \details This function scrolls to the previous available prefabs, by updating start/stop indexes, and loads their preview sprites into view from memory.
	 */
	public void scrollBackward()
	{
		// check meed to scroll
		if (!isActive)
		{
			Debug.LogWarning("Catalog off. Cannot Scroll");
			return;
		}
		else if (filterActive)
		{
			filtScrollBackward();
			return;
		}
		else if (catalogStart == 0)
		{
			Debug.LogWarning("Start of catalog reached");
			return;
		}

		Debug.Log("Scrolling back");

		// remove buttons from view
		toggleCatButtons(-1);

		if (catalogStart - 6 < 0)
		{
			catalogStop = catalogStart;
			catalogStart = 0;
		}
		else
		{
			catalogStop = catalogStart;
			catalogStart = catalogStart - 6;
		}

		StopAllCoroutines();
		StartCoroutine(LoadObjectPreviewsCo());
		//ShowObjectPreviews();
	}




	/*!
	 * \brief Spawns objects by button ID (object spawned corresponds to object in the button's preview)
	 * \param buttonID The ID of the selected button
	 * \param location Specifies where to spawn the object
	 * \details This function spawns the object that matches the preview selected in the GUI button.
	 */
	public void spawnByCatalogButton(int buttonID, Vector3 location)
	{
		if (buttonID < 1 || buttonID > 6)
		{
			Debug.LogWarning("Button index out of range");
			return;
		}

		GameObject toSpawn;
		int index;
		if (!filterActive)
		{
			index = (buttonID - 1) + catalogStart;

			toSpawn = Resources.Load<GameObject>("Prefabs/" + catalogNames[index].Substring(25, catalogNames[index].Length - 7 - 25));
			Debug.Log("Spawning: " + toSpawn.name);
			GameObject furniture = Instantiate(toSpawn, location, toSpawn.transform.rotation);
			furniture.gameObject.layer = 2;
			furniture.GetComponent<Furniture>().isMove = true;

			//Instantiate(catalog[index], location, catalog[index].transform.rotation);
		}
		else
		{
			index = (buttonID - 1) + filtCatalogStart;
			toSpawn = Resources.Load<GameObject>("Prefabs/" + catalogNames[indexInFilteredCatalog[index]].Substring(25, catalogNames[indexInFilteredCatalog[index]].Length - 7 - 25));
			Debug.Log("Spawning: " + toSpawn.name);
			GameObject furniture = Instantiate(toSpawn, location, toSpawn.transform.rotation);
			furniture.gameObject.layer = 2;
			furniture.GetComponent<Furniture>().isMove = true;
			//Instantiate(catalog[indexInFilteredCatalog[index]], location, catalog[index].transform.rotation);
		}

	}

	/*!
	 * \brief Getter/setter for FiltRoomCode
	 * \details This function updates filter values and starts a search for objects that match the current filters
	 */
	public ObjectCategory.ROOMCODE FiltRoomCode
	{
		get
		{
			return filtRoomCode;
		}

		set
		{
			filtRoomCode = value;
			//print("Room Filter: " + filtRoomCode);
			getFilteredObjects();
			//print("Room Filter: " + filtRoomCode);
			titleText.text = "Catalog > Room: " + filtRoomCode + "	Category: " + filtTypeCode;
		}
	}

	/*!
	 * \brief Getter/setter for FiltTypeCode
	 * \details This function updates filter values and starts a search for objects that match the current filters
	 */
	public ObjectCategory.OBJECTTYPE FiltTypeCode
	{
		get
		{
			return filtTypeCode;
		}

		set
		{
			filtTypeCode = value;
			//print("Type Filter: " + filtTypeCode);
			getFilteredObjects();
			titleText.text = "Catalog > Room: " + filtRoomCode + "	Category: " + filtTypeCode;
		}
	}

	/*!
	 * \brief Retrieves the index of all objects that match current filters and stores them in "indexInFilteredCatalog[]"
	 * \details This function searches for objects, sets the filtered catalog size, start/stop indexes, and filter flag
	 */
	private void getFilteredObjects()
	{
		filteredCatalogSize = 0;
		filtCatalogStart = 0;
		filtCatalogStop = 0;

		if (filtRoomCode == ObjectCategory.ROOMCODE.ALL && filtTypeCode == ObjectCategory.OBJECTTYPE.ALL)
		{
			filterActive = false;
			catalogStart = 0;
			if (catalogSize > 6)
				catalogStop = 6;
			else
				catalogStop = catalogSize;

			//ShowObjectPreviews();
			StopAllCoroutines();
			StartCoroutine(LoadObjectPreviewsCo());
			return;
		}
		else if (!(filtRoomCode == ObjectCategory.ROOMCODE.ALL) && !(filtTypeCode == ObjectCategory.OBJECTTYPE.ALL))
		{
			for (int i = 0; i < catalogSize; i++)
			{
				if (categories[i].roomType == filtRoomCode && (categories[i].objectType == filtTypeCode))
				{
					indexInFilteredCatalog[filteredCatalogSize] = i;
					filteredCatalogSize++;
				}
			}
		}
		else if (!(filtRoomCode == ObjectCategory.ROOMCODE.ALL) && (filtTypeCode == ObjectCategory.OBJECTTYPE.ALL))
		{
			for (int i = 0; i < catalogSize; i++)
			{
				if ((categories[i].roomType == filtRoomCode))
				{
					indexInFilteredCatalog[filteredCatalogSize] = i;
					filteredCatalogSize++;
				}
			}
		}
		else
		{
			for (int i = 0; i < catalogSize; i++)
			{
				if ((categories[i].objectType == filtTypeCode))
				{
					indexInFilteredCatalog[filteredCatalogSize] = i;
					filteredCatalogSize++;
				}
			}
		}

		filterActive = true;

		filtCatalogStart = 0;
		if (filteredCatalogSize > 6)
			filtCatalogStop = 6;
		else
			filtCatalogStop = filteredCatalogSize;

		//ShowFilteredPreviews();
		StopAllCoroutines();
		StartCoroutine(LoadFilteredObjectPreviewsCo());
	}

	/*!
	 * \brief [Filter] Scrolls UI list to show next available objects (stops if none available)
	 * \details This function scrolls to the next available prefabs, by updating start/stop indexes, and loads their preview sprites into view from memory.
	 */
	public void filtScrollForward()
	{
		// check meed to scroll
		if (!isActive)
		{
			Debug.LogWarning("[Filt] Catalog off. Cannot Scroll");
			return;
		}
		else if (filtCatalogStop == filteredCatalogSize)
		{
			Debug.LogWarning("[Filt] End of catalog reached");
			return;
		}

		Debug.Log("[Filt] Scrolling forward");
		print("There are " + filteredCatalogSize +" objects in this filter");

		// remove buttons from view
		toggleCatButtons(-1);


		if (filtCatalogStop + 6 < filteredCatalogSize)
		{
			filtCatalogStart = filtCatalogStop;
			filtCatalogStop += 6;
		}
		else
		{
			filtCatalogStart = filtCatalogStop;
			filtCatalogStop = filteredCatalogSize;
		}

		//ShowFilteredPreviews();
		StopAllCoroutines();
		StartCoroutine(LoadFilteredObjectPreviewsCo());
	}

	/*!
	 * \brief [Filter] Scrolls UI list to show previous available objects (stops if none available)
	 * \details This function scrolls to the previous available prefabs by updating start/stop indexes, and loads their preview sprites into view from memory.
	 */
	public void filtScrollBackward()
	{
		// check meed to scroll
		if (!isActive)
		{
			Debug.LogWarning("[Filt] Catalog off. Cannot Scroll");
			return;
		}
		else if (filtCatalogStart == 0)
		{
			Debug.LogWarning("[Filt] Start of catalog reached");
			return;
		}

		Debug.Log("[Filt] Scrolling back");

		// remove buttons from view
		toggleCatButtons(-1);

		if (filtCatalogStart - 6 < 0)
		{
			filtCatalogStop = filtCatalogStart;
			filtCatalogStart = 0;
		}
		else
		{

			filtCatalogStop = filtCatalogStart;
			filtCatalogStart = filtCatalogStart - 6;
		}

		//ShowFilteredPreviews();
		StopAllCoroutines();
		StartCoroutine(LoadFilteredObjectPreviewsCo());
	}

	/*!
	 * \brief Loads the gameObject from memory that matches the given ID
	 * \param id ID of the object to spawn.
	 * \details This function searches the categories array for an object that matches the given ID. Loads from memory when a match is made, null if does not exist.
	 */
	public GameObject getObjectByID(int id)
	{
		int i;
		for (i = 0; i < catalogSize; i++)
		{
			if (categories[i].objectID == id)
				break;
		}

		return Resources.Load<GameObject>("Prefabs/" + catalogNames[i].Substring(25, catalogNames[i].Length - 7 - 25));
	}

	/*!
	 * \brief toggles the catalog on/off from view
	 * \param sender Object that made the call to this function
	 * \param e Arguments attached to this event.
	 */
	public virtual void displayCatalog(object sender, ClickedEventArgs e)
	{
		if (isActive)
		{
			catOff();
		}

		else
		{
			catOn();
		}


	}


	/*!
	 * \brief Move all GameObjects to IgnoreRaycast layer
	 * \details This function removes the collision between objects when catalog GUI is active
	 */
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
			if (parentObject != catalogCanvas.gameObject)
			{
				gObject.layer = 2;
			}
		}
	}

	/*!
	 * \brief Move all GameObjects back to default layer
	 * \details This function restores the collision between objects when catalog GUI is deactivated
	 */
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
			if (parentObject != catalogCanvas)
			{
				if (parentObject.GetComponent<Furniture>() != null && parentObject.GetComponent<Furniture>().isMove)
				{
					gObject.layer = 2;
				}
				else
					gObject.layer = 0;
			}
		}
	}
	
	/*
	GameObject getGameObjectAtIndex(int indexInCatalog) {
		if(indexInCatalog >= catalogSize) {
			Debug.LogWarning("Index is larger than catalog size. Returning null");
			return null;
		}
		return catalog[indexInCatalog];
	}

	void spawnRandom (Vector3 location) {
		int index = Random.Range(0, catalogSize);
		Instantiate(catalog[index], location, catalog[index].transform.rotation);
	}
	

	void spawnObjectAtIndex (int indexInCatalog, Vector3 location) {
		if (indexInCatalog < 1 || indexInCatalog >= catalogSize) {
			Debug.LogWarning("Button index out of range");
			return;
		}

		Instantiate(catalog[indexInCatalog], location, catalog[indexInCatalog].transform.rotation);
	}

	void spawnObjectByID (int objectID, Vector3 location) {
		for(int i = 0; i < catalogSize; i++) {
			if(catalog[i].GetComponent<ObjectCategory>().objectID == objectID) {
				Instantiate(catalog[i], location, catalog[i].transform.rotation);
				return;
			}
		}
		Debug.LogWarning("Object ID not found");
	}

	*/
}
