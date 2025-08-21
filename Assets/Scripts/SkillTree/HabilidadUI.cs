using UnityEngine;
using UnityEngine.EventSystems;

public class HabilidadUI : MonoBehaviour, IPointerClickHandler
{
    public PanelSelectorTrampas panelSelectorTrampas;
    public ArbolDeHabilidades.TipoHabilidad tipoHabilidad;
    public bool esDelInventario = true;

    public GameObject prefabTrampa;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (panelSelectorTrampas != null && esDelInventario)
        {
            switch (tipoHabilidad)
            {
                case ArbolDeHabilidades.TipoHabilidad.Trampa:
                    if (prefabTrampa != null)
                    {
                        if (!panelSelectorTrampas.SeleccionarTrampa(prefabTrampa, gameObject))
                        {
                            Debug.Log("No se pudo asignar la trampa al panel.");
                        }
                    }
                    else
                    {
                        Debug.LogError("No tiene prefab");
                    }
                    break;

                case ArbolDeHabilidades.TipoHabilidad.CuerpoACuerpo:
                    if (!panelSelectorTrampas.AsignarArmaCuerpoACuerpo(gameObject))
                    {
                        
                    }
                    break;

                case ArbolDeHabilidades.TipoHabilidad.ADistancia:
                    if (!panelSelectorTrampas.AsignarArmaADistancia(gameObject))
                    {
                        
                    }
                    break;

                default:
                    Debug.Log("????");
                    break;
            }
        }
    }
}
