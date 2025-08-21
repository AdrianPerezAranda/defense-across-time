using UnityEngine;


public class Cañon3Controller : MonoBehaviour, ActivarTrampa
{
    public GameObject proyectilCañonPrefab;
    public Transform[] puntosDeDisparo;
    public float fuerzaDisparo = 10f;
    public float intervaloDisparo = 3f;

    private bool puedeDisparar = false; // variable para controlar el disparo cuando se intanci

    //Animator
    public Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    public void LanzarCañon()
    {



        if (MusicController.instance != null)
        {
            MusicController.instance.PlayHeadshot(transform);
            Debug.Log("Sonido de disparo desde el cañon");
        }

        if (puntosDeDisparo.Length == 0) return;

        // Selecciona el punto de disparo aleatorio
        Transform puntoDisparoAleatorio = puntosDeDisparo[Random.Range(0, puntosDeDisparo.Length)];

        //Instania el proyectil desde el punto de disparo aleatorio
        GameObject proyectilCañon = Instantiate(proyectilCañonPrefab, puntoDisparoAleatorio.position, puntoDisparoAleatorio.rotation);
        Rigidbody rb = proyectilCañon.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = puntoDisparoAleatorio.forward * fuerzaDisparo;
        }

    }

    public void Disparar()
    {
        if (animator != null && puedeDisparar) 
        { 
            animator.SetTrigger("disparo");
        
        }

    }

    public void Activar()
    {
        puedeDisparar = true; // Permitir disparar
        // Inicia el disparo
        InvokeRepeating("LanzarCañon", 1f, intervaloDisparo);
    }

}
