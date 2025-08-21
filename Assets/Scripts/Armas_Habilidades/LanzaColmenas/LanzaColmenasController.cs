using UnityEngine;

public class LanzaColmenasController : MonoBehaviour
{
    public GameObject panelPrefab; // Prefab del panel de colmena
  
    public Transform puntoLanzamiento; // Punto desde donde se lanza la colmena
    public float intervaloLanzamiento = 3.0f; // Tiempo entre lanzamientos
    public float rangoDeteccion = 50f; // Rngo mamaximo

    void Start()
    {
        Debug.Log("Posición del lanzador: " + transform.position);
        InvokeRepeating(nameof(LanzarColmena), 0f, intervaloLanzamiento);
    }

    public void LanzarColmena()
    {
        Debug.Log("LanzarColmena llamado");
        GameObject enemigo = BuscarEnemigoMasCercano();
        if (enemigo != null)
        {
            Debug.Log("Enemigo encontrado: " + enemigo.name);
            GameObject panel = Instantiate(panelPrefab, puntoLanzamiento.position, Quaternion.identity);
            Colmena panelScript = panel.GetComponent<Colmena>();
            if (panelScript != null)
            {
                panelScript.SetObjetivo(enemigo.transform);
                Debug.Log("Colmena lanzada hacia: " + enemigo.name);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ningún enemigo.");
        }
    }

    GameObject BuscarEnemigoMasCercano()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        Debug.Log("Cantidad de enemigos encontrados: " + enemigos.Length);
        GameObject masCercano = null;
        float distanciaMin = Mathf.Infinity;

        foreach (GameObject enemigo in enemigos)
        {
            float dist = Vector3.Distance(transform.position, enemigo.transform.position);
            Debug.Log("Distancia al enemigo " + enemigo.name + ": " + dist);
            if (dist < distanciaMin && dist <= rangoDeteccion) // Verifica si está dentro del rango
            {
                distanciaMin = dist;
                masCercano = enemigo;
            }
        }

        if (masCercano != null)
        {
            Debug.Log("Enemigo más cercano dentro del rango: " + masCercano.name);
        }
        else
        {
            Debug.LogWarning("No se encontró ningún enemigo dentro del rango.");
        }

        return masCercano;
    }
}
