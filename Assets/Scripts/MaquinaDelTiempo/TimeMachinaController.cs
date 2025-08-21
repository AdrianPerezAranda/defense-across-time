using UnityEngine;
using UnityEngine.Rendering;

public class TimeMachinaController : MonoBehaviour
{
    [SerializeField] private GameObject m_ParticleSystem;

    private float delaySound, realDelay = 5;

    private void Start()
    {

        delaySound = realDelay = 5;

    }

    private void OnEnable()
    {
        GameController.Instance.timeMachine = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            GameObject particles = Instantiate(m_ParticleSystem, transform.position, Quaternion.identity);

            // Opcionalmente, puedes hacer que sea hijo del objeto actual
            particles.transform.SetParent(transform);

            // Opcionalmente, destruir el sistema de partículas después de que termine
            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
    }

    private void Update()
    {
        if (delaySound > 0)
        {
            delaySound -= Time.deltaTime;
        }

        
        if (delaySound <= 0)
        {
            MusicController.instance.PlayTimeMachine(transform);

            delaySound = realDelay;
        }
    }
}
