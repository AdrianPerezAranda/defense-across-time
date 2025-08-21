using Unity.VisualScripting;
using UnityEngine;

public class Ballesta1Controller : MonoBehaviour, ActivarTrampa
{
    public GameObject proyectilBallestaPrefab;
    public Transform puntoDisparo;
    public float velocidadFlecha = 100.0f;
    public float intervaloDisparo = 3.0f; // Intervalo de disparo en segundos
    
    public Animator animator;
    private bool puedeDisparar = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();    
    }


    void Disparar()
    {
        if (puedeDisparar)
        {
            animator.SetTrigger("disparo");
        }
    }
    
    
    public void LanzarFlecha()
    {
        GameObject flecha = Instantiate(proyectilBallestaPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = flecha.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = puntoDisparo.forward * velocidadFlecha;
        }
    }
    public void Activar()
    {
        puedeDisparar = true;
        InvokeRepeating("Disparar", 0f, intervaloDisparo);
    }

}
