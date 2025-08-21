using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanelSelectorTrampas : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelSelectorTrampas;
    public GameObject panelJuego;

    [Header("Casillas específicas")]
    public GameObject[] casillasTrampas;
    public GameObject casillaArmaADistancia;
    public GameObject casillaArmaCuerpoACuerpo;
    private List<GameObject> trampasSeleccionadas = new List<GameObject>();

    
    public List<GameObject> prefabsTrampasSeleccionadas = new List<GameObject>();

    [Header("Referencia al sistema DragAndDrop")]
    public DragAndDrop dragAndDrop; 

    void Start()
    {
        int numeroCasillas = panelSelectorTrampas.transform.childCount;
        casillasTrampas = new GameObject[numeroCasillas];

        for (int i = 0; i < numeroCasillas; i++)
        {
            casillasTrampas[i] = panelSelectorTrampas.transform.GetChild(i).gameObject;
        }
    }

    public bool AsignarArmaCuerpoACuerpo(GameObject armaPrefab)
    {
        var habilidadUI = armaPrefab.GetComponent<HabilidadUI>();
        if (habilidadUI == null || habilidadUI.tipoHabilidad != ArbolDeHabilidades.TipoHabilidad.CuerpoACuerpo)
        {
            Debug.Log("Intentaste asignar algo que no es un arma cuerpo a cuerpo.");
            return false;
        }

        foreach (Transform child in casillaArmaCuerpoACuerpo.transform)
            Destroy(child.gameObject);

        GameObject nuevaArma = Instantiate(armaPrefab, casillaArmaCuerpoACuerpo.transform);
        nuevaArma.transform.localScale = Vector3.one * 1f;
        nuevaArma.transform.localPosition = Vector3.zero;

        var nuevaHabilidadUI = nuevaArma.GetComponent<HabilidadUI>();
        if (nuevaHabilidadUI != null)
        {
            nuevaHabilidadUI.panelSelectorTrampas = this;
            nuevaHabilidadUI.tipoHabilidad = ArbolDeHabilidades.TipoHabilidad.CuerpoACuerpo;
            nuevaHabilidadUI.esDelInventario = false;
        }

        // Sincroniza el panel

        SincronizarPanelJuego();

        Debug.Log($"Arma cuerpo a cuerpo {armaPrefab.name} asignada al panel.");
        return true;
    }

    public bool AsignarArmaADistancia(GameObject armaPrefab)
    {
        var habilidadUI = armaPrefab.GetComponent<HabilidadUI>();
        if (habilidadUI == null || habilidadUI.tipoHabilidad != ArbolDeHabilidades.TipoHabilidad.ADistancia)
        {
            Debug.Log("no es una arma a dustancia");
            return false;
        }

        foreach (Transform child in casillaArmaADistancia.transform)
            Destroy(child.gameObject);

        GameObject nuevaArma = Instantiate(armaPrefab, casillaArmaADistancia.transform);
        nuevaArma.transform.localScale = Vector3.one * 1f;
        nuevaArma.transform.localPosition = Vector3.zero;

        var nuevaHabilidadUI = nuevaArma.GetComponent<HabilidadUI>();
        if (nuevaHabilidadUI != null)
        {
            nuevaHabilidadUI.panelSelectorTrampas = this;
            nuevaHabilidadUI.tipoHabilidad = ArbolDeHabilidades.TipoHabilidad.ADistancia;
            nuevaHabilidadUI.esDelInventario = false;
        }

        // Sincroniza el panel

        SincronizarPanelJuego();

        Debug.Log($"Arma a distancia {armaPrefab.name} se asignada en el panel");
        return true;
    }


    public bool SeleccionarTrampa(GameObject trampaPrefab, GameObject iconoTrampa)
    {
        Debug.Log($" trampa {trampaPrefab.name}");

        if (trampasSeleccionadas.Count >= casillasTrampas.Length)
        {
            Debug.Log("ya tienen las 6 trampas");
            return false;
        }

        foreach (var trampa in trampasSeleccionadas)
        {
            if (trampa.name == trampaPrefab.name)
            {
                Debug.Log("La trampa ya está seleccionada.");
                return false;
            }
        }

        for (int i = 0; i < casillasTrampas.Length; i++)
        {
            // Verificar las casilla correpsonientes con segun el tipo de Habilidad
            if (casillasTrampas[i] == casillaArmaCuerpoACuerpo || casillasTrampas[i] == casillaArmaADistancia)
            {
                continue; // Continua si la casilla no correponde
            }

            if (casillasTrampas[i].transform.childCount == 0)
            {
                // instancia el icono de la trampa en la casilla correspondiente
                GameObject nuevoIcono = Instantiate(iconoTrampa, casillasTrampas[i].transform);
                nuevoIcono.transform.localScale = Vector3.one * 1f;
                nuevoIcono.transform.localPosition = Vector3.zero;

                trampasSeleccionadas.Add(nuevoIcono);

                // Guardar el prefab de la trampa en la lista de prefabs para DragAndDrop
                if (!prefabsTrampasSeleccionadas.Contains(trampaPrefab))
                {
                    prefabsTrampasSeleccionadas.Add(trampaPrefab);
                    Debug.Log($"{trampaPrefab.name} agregado a prefabsTrampasSeleccionadas.");
                }

                // Actualizar trampas en DragAndDrop
                if (dragAndDrop != null)
                {
                    dragAndDrop.ActualizarTrampasSeleccionadas(prefabsTrampasSeleccionadas);
                }

                // llamar al metodo que sincroniza el panel de juego
                SincronizarPanelJuego();

                Debug.Log($"trampa {trampaPrefab.name} seleccionada en el inventario.");
                return true;
            }
        }

    
        return false;
    }

    // Metodo para poder sincronizar el panel de seleccion al panel de juego

    public void SincronizarPanelJuego()
    {
        // limpia el contenido del panel Juego
        foreach (Transform child in panelJuego.transform)
        {
            Destroy(child.gameObject);
        }

        // Instancia los iconos de las trampas seleccionadas en el panel de juego
        foreach (var trampaIcono in trampasSeleccionadas)
        {
            GameObject nuevoIcono = Instantiate(trampaIcono, panelJuego.transform);
            nuevoIcono.transform.localScale = Vector3.one * 1f;
            nuevoIcono.transform.localPosition = Vector3.zero;
        }

        // Sincroniza el arma CaC
        if (casillaArmaCuerpoACuerpo.transform.childCount > 0)
        {
            GameObject iconoCuerpoACuerpo = casillaArmaCuerpoACuerpo.transform.GetChild(0).gameObject;
            GameObject nuevoIconoCuerpoACuerpo = Instantiate(iconoCuerpoACuerpo, panelJuego.transform);
            nuevoIconoCuerpoACuerpo.transform.localScale = Vector3.one * 1f;
            nuevoIconoCuerpoACuerpo.transform.localPosition = Vector3.zero;
        }

        // Sincroniza el icono del arma a distancia
        if (casillaArmaADistancia.transform.childCount > 0)
        {
            GameObject iconoADistancia = casillaArmaADistancia.transform.GetChild(0).gameObject;
            GameObject nuevoIconoADistancia = Instantiate(iconoADistancia, panelJuego.transform);
            nuevoIconoADistancia.transform.localScale = Vector3.one * 1f;
            nuevoIconoADistancia.transform.localPosition = Vector3.zero;
        }

        
    }

}