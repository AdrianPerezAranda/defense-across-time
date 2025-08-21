using UnityEngine;
using System.Collections;

public class LanzaArietesController : MonoBehaviour, ActivarTrampa
{
    public GameObject proyectilLanzaArietesPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 3.0f; // Intervalo de disparo en segundos

    private bool puedeDisparar = false; // variable para controlar el disparo cuando se intancia


    public float fuerzaDisparo = 100.0f;

    public Animator animator;
    
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

    // Motodo para instanciar el proyectil
    public void DispararProyectil()
    {
        GameObject proyectilLanzaArietes = Instantiate(proyectilLanzaArietesPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectilLanzaArietes.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.AddForce(puntoDisparo.forward * fuerzaDisparo, ForceMode.Impulse);
            Debug.Log("Proyectil disparado en dirección: " + puntoDisparo.forward);
        }
        else
        {
            Debug.Log("El proyectil no tiene un Rigidbody.");
        }
    }
 
    public void Activar()
    {
        puedeDisparar = true; // Permitir disparar
        // Inicia el disparo
        InvokeRepeating("Disparar", 1f, intervaloDisparo);
    }
}