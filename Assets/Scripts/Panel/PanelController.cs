using UnityEngine;
using UnityEngine.InputSystem;

public class PanelController : MonoBehaviour
{
    public GameObject panelArbolHabilides; // Panel arbol de habilidades
    public GameObject panelJuego; // Panel del hub de juego
    public GameObject panelInventario; // Referencia al inventario


    // Referencias al input de arbol de habilidades
    public InputActionAsset input; // Referencia al input
    private InputAction abrirPanelArbolHabilidades; // referencia a la accion de abrir el panel


    // Referencia al Input de Inventario

    private InputAction abrirInventarioAccion; // referencia a la accion de abrir el panel




    private void Awake()
    { 
        // Inicializaxcion del input
        var playerMap = input.FindActionMap("UI");

        // Acciones del input system de arbol de habilidades e inventario
        abrirPanelArbolHabilidades = playerMap.FindAction("ArbolHabilidades");
        abrirInventarioAccion = playerMap.FindAction("Inventario"); 

        abrirPanelArbolHabilidades.performed += ctx => TogglePanelArbolHabilidades(); // Asigan  la funcion al evento de la accion arbol de habilidades
        abrirInventarioAccion.performed += ctx => TogglePanelInventario(); // Asigna la funcion al evento de la accion inventario
    }

    private void OnEnable()
    {
        abrirPanelArbolHabilidades.Enable();
        abrirInventarioAccion.Enable(); // 
    }

    private void OnDisable()
    {
        abrirPanelArbolHabilidades.Disable();
        abrirInventarioAccion.Disable(); 
    }

    private void Start()
    {
        panelArbolHabilides.SetActive(false); // Se desactica el panel al inicio
        panelInventario.SetActive(false); // Se desactica el panel al inicio
    }


    void TogglePanelArbolHabilidades() // Funcion para activar y desactivar el panel
    {
        bool panelActivo = !panelArbolHabilides.activeSelf;
        panelArbolHabilides.SetActive(panelActivo);

        if (panelActivo)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelArbolHabilides.GetComponent<ArbolDeHabilidades>().ActualizarUI();
            panelJuego.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            panelArbolHabilides.GetComponent<ArbolDeHabilidades>().OcultarInfoHabilidad();
            panelJuego.SetActive(true);
        }
    }


    void TogglePanelInventario()
    {
        bool panelInvetarioActivo = !panelInventario.activeSelf;
        panelInventario.SetActive(panelInvetarioActivo);

        if (panelInvetarioActivo)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelJuego.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            panelJuego.SetActive(true);
        }
    }


 
}
