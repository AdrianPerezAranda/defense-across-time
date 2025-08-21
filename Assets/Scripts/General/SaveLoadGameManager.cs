using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadGameManager : MonoBehaviour
{
    int diaToSave;
    const string SUB_PATH = "/DaySaved";

    public void SaveDia()
    {
        diaToSave = EnemiesController.instance.idxDiaActual;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SUB_PATH;

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, diaToSave);
        stream.Close();
    }

    public int LoadDia()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SUB_PATH;
        int dayLoaded = 0;

        if(File.Exists(path))
        {
            FileStream countStream = new FileStream(path, FileMode.Open);
            dayLoaded = (int)formatter.Deserialize(countStream);
            countStream.Close();
            File.Delete(path);
        }
        else
        {
            Debug.LogError("File not found in " + path);
        }

        return dayLoaded;
    }

    public void DeleteSavedDay()
    {
        string path = Application.persistentDataPath + SUB_PATH;

        if (File.Exists(path)) 
        { 
            File.Delete(path);
        }
    }

}
