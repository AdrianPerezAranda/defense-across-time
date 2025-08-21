using UnityEngine;

public class DisparoTorreta : MonoBehaviour, ActivarTrampa
{
    public Transform cabezaTorreta;
    public Transform puntoDisparo;
    public GameObject proyectilTorreta;
    public float rangoDeteccion = 50f;
    public float velocidadGiro = 5f;
    public float tiempoEntreDisparos = 1.0f;
    private Transform objetivo;
    private float tiempoDisparo;

    private bool puedeDisparar = false; // Variable para controlar el disparo cuando se instancie

    void Update()
    {

        if (!puedeDisparar) return ;
      
        BuscarObjetivo();

        if (objetivo != null)
        {
            RotarHaciaObjetivo();
            Disparar();
        }
    }

    void BuscarObjetivo()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");

        float menorDistancia = Mathf.Infinity;
        Transform enemigoMasCercano = null;

        foreach (GameObject enemigo in enemigos)
        {
            float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (distancia < menorDistancia && distancia <= rangoDeteccion)
            {
                menorDistancia = distancia;
                enemigoMasCercano = enemigo.transform;
            }
        }

        objetivo = enemigoMasCercano;
    }

    void RotarHaciaObjetivo()
    {
        // Rota la torreta hacia el objetivo en e`l eje Y
        Vector3 objetivoPosicion = new Vector3(objetivo.position.x, cabezaTorreta.position.y, objetivo.position.z);
        Vector3 direccion = objetivoPosicion - cabezaTorreta.position;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            cabezaTorreta.rotation = Quaternion.Lerp(cabezaTorreta.rotation, rotacionObjetivo, Time.deltaTime * velocidadGiro);
        }
    }

    void Disparar()
    {
        if (Time.time >= tiempoDisparo)
        {
            tiempoDisparo = Time.time + tiempoEntreDisparos;

            if (objetivo != null && proyectilTorreta != null)
            {
                GameObject proyectil = Instantiate(proyectilTorreta, puntoDisparo.position, puntoDisparo.rotation);
                Rigidbody rb = proyectil.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = puntoDisparo.forward * 20f;  // Velocidad del proyectil
                }
            }
        }
    }

    public void Activar()
    {
        puedeDisparar = true;
        Debug.Log("Torreta activada");
    }
}
