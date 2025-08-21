using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialOpenDoor : MonoBehaviour
{
    // Asset de Input System
    [SerializeField] InputActionAsset inputMovement;

    // Animator de la primera puerta
    [SerializeField] Animator doorAnimator1;

    // Animator de la segunda puerta
    [SerializeField] Animator doorAnimator2;

    // Referencia al TutorialManager
    [SerializeField] TutorialManager tutorialManager;

    // Paso del tutorial asociado a esta puerta
    [SerializeField] TutorialManager.PasoTutorial paso;

    // Flag para evitar múltiples ejecuciones
    private bool triggered = false;

    private void OnTriggerStay(Collider other)
    {
        // Si ya se activó antes, no hace nada
        if (triggered) return;

        // Si el jugador está dentro y presiona "Interactuar"
        if (other.CompareTag("Player") && inputMovement.FindActionMap("Player").FindAction("Interact").ReadValue<float>() != 0)
        {
            // Abre ambas puertas
            doorAnimator1.SetTrigger("Open");
            doorAnimator2.SetTrigger("Open");

            // Marca el paso del tutorial como completado
            tutorialManager.AccionRealizada(paso);

            // Impide que se vuelva a activar
            triggered = true;
        }
    }
}

