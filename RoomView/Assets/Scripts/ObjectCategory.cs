/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Object Categories
 */ 
using UnityEngine;
using System.Collections;

public class ObjectCategory : MonoBehaviour {

    public enum ROOMCODE
    {
        KITCHEN, BATHROOM, BEDROOM, LIVING, DINING, REC, OUTDOORS, NONE, ALL
    }																				//!< defines values to categorize objects by room
	
    public enum OBJECTTYPE
    {
        APPLIANCE, COMFORT, DECORATION, ELECTRONIC, KIDS, LIGHTING, PLUMBING, SURFACES, MISC, NONE, ALL
    }																				//!< defines values to categorize objects by function
    
	
	
    public ROOMCODE roomType = ROOMCODE.KITCHEN;			//!< this object's room type
	public OBJECTTYPE objectType = OBJECTTYPE.APPLIANCE;	//!< this object's function type
	public int objectID;									//!< this object's ID

	
	/*!
	 * \brief [Unused]Returns this object's room type
	 */
	public ROOMCODE getRoomType() { return roomType; }	

}
