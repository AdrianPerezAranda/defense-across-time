using UnityEngine;

public class DragAndDropWall : MonoBehaviour
{
    public GameObject[] trapPrefabsWall;
    private GameObject currentTrapWall;
    private Camera mainCameraWall;
    public float gridSizeWall = 1f;
    private bool isDragging = false;
    private int selectedTrapWallIndex = 0;

    private HubControlllerGame hubController;

    private void Awake()
    {
        mainCameraWall = Camera.main;
        hubController = FindAnyObjectByType<HubControlllerGame>();
    }

    private void Update()
    {
        // Selección de trampas
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectTrapWall(0);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectTrapWall(1);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectTrapWall(2);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectTrapWall(3);

        if (currentTrapWall != null && isDragging)
        {
            Ray ray = mainCameraWall.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Verificar si el objeto golpeado tiene la etiqueta "
                if (hit.collider.CompareTag("Pared"))
                {
                    Vector3 position = hit.point + hit.normal * 0.05f;
                    Quaternion rotation = Quaternion.LookRotation(-hit.normal); // Orientación correcta

                    currentTrapWall.transform.position = position;
                    currentTrapWall.transform.rotation = rotation;
                }
                else
                {
                    Debug.Log("No es una pared válida para colocar la trampa.");
                }
            }
        }

        // Colocar la trampa en la pared
        if (Input.GetMouseButtonDown(0) && currentTrapWall != null && isDragging)
        {
            isDragging = false;
            currentTrapWall = null;
        }
    }

    private void SelectTrapWall(int index)
    {
        if (currentTrapWall != null)
        {
            Debug.Log("Ya hay una trampa siendo arrastrada. Colócala antes de seleccionar otra.");
            return;
        }

        if (index >= 0 && index < trapPrefabsWall.Length)
        {
            selectedTrapWallIndex = index;
            Debug.Log($"Trampa seleccionada: {trapPrefabsWall[selectedTrapWallIndex].name}");

            if (hubController != null && hubController.ComprarTrampa(index))
            {
                Vector3 spawnPosition = Vector3.zero;
                currentTrapWall = Instantiate(trapPrefabsWall[selectedTrapWallIndex], spawnPosition, Quaternion.identity);
                isDragging = true;
            }
            else
            {
                Debug.Log("No tienes dinero suficiente para comprar esta trampa.");
            }
        }
        else
        {
            Debug.Log("Trampa fuera de rango");
        }
    }
}
