using UnityEngine;

public class CatapultaController : MonoBehaviour, ActivarTrampa 
{
    public Transform puntoDisparo;
    public GameObject proyectilPrefab;
    public float fuerzaDisparo = 10f;
    public Animator animator;
    private bool puedeDisparar = false; 


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void PrepararDisparo()
    {
        if (puedeDisparar)
        {
              animator.SetTrigger("disparo"); // Activa la animacion de disparo
        }
        
    }
    // Este metodo debe ser llamado desde un evento en la animacion en el momento exacto del disparo
    public void Disparar()
    {

        if (MusicController.instance != null)
        {

            MusicController.instance.PlayCatapultSound(transform);
            Debug.Log("Sonido de disparo desde la catapulta");
        }

        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = proyectil.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direccion = transform.forward + transform.up * 0.5f;
            rb.AddForce(direccion * fuerzaDisparo, ForceMode.Impulse);
        }
    }

    public void Activar()
    {
        puedeDisparar = true; // Permitir disparar
        // Llamar a la animacion de disparo inmediatamente y luego repetir cada 3 segundos
        InvokeRepeating(nameof(PrepararDisparo), 5.0f, 3.0f);

    }

}
