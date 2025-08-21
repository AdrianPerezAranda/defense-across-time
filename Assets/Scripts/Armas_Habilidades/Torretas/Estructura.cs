using Unity.VisualScripting;
using UnityEngine;

public class Estructura : MonoBehaviour
{
    [SerializeField] public int idEstructura;
    private void Awake()
    {
        SaveableObjectsController.objectsToSave.Add(GetComponent<Estructura>());
    }

    private void OnDestroy()
    {
        SaveableObjectsController.objectsToSave.Remove(GetComponent<Estructura>());
    }
}
