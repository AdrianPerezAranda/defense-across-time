using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

public class SaveableObjectsController : MonoBehaviour
{
    [SerializeField] GameObject[] estructures;

    public static List<Estructura> objectsToSave = new List<Estructura>();

    const string SUB_PATH = "/savedData";
    const string SUB_COUNT_PATH = "/savedDataCount";

    
    public void SaveGame()
    {   
        if (objectsToSave.Count > 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SUB_PATH;
            string countPath = Application.persistentDataPath + SUB_COUNT_PATH;

            //Stream para guardar el numero de torretas que se guardan
            FileStream countStream = new FileStream(countPath, FileMode.Create);
            formatter.Serialize(countStream, objectsToSave.Count);
            countStream.Close();

            for (int i = 0; i < objectsToSave.Count; i++)
            {
                FileStream stream = new FileStream(path + i, FileMode.Create);
                SaveableObject obj = new SaveableObject(objectsToSave[i]);

                formatter.Serialize(stream, obj);
                stream.Close();
            }
            //Limpiamos la lista porque ya se han guardado
            objectsToSave.Clear();
        }
    }

    public void LoadGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SUB_PATH;
        string countPath = Application.persistentDataPath + SUB_COUNT_PATH;
        int fishCount = 0;

        //Lee el archivo de cuantos tipos de estrucutras se han guardado
        if(File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open);
            fishCount =(int) formatter.Deserialize(countStream);
            countStream.Close ();

            File.Delete(countPath);
        }
        else
        {
            Debug.LogError("File does not exist in " + countPath);
        }

        //Lee todos los archivos
        for(int i = 0; i < fishCount;i++)
        {
            if(File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                SaveableObject data = formatter.Deserialize(stream) as SaveableObject;
                stream.Close();

                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                Vector3 rotation = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);

                Instantiate(estructures[data.id], position, Quaternion.Euler(rotation));

                File.Delete(path + i);
            }
            else
            {
                Debug.LogError("File does not exist in " + path + i);
            }
        }
    }
}


