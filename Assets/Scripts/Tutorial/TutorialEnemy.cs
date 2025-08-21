using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    // Referencia al TutorialManager
    [SerializeField] TutorialManager tutorialManager;

    // Paso del tutorial asociado a este enemigo (ej: Disparar o Apuntar)
    [SerializeField] TutorialManager.PasoTutorial paso;

    private void OnTriggerEnter(Collider other)
    {
        // Si lo que colisionó tiene el tag "Bala"
        if (other.CompareTag("Bala"))
        {
            // Marca el paso del tutorial como completado
            tutorialManager.AccionRealizada(paso);

            MusicController.instance.PlayKillEnemyTutorial(transform);
            // Destruye al enemigo
            Destroy(gameObject);
        }
    }
}
