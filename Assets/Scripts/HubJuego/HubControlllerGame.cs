using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
// Clase para representar una trampa
public class Trampa
{
    public Image imagenTrampa;
    public int precio;
    public TextMeshProUGUI indicadorPrecio;
    
}

// Clase para representar una Arma
[System.Serializable]
public class ArmaDistancia
{
    public Image imagenArma;
    public bool estaDesbloqueada;
    public int municion;
    public int municionTotal;
    public int daño;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI indicadorMunicion;
    public TextMeshProUGUI indicadorMunicioTotal;

}

// Clase de armas cuerpo a cuerpo
[System.Serializable]
public class ArmaCaC
{
    public Image imagenArma;
    public bool estaDesbloqeuada;
    public int daño;
    public TextMeshProUGUI nombre;
}

public class HubControlllerGame : MonoBehaviour
{
    public static HubControlllerGame instance;


    [SerializeField] private Slider vidaPersonaje; // Vida del personaje en un Slider   
    [SerializeField] private Slider vidaBase; // Vida de la base en un Slider
    [SerializeField] private TextMeshProUGUI indicadorDinero; // Texto para mostrar el dinero disponible
    [SerializeField] private int dinero;// Dinero disponible para poder comprar las trampas
    [SerializeField] public GameObject avisoEmpezarNuevaOleada; //Aviso para pulsar cuando se pueda empezar una nueva oleada
                                      
    // Array de trampas
    [Header("Array de trampas")]
    [SerializeField] private Trampa[] trampas;


    //Arrray de Armas 
    [Header("Array de armas a distancia")]
    [SerializeField] private ArmaDistancia[] ArmasDistancia;

    //Array de armas cuerpo a cuerpo
    [Header("Array de armas cuerpo a cuerpo")]
    [SerializeField] private ArmaCaC[]armasCaC;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    void Start()
    {   
        // Inicializaci�n de los valores de los Sliders
        if (vidaPersonaje != null)
        {
            vidaPersonaje.maxValue = GameController.Instance.GetGameConfig().playerHealth;  // Valor m�ximo de vida del personaje
            vidaPersonaje.value = vidaPersonaje.maxValue;                                   // Valor inicial de vida del personaje
        }

        if (vidaBase != null)
        {
            vidaBase.maxValue = GameController.Instance.GetGameConfig().baseHealth; // Valor m�ximo de vida de la base
            vidaBase.value = vidaBase.maxValue;                                     // Valor inicial de vida de la base
        }


        // Inicializaci�n del texto del dinero
        indicadorDinero.text = PlayerController.instance.shopCoins.ToString();

        // Inicializaci�n de los precios de las trampas
        ActualizarEstadoTrampas();
    }

    void Update()
    {
        //comprarTrampaTeclado();
    }

    // M�todo para actualizar la vida del personaje
    public void UpdateVidaPersonaje(float vida)
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.value = vida;
        }
    }

    // M�todo para actualizar la vida de la base
    public void UpdateVidaBase(float vida)
    {
        if (vidaBase != null)
        {
            vidaBase.value = vida;
        }
    }


    // M�todo para actualizar el estado de las trampas en funci�n del dinero disponible
    public void ActualizarEstadoTrampas()
    {

        // Recorremos el array de trampas
        for (int i = 0; i < trampas.Length; i++)
        {

            // si el dinero es mayor que el valor de la trampa se muestra en verde
            if (PlayerController.instance.shopCoins >= trampas[i].precio)
            {
                trampas[i].indicadorPrecio.color = Color.green;
            }

            // si el dinero es menor que el valor de la trampa se muestra en rojo
            else
            {
                trampas[i].indicadorPrecio.color = Color.red;
            }

            // Actualizamos el precio de la trampa
            trampas[i].indicadorPrecio.text = trampas[i].precio.ToString();
        }
    }

    // M�todo para actualizar el dinero y el estado de las trampas
    public void ActualizarDinero(int cantidad)
    {
        PlayerController.instance.shopCoins += cantidad;
        indicadorDinero.text = PlayerController.instance.shopCoins.ToString();
        ActualizarEstadoTrampas();
    }



    // Metodo para comprar una trampa
    // M�todo para comprar una trampa (ahora devuelve un bool)
    public bool ComprarTrampa(int indice)
    {
        Debug.Log($"Intentando comprar trampa en �ndice:  { indice} + { trampas.Length}");
        if (indice >= 0 && indice < trampas.Length)
        {
            Trampa trampa = trampas[indice];
            if (PlayerController.instance.shopCoins >= trampa.precio)
            {
                PlayerController.instance.shopCoins -= trampa.precio;
                indicadorDinero.text = PlayerController.instance.shopCoins.ToString();
                ActualizarEstadoTrampas();
                return true; // Compra exitosa
            }
            else
            {
                Debug.Log("No tienes suficiente dinero para comprar esta trampa.");
                return false; // Compra fallida
            }
        }
        else
        {
            Debug.Log("�ndice de trampa fuera de rango.");
            return false; // �ndice fuera de rango
        }
    }

    //Metodo verificar si puedo comprar una trampa
    public void comprarTrampaTeclado()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ComprarTrampa(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ComprarTrampa(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ComprarTrampa(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ComprarTrampa(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ComprarTrampa(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ComprarTrampa(5);
        }
    }
}


