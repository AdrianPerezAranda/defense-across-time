using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region SINGLETONE
    public static GameController Instance;
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public enum gameFase
    {
        playing,
        editingBase,
        inMenus
    }

    [SerializeField] GameConfig config;
    [HideInInspector] public GameObject timeMachine;

    public gameFase actualGameFase;
    public PlayerController playerInstance;

    public bool puedeAcabarOleada;
    public bool puedeEmpezarOleada;
    int vidaBase;

    public int idxDiaActual;

    [HideInInspector] public UnityEvent startPlaying;
    [HideInInspector] public UnityEvent startEditing;

    private void OnEnable()
    {


        //EVENTOS
        startPlaying.AddListener(SetGameFasePlaying);
        startPlaying.AddListener(SetCursorPlayingMode);
        startEditing.AddListener(SetGameFaseEditing);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        //Inicialicacion variables
        actualGameFase = gameFase.inMenus;
        puedeAcabarOleada = puedeEmpezarOleada = false;
        vidaBase = config.baseHealth;
        idxDiaActual = GetComponent<SaveLoadGameManager>().LoadDia();

        //COMENTAR ESTA LINEA PARA NO EMPEZAR SIEMPPRE EN EL TUTORIAL
        //PlayerPrefs.SetInt("Eras", 0);
    }

    private void OnDisable()
    {
        startPlaying.RemoveAllListeners();
        startEditing.RemoveListener(SetGameFaseEditing);
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

        //Guardo el dia actual donde se ha quedado en el ordenador
        GetComponent<SaveLoadGameManager>().SaveDia();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene == SceneManager.GetSceneByName("MainMenu"))
        {
            if (EnemiesController.instance) EnemiesController.instance.PararOleada();
        }
        else if (scene == SceneManager.GetSceneByName("Prehistory"))
        {
            if (playerInstance) playerInstance.gameObject.SetActive(true);
            startEditing?.Invoke();
            GetComponent<SaveableObjectsController>().LoadGame();
        }
        else if (scene == SceneManager.GetSceneByName("EdadMedia_Nueva"))
        {
            if (playerInstance)
            {
                playerInstance.gameObject.SetActive(true);
                playerInstance.AvanzarArma();
            }

            startEditing?.Invoke();
            GetComponent<SaveableObjectsController>().LoadGame();
        }
        else if (scene == SceneManager.GetSceneByName("Future"))
        {
            if (playerInstance)
            {
                playerInstance.gameObject.SetActive(true);
                playerInstance.AvanzarArma();
            }
            startEditing?.Invoke();
            GetComponent<SaveableObjectsController>().LoadGame();
        }
        else if(scene == SceneManager.GetSceneByName("LoadingScreen"))
        {
            if(playerInstance) playerInstance.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Funcion que se llamará siempre que la base sea atacada
    /// SOLO SE LLAMARA DESDE LA CLASE 'ENEMY'
    /// </summary>
    public void DoDamageToBase()
    {
        vidaBase--;
        HubControlllerGame.instance.UpdateVidaBase(vidaBase);
        //Cuando la vida de la base baja a 0 pierdes
        if (vidaBase == 0 ) { PartidaPerdida(); }
    }

    /// <summary>
    /// Funcion que se llamara siempre que tenga que empezar una nueva oleada
    /// </summary>
    public void EmpezarSiguienteOlada()
    {
        puedeEmpezarOleada = false;

        //CuandoEmpiezas la siguiente oleada cambia el gameEstate
        startPlaying?.Invoke();

        //Desactivas el aviso de nueva oleada
        HubControlllerGame.instance.avisoEmpezarNuevaOleada.SetActive(false);

        EnemiesController.instance.EmpezarSiguienteOleada();
    }

    /// <summary>
    /// Funcion para cuando la partida esta perdida
    /// </summary>
    private void PartidaPerdida()
    {
        playerInstance.gameObject.SetActive(false);
        MusicController.instance.PlayDefeat();

        //Mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene("LoseScene");

        actualGameFase = gameFase.inMenus;

    }
    public void OleadaGanada()
    {

        puedeEmpezarOleada = true;
        
        //Cuando acabas una oleada puedes editar la base
        startEditing?.Invoke();

        //Activas el aviso de nueva oleada
        HubControlllerGame.instance.avisoEmpezarNuevaOleada.SetActive(true);
        puedeAcabarOleada = false;

        actualGameFase = gameFase.editingBase;

        EnemiesController.instance.CalcularSiguienteOleada();
        EnemiesController.instance.HaveToPassEra();
    }

    public void PartidaGanada()
    {
        playerInstance.gameObject.SetActive(false);

        GetComponent<SaveLoadGameManager>().DeleteSavedDay();


        MusicController.instance.PlayWin();
        SceneManager.LoadScene("CreditsScreen");
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        actualGameFase = gameFase.inMenus;
    }

    public void NewMapLoaded()
    {
        puedeEmpezarOleada = true;
        startPlaying?.Invoke();
    }

    public GameConfig GetGameConfig()
    {
        return config;
    }

    public void SetCursorPlayingMode()
    {
        // Se desactiva la vision del raton.
        Cursor.visible = false;

        // Se centra el raton en mitad de la pantalla de juego.
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetGameFasePlaying()
    {
        actualGameFase = gameFase.editingBase;
    }

    private void SetGameFaseEditing()
    {
        actualGameFase = gameFase.editingBase;
    }

    [ContextMenu("AvanzarEpoca")]
    public void AvanzarEpoca()
    {
        PlayerPrefs.SetInt("Eras", Mathf.Min(PlayerPrefs.GetInt("Eras", 0) + 1, 3));

        SceneManager.LoadScene("LoadingScreen");
    }

}
