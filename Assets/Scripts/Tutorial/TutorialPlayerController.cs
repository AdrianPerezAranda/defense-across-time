using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class TutorialPlayerController : MonoBehaviour
{
    // Asset de Input System con los mapas de acción
    [SerializeField] InputActionAsset inputMovement;

    [Header("Movimiento")]
    // Velocidad de movimiento base
    public float speed = 5f;

    // Altura del salto
    public float jumpHeight = 1.5f;

    // Gravedad aplicada al jugador
    public float gravity = -9.81f;

    // Multiplicador de velocidad al correr
    public float sprintMultiplier = 1.5f;

    [Header("Cámara")]
    // Sensibilidad del mouse para mirar
    public float sensibilidad = 100f;

    // Referencia a la cámara Cinemachine
    public CinemachineCamera camara;

    // Campo de visión normal
    public float normalFOV = 65f;

    // Campo de visión al correr
    public float sprintFOV = 85f;

    // Campo de visión al apuntar
    public float aimFOV = 45f;

    // Velocidad con la que cambia el FOV entre estados
    public float transitionSpeedFOV = 2f;

    [Header("Referencias")]
    // Referencia al TutorialManager para informar acciones
    public TutorialManager tutorialManager;

    // Componente del CharacterController
    private CharacterController controller;

    // Velocidad vertical acumulada
    private Vector3 velocity;

    // Si el jugador está tocando el suelo
    private bool isGrounded;

    // Acciones de Input System
    private InputAction move, look, jump, sprint, aim, interact, shoot;

    // Almacena el input del mouse
    private Vector2 inputLook, inputMove;

    // Acumulador para la rotación vertical
    private float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();

        // Obtiene los mapas de acción
        var mapPlayer = inputMovement.FindActionMap("Player");
        var mapUI = inputMovement.FindActionMap("UI");

        // Asocia las acciones del mapa
        move = mapPlayer.FindAction("Move");
        look = mapPlayer.FindAction("Look");
        jump = mapPlayer.FindAction("Jump");
        sprint = mapPlayer.FindAction("Sprint");
        interact = mapPlayer.FindAction("Interact");
        shoot = mapPlayer.FindAction("Attack");

        aim = mapUI.FindAction("RightClick");

        // Activa las acciones
        move.Enable();
        look.Enable();
        jump.Enable();
        sprint.Enable();
        aim.Enable();
        interact.Enable();
        shoot.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Lee el input del mouse y lo escala por sensibilidad y deltaTime
        inputLook = look.ReadValue<Vector2>() * sensibilidad * Time.deltaTime;

        // Si el jugador mueve la cámara y está permitido, marca el paso como completado
        if (inputLook.magnitude > 0.1f &&
            tutorialManager.PermitePaso(TutorialManager.PasoTutorial.MoverCamara))
        {
            tutorialManager.AccionRealizada(TutorialManager.PasoTutorial.MoverCamara);
        }

        // Aplica rotación vertical de la cámara
        xRotation -= inputLook.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        camara.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplica rotación horizontal al jugador
        transform.Rotate(Vector3.up * inputLook.x);

        // Verifica si está en el suelo
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        // Lee el movimiento en crudo
        Vector2 rawMove = move.ReadValue<Vector2>();

        // Si se permite moverse, usa el input; si no, se queda en cero
        Vector2 inputMove = tutorialManager.PermitePaso(TutorialManager.PasoTutorial.Moverse) ? rawMove : Vector2.zero;

        // Calcula la dirección de movimiento en el mundo
        Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

        // Verifica si se permite correr y se está presionando el botón
        bool puedeCorrer = tutorialManager.PermitePaso(TutorialManager.PasoTutorial.Correr);
        float sprintInput = sprint.ReadValue<float>();

        // Aplica multiplicador de velocidad si se corre
        float currentSpeed = (puedeCorrer && sprintInput > 0.1f) ? speed * sprintMultiplier : speed;

        // Marca el paso correr si se está corriendo y está permitido
        if (puedeCorrer && sprintInput > 0.1f)
            tutorialManager.AccionRealizada(TutorialManager.PasoTutorial.Correr);

        // Mueve al jugador
        controller.Move(moveDir * currentSpeed * Time.deltaTime);

        // Salto
        if (jump.triggered &&
            isGrounded &&
            tutorialManager.PermitePaso(TutorialManager.PasoTutorial.Saltar))
        {
            // Aplica fórmula de física para altura de salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            tutorialManager.AccionRealizada(TutorialManager.PasoTutorial.Saltar);
        }

        // Aplica gravedad acumulativa
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Cambia el FOV dependiendo si corre o apunta
        float targetFOV = normalFOV;

        if (tutorialManager.PermitePaso(TutorialManager.PasoTutorial.Apuntar) &&
            aim.ReadValue<float>() > 0.1f)
        {
            targetFOV = aimFOV;
        }
        else if (sprintInput > 0.1f && puedeCorrer)
        {
            targetFOV = sprintFOV;
        }

        // Interpola suavemente el cambio de FOV
        camara.Lens.FieldOfView = Mathf.Lerp(camara.Lens.FieldOfView, targetFOV, Time.deltaTime * transitionSpeedFOV);
    }
}

