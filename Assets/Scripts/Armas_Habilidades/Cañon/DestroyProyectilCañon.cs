using UnityEngine;

public class DestroyProyectilCañon : MonoBehaviour
{
    public float daño = 10.0f;
    public float tiempoDeVida = 3.0f; // Tiempo en segundos antes de destruir el proyectil
    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Enemy enemigo = collision.gameObject.GetComponent<Enemy>();
            if (enemigo != null)
            {
                enemigo.ReciveDamage(daño); 
            }
            Destroy(gameObject); 
        }
    }
}
