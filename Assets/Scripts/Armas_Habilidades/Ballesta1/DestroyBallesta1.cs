using UnityEngine;

public class DestroyBallesta1 : MonoBehaviour
{
    public float daño = 10.0f;
    void Start()
    {
        Destroy(gameObject, 3.0f); // Destruir el proyectil después de 3 segundos
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
