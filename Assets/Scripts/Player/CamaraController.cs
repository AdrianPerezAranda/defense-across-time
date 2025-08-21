using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovimiento))]
public class CamaraController : MonoBehaviour
{
    [SerializeField] public CinemachineCamera cameraPlayer;

    private PlayerMovimiento playerMovimiento;

    private float sens;

    private InputAction look;

    private float xRotation, yRotation;

    private InputAction aim, sprint;

    private float normalFOV = 65f;
    private float aimFOV = 45f;
    private float sprintFOV = 85f;

    public float transitionSpeedFOV = 1f;

    private float targetFOV, sprintValue, aimValue;

    private void OnEnable()
    {
        playerMovimiento = GetComponent<PlayerMovimiento>();
        sens = playerMovimiento.sens;
    }

    void Start()
    {
        // Configuración de acciones de entrada
        look = PlayerController.instance.inputMovement.FindActionMap("Player").FindAction("Look");
        aim = PlayerController.instance.inputMovement.FindActionMap("UI").FindAction("RightClick");
        sprint = PlayerController.instance.inputMovement.FindActionMap("Player").FindAction("Sprint");

        look.Enable();
        aim.Enable();
        sprint.Enable();

        targetFOV = normalFOV;
    }

    void Update()
    {
        // Lee el input de movimiento de la cámara
        Vector2 lookValue = look.ReadValue<Vector2>();



        Debug.LogError("LOOK VALUE: " + lookValue);



        // Lee las acciones de apuntar y correr
        aimValue = aim.ReadValue<float>();
        sprintValue = sprint.ReadValue<float>();


        // Actualiza las rotaciones, evitando la acumulación innecesaria
        xRotation -= lookValue.y * sens;
        yRotation += lookValue.x * sens;

        // Limita la rotación vertical (eje X)
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Aplica la rotación al jugador
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Lógica de FOV
        if (aimValue > 0)
        {
            targetFOV = aimFOV;
        }
        else if (sprintValue > 0)
        {
            targetFOV = sprintFOV;
        }
        else
        {
            targetFOV = normalFOV;
        }

        // Interpola suavemente el cambio de FOV
        cameraPlayer.Lens.FieldOfView = Mathf.Lerp(cameraPlayer.Lens.FieldOfView, targetFOV, Time.deltaTime * transitionSpeedFOV);
    }
}
