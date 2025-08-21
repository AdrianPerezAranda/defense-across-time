using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovimiento : MonoBehaviour
{
    [SerializeField] public float sens;

    private InputAction movement, jump, sprint;


    public float speed = 5f;              // Velocidad del personaje
    public float gravity = -9.81f;        // Gravedad
    public float jumpHeight = 1.5f;       // Altura del salto

    private CharacterController controller;
    private Vector3 velocity;            // Controla la velocidad (gravedad)
    private bool isGrounded;             // Verifica si está en el suelo
    private float horizontal, vertical;  // Valores de entrada de movimiento

    // Variable para controlar el tiempo entre sonidos de pasos
    private float stepTimer = 0f;
    private float stepInterval = 0.5f; // Ajusta este valor para cambiar la frecuencia de los pasos

    void Start()
    {
        controller = GetComponent<CharacterController>();

        movement = PlayerController.instance.inputMovement.FindActionMap("Player").FindAction("Move");

        jump = PlayerController.instance.inputMovement.FindActionMap("Player").FindAction("Jump");

        sprint = PlayerController.instance.inputMovement.FindActionMap("Player").FindAction("Sprint");
    }

    void Update()
    {

        // Verificar si está tocando el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Asegura que el jugador se mantenga pegado al suelo
        }

        // Capturar entrada de movimiento
        horizontal = movement.ReadValue<Vector2>().x; // Teclas A/D
        vertical = movement.ReadValue<Vector2>().y;   // Teclas W/S


        transform.right = new Vector3(transform.right.x, 0, transform.right.z);
        transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);

        Vector3 moveX = horizontal * transform.right;
        Vector3 moveZ = vertical * transform.forward;

        Vector3 move = moveX + moveZ;

        // Movimiento y sprint
        bool isMoving = (horizontal != 0 || vertical != 0);

        if (isMoving && sprint.ReadValue<float>() <= 0)
        {

            controller.Move(move * speed * Time.deltaTime);

            // Controla la reproducción del sonido de los pasos
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                MusicController.instance.PlayStepsPlayer(transform);
                stepTimer = stepInterval; // Reinicia el temporizador
            }
        }
        else if (isMoving && sprint.ReadValue<float>() > 0)
        {
            controller.Move(move * (speed * 1.5f) * Time.deltaTime);

            // Controla la reproducción del sonido de los pasos
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0.15f)
            {
                MusicController.instance.PlayStepsPlayer(transform);
                stepTimer = stepInterval; // Reinicia el temporizador
            }
        }
        else
        {
            // Reinicia el temporizador cuando el jugador deja de moverse
            stepTimer = 0f;
        }

        // Salto
        if ((jump.ReadValue<float>() > 0) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetPlayerVelocityToZero()
    {
        velocity = Vector3.zero;      
    }
}
