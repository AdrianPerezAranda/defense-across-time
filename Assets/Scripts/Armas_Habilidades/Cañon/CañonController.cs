using UnityEngine;

public class CañonController : MonoBehaviour, ActivarTrampa
{
    public GameObject proyectilCanonPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 10f;
    public Animator animator;
    public float intervaloDisparo = 3.0f;
    public bool puedeDisparar = false;


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
    
    
    public void LanzarCañon()
    {

        if (MusicController.instance != null)
        {
            MusicController.instance.PlayHeadshot(transform);
            Debug.Log("Sonido de disparo desde el cañon");
        }

        GameObject proyectilCañon = Instantiate(proyectilCanonPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectilCañon.GetComponent<Rigidbody>();


        if (rb != null)
        {
            rb.linearVelocity = puntoDisparo.forward * fuerzaDisparo;
        }
    }


   public void Activar()
   {
       puedeDisparar = true;
       InvokeRepeating("Disparar", 3.0f, intervaloDisparo);
   }

}
