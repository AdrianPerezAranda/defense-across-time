using UnityEngine;

[System.Serializable]
public class SaveableObject
{
    public int id;
    public float[] position;
    public float[] rotation;

    public SaveableObject(Estructura go) 
    {
        id = go.idEstructura;
        Vector3 pos = go.transform.position;
        Vector3 rot = go.transform.rotation.eulerAngles;

        position = new float[] { pos.x, pos.y, pos.z };
        rotation = new float[] { rot.x, rot.y, rot.z };
    }

}
