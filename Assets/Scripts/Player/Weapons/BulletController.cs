using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float velocity;
    public float timeLife;
    public float damageEnemy;
    
    private Vector3 m_Velocity;

    void Update()
    {
        // Velocidad de la bala
        m_Velocity = transform.forward * velocity;

        // Movimiento de la bala
        transform.position += m_Velocity * Time.deltaTime;

        // Resta del tiempo de vida - el tiempo que esta vivo
        timeLife -= Time.deltaTime;

        if(timeLife <= 0)
        {
            // Desaparicion de la bala al terminarse su tiempo de vida
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // Desaparicion de la bala al chocar con enemigo
            Destroy(gameObject);

            // Daño a enemigo
            collision.gameObject.GetComponent<Enemy>().ReciveDamage(damageEnemy);
        }
    }
}
