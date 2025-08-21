using System.Collections;
using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    // Animator de la puerta que se abre automáticamente
    [SerializeField] Animator doorAnimator;

    // Referencia al TutorialManager
    [SerializeField] TutorialManager tutorialManager;

    // Paso del tutorial relacionado a esta zona
    [SerializeField] TutorialManager.PasoTutorial paso;

    // Para evitar que se active más de una vez
    private bool triggered = false, puertaAbierta = false;

    private void OnTriggerEnter(Collider other)
    {
        // Si ya se activó antes, no hace nada
        if (triggered) return;

        // Si el jugador entra en la zona
        if (other.CompareTag("Player"))
        {

            StartCoroutine(_AbrirPuerta());

            // Evita reactivación
            triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (puertaAbierta && other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("StayOpen");

            Debug.Log("Mantener abierta");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador sale de la zona
        if (other.CompareTag("Player"))
        {
            triggered = false;

            if(puertaAbierta)
            {
                StartCoroutine(_CerrarPuerta());
            }
            
        }
    }

    private IEnumerator _AbrirPuerta()
    {
        // Abre la puerta
        doorAnimator.SetTrigger("Open");

        Debug.Log("Abrir puerta");

        // Marca el paso del tutorial como completado
        tutorialManager.AccionRealizada(paso);

        yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorStateInfo(0).length);

        puertaAbierta = true;

        if(!triggered) 
        {
            StartCoroutine(_CerrarPuerta());
        }
    }

    private IEnumerator _CerrarPuerta()
    {
        // Cierra la puerta
        doorAnimator.SetTrigger("Close");

        Debug.Log("Cerrar puerta");

        puertaAbierta = false;

        yield return null;
    }
}

