using UnityEngine;
using UnityEngine.Rendering;

public class ProyectilCatapulta : MonoBehaviour
{

    public float velocidad = 20.0f;
    public float daño = 10.0f;
    public float tiempoDeVida = 3.0f;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida); // Destruye el objeto 3sg despues de su instacia
    }

    void Update()
    
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Enemy enemigo = collision.gameObject.GetComponent<Enemy>();
            if (enemigo != null)
            {
                enemigo.ReciveDamage(daño);
            }
            Destroy(gameObject); // Destruir el proyectil tras impactar con el enemigo
        }
    }
}
