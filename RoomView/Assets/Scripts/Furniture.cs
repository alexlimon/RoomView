/*!
 * \author
 * \version 1.0
 * \date 4-15-2017
 *
 * \mainpage Logic behind the manipulation (rotate, clone, move, delete) of a furniture object
 * \bug No known bugs
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour {


    public SteamVR_TrackedObject controller; //!< pointer to the controller
    public GameObject contextMenuPrefab; //!< prefab of object menu object
    public static bool isFurnitureSelected = false; //!< flag to show if furniture piece is selected
    public bool isSelected = false; //!< flag to show if furniture piece has been selected
    public bool isRotate = false; //!< flag to show if furniture piece is being rotated
    public bool isMove = false; //!< flag to show if furniture piece is being moved
    public bool isClone = false; //!< flag to show if furniture piece was cloned
    //public bool fromCatalog = false; //!< flag to show if furniture piece was directly taken from catalog
    //public bool needsPlacement = false; //!< flag to show if furniture piece needs to placed within space
    //public bool placing = false; //!< flag to show if furniture piece is being placed


    //[DontSaveMember] private Material highlightMaterial;

    //[DontSaveMember] private Material[] materialArray;

    //[DontSaveMember] private Material[] materialArrayWithHighlight;

    private Vector3 offset; //!< furniture piece offset from hit point
    private SteamVR_TrackedController controllerInput; //!< pointer to the controller's input (buttons)
    private SteamVR_LaserPointer pointer; //!< pointer to the laser pointer beaming off controller
    public bool isHover = false; //!< flag to show if furniture piece needs to placed within space
    public bool isMoving = false; //!< flag to show if furniture piece needs to placed within space
    private Quaternion targetRotation = Quaternion.identity; //!< used to define amount of rotation a furniture peice is rotated by
    private Vector3 rotationDifference; //!< flag to show if furniture piece needs to placed within space
    private float padX; //!< used to store rotation (left and right d pad click) about the y axis of a furniture peice,
    private int id; //!< unique id of a furniture peice

	//[DontSaveMember]
	//Material thisShader;


  /*!
   * \brief Runs upon start and initializes variables
   * \details This function initializes the controller listeners and SteamVr laser pointer
   */
    void Awake() {

        //Can't be completed on furniture in the room at start


        if (controller != null)
        {
            try
            {
                pointer = controller.GetComponent<SteamVR_LaserPointer>();
                controllerInput = controller.GetComponent<SteamVR_TrackedController>();
            }
            catch
            {
                Debug.LogError(this.name + ": Could not find LaserPointer");
            }
        }
    }

    /*!
     * \brief Check to see whether a furniture object has been clone or from catalog menu
     * \details loads menu prefab, allows object
     */
    void Start()
    {
        contextMenuPrefab = Resources.Load("UI Prefabs/ContextMenu") as GameObject;
        id = GetComponent<ObjectCategory>().objectID;


        List<Material> materials = gameObject.GetComponent<MeshRenderer>().materials.Cast<Material>().ToList();
        List<Material> newMaterials = gameObject.GetComponent<MeshRenderer>().materials.Cast<Material>().ToList();
        if ( !isClone ) //If it is a clone, material arrays are being set be equal to the cloned object to prevent object from always being highlighted
        {
            newMaterials.Add(highlightMaterial);
        }
        else
        {
            materials.RemoveAt(materials.Count - 1);
            pointer.PointerIn += OnHover;
            pointer.PointerOut += OffHover;
            controllerInput.PadClicked += OnPadClicked;
        }

        materialArrayWithHighlight = newMaterials.ToArray();
        materialArray = materials.ToArray();

        if (isClone)
        {
            pointer.PointerIn += OnHover;
            pointer.PointerOut += OffHover;
            controllerInput.PadClicked += OnPadClicked;
			//isClone = false;
        }
		GameObject controllerObject = GameObject.FindGameObjectWithTag("Right Controller");
        if (controller == null)
            controller = controllerObject.GetComponent<SteamVR_TrackedObject>();

        if(controllerInput == null)
        {
            controllerInput = controllerObject.GetComponent<SteamVR_TrackedController>();
            controllerInput.PadClicked += OnPadClicked;
        }

        if(pointer == null)
        {
            pointer = controllerObject.GetComponent<SteamVR_LaserPointer>();
            pointer.PointerIn += OnHover;
            pointer.PointerOut += OffHover;
        }
        ObjectIdentifier objectIdentifier = gameObject.GetComponent<ObjectIdentifier>();
        objectIdentifier.id = gameObject.GetComponent<ObjectCategory>().objectID.ToString();
        objectIdentifier.componentSaveMode = ObjectIdentifier.ComponentSaveMode.All;

        try {
			GetComponent<MeshRenderer>().sharedMaterial = new Material(GetComponent<MeshRenderer>().sharedMaterial);
		}
		catch {
			MeshRenderer[] renders = GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer renderer in renders) {
				renderer.sharedMaterial = new Material(renderer.sharedMaterial);
			}
		}

		UnityEditor.PrefabUtility.DisconnectPrefabInstance(this);
    }

    /*!
     * \brief Manages how a furniture moves about based on section on context menu
     * \details allows furniture to be rotated and moved about a virtual space and ensures an object can be moved properly
     */
    private void Update()
    {

        if (isRotate)
        {

            float rotationAngle = 60f * Time.deltaTime;
            if (padX>0)
            {
                Quaternion targetQuat = Quaternion.AngleAxis(rotationAngle, Vector3.up) * transform.rotation;
                transform.rotation = targetQuat;
                //Debug.Log("Rotate item right");
            }
            else
            {
                Quaternion targetQuat = Quaternion.AngleAxis(-rotationAngle, Vector3.up) * transform.rotation;
                transform.rotation = targetQuat;
                //Debug.Log("Rotate item left");
            }
        }

        if (isMove & !isMoving /*& !fromCatalog*/)
        {
            isMoving = true;
            controllerInput.TriggerClicked -= OnSelectButton;
            controllerInput.TriggerClicked += CompleteMove;

            CatalogManager manager = GameObject.Find("PrefabCatalog").GetComponent<CatalogManager>();
            offset = manager.getObjectByID(id).transform.position;
        }

        else if (isMove)//Conext menu button sets isMove
        {
            Ray raycast = new Ray(controller.transform.position, controller.transform.forward); //
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit);
            transform.position = hit.point + offset;
        }


        if (isSelected || isHover)//If hover or selected highlight
        {
            //gameObject.GetComponent<MeshRenderer>().materials = materialArrayWithHighlight;
			try {
				GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
			}
			catch {
				MeshRenderer[] renders = GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer  renderer in renders) {
					renderer.sharedMaterial.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
				}
			}
		}

        else {
			try
			{
				GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", new Color());
			}
			catch
			{
				MeshRenderer[] renders = GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer renderer in renders)
				{
					renderer.sharedMaterial.SetColor("_EmissionColor", new Color());
				}
			}
		}
    }

    /*!
     * \brief Checks to see if furniture object has been placed
     * \details ensures certain flags on furniture object are set correctly after object is moved
     */
    void CompleteMove(object sender, ClickedEventArgs e)
    {
        Debug.Log("Furniture.CompleteMove: BEGIN");
        isSelected = false;
        isHover = false;
        isFurnitureSelected = false;
        isMove = false;
        gameObject.layer = 0;
        controllerInput.TriggerClicked -= CompleteMove;
        isMoving = false;
        Debug.Log("Furniture.CompleteMove: END");

    }

    /*!
     * \brief Checks to see if furniture object is hovered over and thus able to be selected
     */
    void OnHover(object sender, PointerEventArgs e)
    {
        if((e.target == GetComponent<Collider>().transform) && !isSelected && !isFurnitureSelected)
        {
            controllerInput.TriggerClicked += OnSelectButton;
            isHover = true;
        }
    }

    /*!
     * \brief Checks to see if furniture object is not hovered over and thus not able to be selected
     */
    void OffHover(object sender, PointerEventArgs e)
    {
        if(e.target == GetComponent<Collider>().transform)
        {
            isHover = false;
            controllerInput.TriggerClicked -= OnSelectButton;
        }
    }

    /*!
     * \brief Instantiates object menu to display options such as move, clone, rotate
     */
    public virtual void OnSelectButton(object sender, ClickedEventArgs e)
    {
        GameObject contextMenu = Instantiate(contextMenuPrefab)as GameObject;
        contextMenu.GetComponent<UI_Follower>().mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        contextMenu.GetComponent<UI_Follower>().setSnappedObject(gameObject);
        isSelected = true;
        isFurnitureSelected = true;

    }


/*!
 * \brief Check to see whether object is clone or from catalog menu
 * \details
 */
    private void OnPadClicked(object sender, ClickedEventArgs e)
    {
        padX = e.padX;
    }

    private void OnDestroy()
    {
        pointer.PointerIn -= OnHover;
        pointer.PointerOut -= OffHover;
        controllerInput.TriggerClicked -= OnSelectButton;
        controllerInput.PadClicked -= OnPadClicked;
    }


}
