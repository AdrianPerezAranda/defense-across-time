using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] public InputActionAsset input;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public GameObject controlsMenu;
    [SerializeField] public GameObject lenguageMenu;

    private InputAction pause;

    void Start()
    {
        input.Enable();

        pause = input.FindActionMap("Player").FindAction("Pause");
    }

    void Update()
    {
        // Se comprueba que se actiba la tecla de pausa (Esc) y que el menu de pausa esta desactivado.
        if (pause.ReadValue<float>() != 0 && !pauseMenu.activeInHierarchy && !audioMenu.activeInHierarchy && !controlsMenu.activeInHierarchy && !lenguageMenu.activeInHierarchy)
        {
            // Se pausa el tiempo del juego.
            Time.timeScale = 0;
            
            // Se activa el menu de pausa.
            pauseMenu.SetActive(true);

            Cursor.visible = true; // Mostrar cursor

            Cursor.lockState = CursorLockMode.None;

        }
    }
}
