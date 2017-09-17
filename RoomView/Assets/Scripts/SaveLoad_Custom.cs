/*!
 * \author Luis Diaz Jr
 * \version 1.0
 * \date 4-17-2017
 *
 * \mainpage Save/Load controller
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad_Custom : MonoBehaviour
{
	
	/*!
	 * \brief Saves the current gamestate
	 * \param FileName Name of the file to save
	 * \details Saves the location of all furniture, their rotation, and prefab names to file (csv)
	 */
	public bool SaveData(string FileName)
	{
		StreamWriter writer;			
		GameObject[] allFurniture;		// holds a list of all spawned objects

		File.Delete(FileName);			// deletes the old file if it exists

		// try opening the file to save to
		try
		{
			writer = new StreamWriter(new FileStream(FileName, FileMode.OpenOrCreate));
		}
		catch
		{
			Debug.LogError("Cannot access File");
			return false;
		}

		// try writing to file
		try
		{
			allFurniture = GameObject.FindGameObjectsWithTag("Furniture");		// get all the furniture
			Debug.Log("There are this many objects: " + allFurniture.Length);	
			writer.Write(allFurniture.Length + "\n");							// write the number of furniture to file

			// write the name, prefabname, position and rotation to file (csv line)
			foreach (GameObject furniture in allFurniture)
			{	
				string toFile = furniture.name + "," +
				furniture.name.Replace("(Clone)" , "") + "," +
				furniture.transform.position.x.ToString("0.00") + "," +
				furniture.transform.position.y.ToString("0.00") + "," +
				furniture.transform.position.z.ToString("0.00") + "," +

				furniture.transform.rotation.eulerAngles.x.ToString("0.00") + "," +
				furniture.transform.rotation.eulerAngles.y.ToString("0.00") + "," +
				furniture.transform.rotation.eulerAngles.z.ToString("0.00") + "\n";

				writer.Write(toFile);
			}
		}
		catch
		{
			Debug.LogError("Cannot write to file");
			writer.Close();
			return false;
		}

		
		writer.Close(); // close the file
		return true;
	}

	/*!
	 * \brief Loads a file to the current scene
	 * \param FileName Name of the file to load from
	 * \details Loads the location of all furniture, their rotation, and spawns them to the scene
	 */
	public bool LoadData(string FileName)
	{

		GameObject[] allFurniture = GameObject.FindGameObjectsWithTag("Furniture");  // find all objects still in the scene

		// delete all objects in the scene
		if(allFurniture.Length > 0) {
			foreach(GameObject furn in allFurniture) {
				Destroy(furn);
			}
		}

		// try opening the file 
		StreamReader reader; 
		try
		{
			reader = new StreamReader(new FileStream(FileName, FileMode.Open));
			//new BinaryWriter(new FileStream(FileName, FileMode.OpenOrCreate));
		}
		catch
		{
			Debug.LogError("Cannot access File");
			return false;
		}

		// try reading from the file and spawn
		try
		{
			int size = int.Parse(reader.ReadLine());
			Debug.Log("There are this many objects to load: " + size);
			
			string temp;
			while ((temp = reader.ReadLine()) != null)
			{
				CreateObject(temp);
			}
		}
		catch
		{
			Debug.LogError("Cannot read from file");
			reader.Close();
			return false;
		}

		reader.Close(); // close the file
		return true;
	}

	/*!
	 * \brief Spawns an object using the string read from the file
	 * \param fromFile Contains the line read from the file
	 * \details Splits the string read in and constructs a new object with values in the string (location, name, rotation).
	 */
	void CreateObject(string fromFile) {
		string[] tokens = fromFile.Split(',');
		Debug.Log("string size: " + tokens.Length);

		GameObject newFurniture = Instantiate(Resources.Load<GameObject>("Prefabs/" + tokens[1]));
		newFurniture.name = tokens[0];
		newFurniture.transform.position = new Vector3(float.Parse(tokens[2]), float.Parse(tokens[3]), float.Parse(tokens[4]));
		newFurniture.transform.eulerAngles = new Vector3(float.Parse(tokens[5]), float.Parse(tokens[6]), float.Parse(tokens[7]));
	}
}