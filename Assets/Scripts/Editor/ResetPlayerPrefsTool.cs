using System.IO;
using UnityEditor;
using UnityEngine;

public class ResetPlayerPrefsTool : MonoBehaviour
{
    [MenuItem("Tools/ResetPlayerPrefs")]
    static void ResetPlayerPrefs()
    {
        print("PlayerPrefs Eliminados");
        PlayerPrefs.DeleteKey("Eras");
        PlayerPrefs.DeleteKey("TutorialCompletado");
        if (File.Exists(Application.persistentDataPath + "/DaySaved"));
        {
            File.Delete(Application.persistentDataPath + "/DaySaved");
        }
    }
}
