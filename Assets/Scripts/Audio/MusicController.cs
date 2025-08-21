using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    #region SINGLETONE
    public static MusicController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private FMODUnity.EventReference eventReferenceBowShot;

    private FMOD.Studio.EventInstance bowShot;

    [SerializeField] private FMODUnity.EventReference eventReferenceBlunderbussShot;

    private FMOD.Studio.EventInstance blunderbussShot;

    [SerializeField] private FMODUnity.EventReference eventReferencePlayerSteps;

    private FMOD.Studio.EventInstance playerSteps;

    [SerializeField] private FMODUnity.EventReference eventReferenceEnemyScream;

    private FMOD.Studio.EventInstance enemyScream;

    [SerializeField] private FMODUnity.EventReference eventReferenceEnemySound;

    private FMOD.Studio.EventInstance enemySound;

    [SerializeField] private FMODUnity.EventReference eventReferenceButtonClick;

    private FMOD.Studio.EventInstance buttonClick;

    [SerializeField] private FMODUnity.EventReference eventReferenceBuyItem;

    private FMOD.Studio.EventInstance buyItem;

    [SerializeField] private FMODUnity.EventReference eventReferenceCatapultSound;

    private FMOD.Studio.EventInstance catapultSound;

    [SerializeField] private FMODUnity.EventReference eventReferenceDefeat;

    private FMOD.Studio.EventInstance defeat;

    [SerializeField] private FMODUnity.EventReference eventReferenceFlameThrower;

    private FMOD.Studio.EventInstance flameThrower;

    [SerializeField] private FMODUnity.EventReference eventReferenceHeadshot;

    private FMOD.Studio.EventInstance headshot;

    [SerializeField] private FMODUnity.EventReference eventReferencePlayerHurt;

    private FMOD.Studio.EventInstance playerHurt;

    [SerializeField] private FMODUnity.EventReference eventReferenceReloadGun;

    private FMOD.Studio.EventInstance reloadGun;

    [SerializeField] private FMODUnity.EventReference eventReferenceStartRound;

    private FMOD.Studio.EventInstance startRound;

    [SerializeField] private FMODUnity.EventReference eventReferenceStartRoundBoss;

    private FMOD.Studio.EventInstance startRoundBoss;

    [SerializeField] private FMODUnity.EventReference eventReferenceTimeMachine;

    private FMOD.Studio.EventInstance timeMachine;

    [SerializeField] private FMODUnity.EventReference eventReferenceWin;

    private FMOD.Studio.EventInstance win;

    [SerializeField] private FMODUnity.EventReference eventReferenceTorretaTesla;

    private FMOD.Studio.EventInstance torretaTesla;

    [SerializeField] private FMODUnity.EventReference eventReferenceTorretaGravedad;

    private FMOD.Studio.EventInstance torretaGravedad;

    [SerializeField] private FMODUnity.EventReference eventReferenceBowShot2;

    private FMOD.Studio.EventInstance BowShot2;

    [SerializeField] private FMODUnity.EventReference eventReferenceSpawnEnemyTutorial;

    private FMOD.Studio.EventInstance spawnEnemyTutorial;

    [SerializeField] private FMODUnity.EventReference eventReferenceKillEnemyTutorial;

    private FMOD.Studio.EventInstance killEnemyTutorial;

    [SerializeField] private FMODUnity.EventReference eventReferenceHitmarker;

    private FMOD.Studio.EventInstance hitmarker;

    [Header("Music")]
    // Referencias a eventos de FMOD para cada tipo de música
    [SerializeField] private FMODUnity.EventReference eventReferenceMusicFuture;

    [SerializeField] private FMODUnity.EventReference eventReferenceMusicMiddleAge;

    [SerializeField] private FMODUnity.EventReference eventReferenceMusicPrehistoric;

    [SerializeField] private FMODUnity.EventReference eventReferenceMusicMenu;

    // Instancia de musica activa actualmente
    private FMOD.Studio.EventInstance currentMusic;

    private void Start()
    {
        bowShot = RuntimeManager.CreateInstance(eventReferenceBowShot);

        blunderbussShot = RuntimeManager.CreateInstance(eventReferenceBlunderbussShot);

        playerSteps = RuntimeManager.CreateInstance(eventReferencePlayerSteps);

        enemyScream = RuntimeManager.CreateInstance(eventReferenceEnemyScream);

        enemySound = RuntimeManager.CreateInstance(eventReferenceEnemySound);

        buttonClick = RuntimeManager.CreateInstance(eventReferenceButtonClick);

        buyItem = RuntimeManager.CreateInstance(eventReferenceBuyItem);

        catapultSound = RuntimeManager.CreateInstance(eventReferenceCatapultSound);

        defeat = RuntimeManager.CreateInstance(eventReferenceDefeat);

        flameThrower = RuntimeManager.CreateInstance(eventReferenceFlameThrower);

        headshot = RuntimeManager.CreateInstance(eventReferenceHeadshot);

        playerHurt = RuntimeManager.CreateInstance(eventReferencePlayerHurt);

        reloadGun = RuntimeManager.CreateInstance(eventReferenceReloadGun);

        startRound = RuntimeManager.CreateInstance(eventReferenceStartRound);

        startRoundBoss = RuntimeManager.CreateInstance(eventReferenceStartRoundBoss);

        timeMachine = RuntimeManager.CreateInstance(eventReferenceTimeMachine);

        win = RuntimeManager.CreateInstance(eventReferenceWin);

        torretaTesla = RuntimeManager.CreateInstance(eventReferenceTorretaTesla);

        torretaGravedad = RuntimeManager.CreateInstance(eventReferenceTorretaGravedad);

        BowShot2 = RuntimeManager.CreateInstance(eventReferenceBowShot2);

        killEnemyTutorial = RuntimeManager.CreateInstance(eventReferenceKillEnemyTutorial);

        hitmarker = RuntimeManager.CreateInstance(eventReferenceHitmarker);

        spawnEnemyTutorial = RuntimeManager.CreateInstance(eventReferenceSpawnEnemyTutorial);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        bowShot.release();
        blunderbussShot.release();
        playerSteps.release();
        enemyScream.release();
        enemySound.release();
        buttonClick.release();
        buyItem.release();
        catapultSound.release();
        defeat.release();
        flameThrower.release();
        headshot.release();
        playerHurt.release();
        reloadGun.release();
        startRound.release();
        startRoundBoss.release();
        timeMachine.release();
        win.release();
        torretaTesla.release();
        torretaGravedad.release();
        BowShot2.release();
        killEnemyTutorial.release();
        hitmarker.release();
        spawnEnemyTutorial.release();

        // Liberamos la instancia de musica actual
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }


    }

    public void PlayButtonClick() 
    {

        buttonClick.start();

    }

    public void PlayEnemySound(Transform sourceTransform) 
    {
        enemySound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        enemySound.start();
    }

    public void PlayBuyItem() 
    {
       
        buyItem.start();

    }

    public void PlayCatapultSound(Transform sourceTransform) 
    {
        catapultSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        catapultSound.start();
    }

    public void PlayDefeat() 
    {

        defeat.start();

    }


    public void PlayFlameThrower(Transform sourceTransform) 
    {
        flameThrower.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        flameThrower.start();
    }

    public void PlayHeadshot(Transform sourceTransform) 
    {
        headshot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        headshot.start();
    }

    public void PlayPlayerHurt() 
    {

        playerHurt.start();

    }

    public void PlayReloadGun(Transform sourceTransform) 
    {
        reloadGun.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        reloadGun.start();
    }

    public void PlayStartRound() 
    {

        startRound.start();

    }

    public void PlayStartRoundBoss() 
    {

        startRoundBoss.start();

    }

    public void PlayTimeMachine(Transform sourceTransform) 
    {
        timeMachine.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        timeMachine.start();
    }

    public void PlayWin() 
    {

        win.start();

    }

    public void PlayBowShot(Transform sourceTransform)
    {

        bowShot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        bowShot.start();

    }

    public void PlayStepsPlayer(Transform sourceTransform)
    {
        playerSteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        playerSteps.start();
    }

    public void PlayEnemyScream(Transform sourceTransform)
    {
        enemyScream.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        enemyScream.start();
    }

    public void PlayBlunderbussShot(Transform sourceTransform)
    {
        blunderbussShot.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        blunderbussShot.start();
    }

    public void PlayTorretaTesla(Transform sourceTransform)
    {
        torretaTesla.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        torretaTesla.start();
    }

    public void PlayTorretaGravedad(Transform sourceTransform)
    {
        torretaGravedad.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        torretaGravedad.start();
    }

    public void PlayBowShot2(Transform sourceTransform)
    {
        BowShot2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        BowShot2.start();
    }

    public void PlayKillEnemyTutorial(Transform sourceTransform)
    {
        killEnemyTutorial.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        killEnemyTutorial.start();
    }

    public void PlayHitmarker()
    {
        hitmarker.start();
    }

    public void PlaySpawnEnemyTutorial(Transform sourceTransform)
    {
        spawnEnemyTutorial.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(sourceTransform));

        spawnEnemyTutorial.start();
    }

    /// <summary>
    /// Funcion que se llama automáticamente cuando se carga una nueva escena
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si hay música activa, la detenemos suavemente y liberamos su instancia
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // fade out suave
            currentMusic.release(); // liberamos recursos
        }

        // Determinar qué música debe sonar para esta escena
        EventReference musicToPlay = GetMusicForScene(scene.name);

        // Crear nueva instancia y reproducirla
        currentMusic = RuntimeManager.CreateInstance(musicToPlay);
        currentMusic.start();

    }

    /// <summary>
    /// Funcion que se llama cuando se carga una scena.
    /// 
    /// Determina la música apropiada según el nombre de la escena y PlayerPrefs
    /// </summary>
    private EventReference GetMusicForScene(string sceneName)
    {
        // Si estamos en el menú principal
        if (sceneName == "MainMenu")
        {
            return eventReferenceMusicMenu;
        }

        // Si estamos en una pantalla de victoria o derrota
        if (sceneName.Contains("WinScene") || sceneName.Contains("LoseScene") || sceneName.Contains("TutorialScene"))
        {
            return eventReferenceMusicMenu;
        }

        // Lista de escenas que son niveles jugables (puedes agregar más aquí)
        string[] levelScenes = { "Prehistory", "EdadMedia_Nueva", "Mapa Futuro" };

        if (System.Array.Exists(levelScenes, name => name == sceneName))
        {
            int era = PlayerPrefs.GetInt("Eras", 0); // 0: tutorial 1: prehistoria, 2: edad media, 3: futuro

            switch (era)
            {
                case 1:
                    return eventReferenceMusicPrehistoric;
                case 2:
                    return eventReferenceMusicMiddleAge;
                case 3:
                    return eventReferenceMusicFuture;
                default:
                    return eventReferenceMusicMenu;
            }
        }

        // Por defecto, usar música del menú
        return eventReferenceMusicMenu;
    }
}
