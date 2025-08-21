using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    public GameObject[] trapPrefabs;
    private GameObject currentTrap;
    private Camera mainCamera;
    public float gridSize = 1f;
    private bool isDragging = false;
    private int selectedTrapIndex = 0;
    public float maxDistancia = 35.0f; // Distancia máxima para colocar la trampa

    private HubControlllerGame hubController;
    public InputActionAsset inputActions;
    private InputAction selectTrapAction;

    private Transform playerTransform;

    // suavizar el movimiento
    private Vector3 targetPosition;
    private float smoothTime = 0.1f;
    private Vector3 velocity;

    // Para feedback visual
    private Renderer trapRenderer;
    private Color colorNormal = Color.white;


    // Variables para controalar la superposicion de trampas
    private List<Vector3> posicionesTrampas = new List<Vector3>();
    public float radioSuperposicion = 1.0f;


    private void Awake()
    {
        mainCamera = Camera.main;
        hubController = FindAnyObjectByType<HubControlllerGame>();

        var playerActionMap = inputActions.FindActionMap("Player");
        selectTrapAction = playerActionMap.FindAction("SeleccionarTrampa");
        selectTrapAction.performed += OnSelectTrap;
    }

    private void Start()
    {
        playerTransform = GameController.Instance.playerInstance.transform;
    }
    private void OnEnable()
    {
        selectTrapAction.Enable();
    }

    private void OnDisable()
    {
        selectTrapAction.Disable();
    }

    private void Update()
    {
        if (currentTrap != null && isDragging)
        {

            // Cancelar el arrastre si se presiona el clic derecho
            if (Input.GetMouseButtonDown(1))
            {
                CancelarArrastre();
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 position = hit.point;
                Quaternion spawnRotation = GameController.Instance.playerInstance.gameObject.transform.rotation;
                Vector3 rot = spawnRotation.eulerAngles;
                rot.x = 0;
                rot.z = 0;
                spawnRotation = Quaternion.Euler(rot);

                position.x = Mathf.Round(position.x / gridSize) * gridSize;
                position.z = Mathf.Round(position.z / gridSize) * gridSize;

                if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out RaycastHit groundHit))
                {
                    targetPosition = groundHit.point;
                    currentTrap.transform.position = Vector3.SmoothDamp(currentTrap.transform.position, targetPosition, ref velocity, smoothTime);
                    currentTrap.transform.rotation = spawnRotation;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && currentTrap != null && isDragging)
        {
            // Comprueba si hay una trampa instanciada en esta posicion
            if (HayTrampaEnPosicion(currentTrap.transform.position))
            {
                Debug.Log("Ya hay una trampa en esta posicion.");
                return;
            }

            // Limita la distancia en que se coloca la trampa
            float distance = Vector3.Distance(playerTransform.position, currentTrap.transform.position);
            if (distance > maxDistancia)
            {
                Debug.Log("No puedes colocar la trampa tan lejos del jugador.");
                return;
            }

            // Cobra en esta parte el valor de la trampa
            if (hubController != null && hubController.ComprarTrampa(selectedTrapIndex)) //
            {
                ActivarTrampa trampaActivable = currentTrap.GetComponent<ActivarTrampa>();
                if (trampaActivable != null) trampaActivable.Activar();

                // Agrega la posición de la trampa para evitar superposición
                posicionesTrampas.Add(currentTrap.transform.position);

                currentTrap = null;
                isDragging = false;
            }
            else
            {
                Debug.Log("No tienes dinro suficiente para comprar esta trampa.");
                CancelarArrastre(); 
            }



        }
    }
    private void OnSelectTrap(InputAction.CallbackContext context)
    {
        string keyPressed = context.control.displayName;

        int trapIndex;
        if (int.TryParse(keyPressed, out trapIndex))
        {
            trapIndex -= 1;
            Debug.Log($"Seleccionando trampa con índice: {trapIndex}");
            SelectTrap(trapIndex);
        }
        else
        {
            Debug.Log("No se puede convertir");
        }
    }

    private void SelectTrap(int index)
    {
        if (trapPrefabs == null || trapPrefabs.Length == 0)
        {
            Debug.LogError("El array trapPrefabs está vacío o no ha sido asignado.");
            return;
        }

        if (currentTrap != null)
        {
            Debug.Log("Ya hay una trampa siendo arrastrada. Colócala antes de seleccionar otra.");
            return;
        }

        if (index >= 0 && index < trapPrefabs.Length)
        {
            selectedTrapIndex = index;
            Debug.Log($"Trampa seleccionada: {trapPrefabs[selectedTrapIndex].name}");

            Vector3 spawnPosition = Vector3.zero;
            currentTrap = Instantiate(trapPrefabs[selectedTrapIndex], spawnPosition, Quaternion.identity);
            if (currentTrap != null)
            {
                trapRenderer = currentTrap.GetComponentInChildren<Renderer>();
                if (trapRenderer != null) trapRenderer.material.color = colorNormal;
                isDragging = true;
            }
            else
            {
                Debug.LogError("Error al instanciar la trampa");
            }
        }
        else
        {
            Debug.Log("Trampa fuera de rango");
        }
    }




    private bool HayTrampaEnPosicion(Vector3 posicion)
    {
        Vector3 posicionComparar = new Vector3(posicion.x, 0, posicion.z);
        Debug.Log($"Verificando posición: {posicionComparar}");

        foreach (var pos in posicionesTrampas)
        {
            Vector3 posOcupada = new Vector3(pos.x, 0, pos.z);
            Debug.Log("Posicion ocupada");

            if (Vector3.Distance(posOcupada, posicionComparar) < radioSuperposicion)
            {
                Debug.Log("Posición ocupada detectada.");
                return true;
            }
        }
        Debug.Log("Posición libre.");
        return false;
    }
    public void CancelarArrastre()
    {

        if (hubController != null)
        {
            Destroy(currentTrap);
            currentTrap = null;
            isDragging = false;

            //hubController.DevolverDinero(selectedTrapIndex);
        }
    }

    // Motodos para instanciar las trampas
    public void ActualizarTrampasSeleccionadas(List<GameObject> trampasSeleccionadas)
    {
        trapPrefabs = trampasSeleccionadas.ToArray();
        Debug.Log("Trampas actualizadas en DragAndDrop");
    }
}