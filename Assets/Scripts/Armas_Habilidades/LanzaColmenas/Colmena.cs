using UnityEngine;
using UnityEngine.Rendering;

public class Colmena : MonoBehaviour
{
    public float velocidad = 5f;
    public float frecuenciaZigzag = 10f; // Frecuencia del zigzag
    public float amplitudZigzag = 0.5f; // Amplitud del zigzag
    private Transform objetivo;

    public float tiempoVida = 5f; // Tiempo de vida de la colmena
    public float daño = 10f;


    void Start()
    {
        Destroy(gameObject, tiempoVida); // Destruir la colmena después de un tiempo
    }

    public void SetObjetivo(Transform nuevoObjetivo)
    {
        objetivo = nuevoObjetivo;
    }

    void Update()
    {
        if (objetivo == null)
        {
            Debug.LogWarning("El objetivo ha desaparecido. Destruyendo la colmena.");
            Destroy(gameObject); // Destruye a la colmena sin el el objetovo se destruye
            return;
        }

        // Movimiento de la colmena
        Vector3 direccion = (objetivo.position - transform.position).normalized;

        // Agrega un pequeño zigzag para simular las abejas
        float zigzag = Mathf.Sin(Time.time * frecuenciaZigzag) * amplitudZigzag;
        Vector3 offset = Vector3.right * zigzag;

        transform.position = Vector3.MoveTowards(transform.position, objetivo.position + offset, velocidad * Time.deltaTime);

    }
    // Destruit la colmena al colisionar con el enemigo
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Enemy enemigo = other.gameObject.GetComponent<Enemy>();
            if (enemigo != null)
            {
                enemigo.ReciveDamage(daño); // Causa 10 de daño
            }
            Destroy(gameObject);
        }
    }
}
