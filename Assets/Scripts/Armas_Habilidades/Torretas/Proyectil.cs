using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 20f;
    public float daño = 10f;
    public float tiempoDeVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida); // Destruir despu�s de cierto tiempo
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
            Destroy(gameObject); // Destruir el proyectil tras impactar
        }
    }
}
    