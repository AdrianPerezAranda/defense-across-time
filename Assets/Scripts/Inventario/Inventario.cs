using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    private int numeroCasillas;
    private GameObject[] casillas;
    public GameObject contenedorCasillas;
    public PanelSelectorTrampas panelSelectorTrampas;

   
    private List<GameObject> trampas = new List<GameObject>();

    void Start()
    {
        numeroCasillas = contenedorCasillas.transform.childCount;
        casillas = new GameObject[numeroCasillas];

        for (int i = 0; i < numeroCasillas; i++)
        {
            casillas[i] = contenedorCasillas.transform.GetChild(i).gameObject;
        }
    }

    public GameObject AgregarHabilidadAlInventario(GameObject iconoHabilidad, ArbolDeHabilidades.TipoHabilidad tipoHabilidad)
    {
        for (int i = 0; i < numeroCasillas; i++)
        {
            if (casillas[i].transform.childCount == 0)
            {
                GameObject nuevaHabilidad = Instantiate(iconoHabilidad, casillas[i].transform);
                nuevaHabilidad.transform.localScale = Vector3.one * 2.0f;
                nuevaHabilidad.transform.localPosition = Vector3.zero;

                var rawImage = nuevaHabilidad.GetComponent<UnityEngine.UI.RawImage>();
                if (rawImage != null)
                {
                    rawImage.raycastTarget = true;
                }

               
                var habilidadUI = nuevaHabilidad.GetComponent<HabilidadUI>();
                if (habilidadUI == null)
                    habilidadUI = nuevaHabilidad.AddComponent<HabilidadUI>();
                habilidadUI.panelSelectorTrampas = panelSelectorTrampas;
                habilidadUI.tipoHabilidad = tipoHabilidad;
                habilidadUI.esDelInventario = true;

                Debug.Log("Habilidad añadida al inventario");
                return nuevaHabilidad;
            }
        }
        Debug.Log("No hay espacio en el inventario");
        return null;
    }




    public void SeleccionarHabilidadDesdeInventario(GameObject habilidad, ArbolDeHabilidades.TipoHabilidad tipoHabilidad)
    {
        switch (tipoHabilidad)
        {
            case ArbolDeHabilidades.TipoHabilidad.CuerpoACuerpo:
                if (!panelSelectorTrampas.AsignarArmaCuerpoACuerpo(habilidad))
                {
                    Debug.Log("No se pudo asignar el arma cuerpo a cuerpo al panel.");
                }
                break;

            case ArbolDeHabilidades.TipoHabilidad.ADistancia:
                if (!panelSelectorTrampas.AsignarArmaADistancia(habilidad))
                {
                    Debug.Log("No se pudo asignar el arma a distancia al panel.");
                }
                break;

            case ArbolDeHabilidades.TipoHabilidad.Trampa:
                
                var habilidadUI = habilidad.GetComponent<HabilidadUI>();
                if (habilidadUI != null && habilidadUI.prefabTrampa != null)
                {
                    if (!panelSelectorTrampas.SeleccionarTrampa(habilidadUI.prefabTrampa, habilidad))
                    {
                        Debug.Log("No se pudo asignar la trampa al panel.");
                    }
                }
                else
                {
                    Debug.LogError("El prefab de la trampa no está asignado o el componente HabilidadUI no existe.");
                }
                break;
        }
    }



    public void AgregarPrefabTrampa(GameObject prefabTrampa)
    {
        if (prefabTrampa == null)
        {
            Debug.LogError("El prefab de la trampa es nulo y no se puede agregar al inventario.");
            return;
        }

        trampas.Add(prefabTrampa);
    }
}