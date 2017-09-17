/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage The UI Follower
 */
using UnityEngine;
using System.Collections;

public class UI_Follower : MonoBehaviour {

	public GameObject mainCamera;			//!< camera gameobject (for use in tracking)
	public GameObject snappedObject;		//!< gameObject snapped to (for use in tracking)

	private Vector3 offset;					//!< used to set the position between snappedObject and mainCamera

	/*!
	 * \brief Set the context menu at 50% distance between the snapped object and the camera
	 * \details Runs at start and sets the offset position
	 */
	void Start () {

		// set menu midway between both objects (50% between)
		offset = Vector3.Lerp(mainCamera.transform.position, snappedObject.transform.position, 0.5f);
		transform.position = offset;
	}

	/*!
	 * \brief Set the context menu at 50% distance between the snapped object and the camera. Faces the UI toward the camera
	 * \details Runs at every frame
	 */
	void Update () {

		// set menu midway between both objects (50% between)
		offset = Vector3.Lerp(mainCamera.transform.position, snappedObject.transform.position, 0.5f);
		transform.position = offset;

		// set canvas to face camera
		transform.LookAt(mainCamera.transform);
	}

	/*!
	 * \brief Sets the snapped object to hover around
	 */
	public void setSnappedObject(GameObject newGO) {

		snappedObject = newGO;
		Debug.LogWarning("Object changed");
	}
}
