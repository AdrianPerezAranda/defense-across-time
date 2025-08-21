using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region SINGLETONE

    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            inputMovement.Enable();
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] Transform parentBrazos;
    [SerializeField] GameObject weapon_0;
    [SerializeField] GameObject weapon_1;
    [SerializeField] GameObject weapon_2;

    public InputActionAsset inputMovement;

    const float MAX_HEALTH = 100;
    float health;
    public int shopCoins, prestigePoints;

    bool isDead = false;

    Coroutine lastHealingCoroutine;

    #region EVENTOS
    private void OnEnable()
    {
        SetPlayerPosAtRespawn();
        if (GameController.Instance)
        {
            GameController.Instance.startEditing.AddListener(ChangeToHammerHand);

            GameController.Instance.startPlaying.AddListener(ChangeDefaultHand);
        }
        else
        {
            Debug.LogError("No se ha encontrado el Game Controller");
        }
    }


    private void OnDisable()
    {
        if (GameController.Instance)
        {
            GameController.Instance.startEditing.RemoveListener(ChangeToHammerHand);
            GameController.Instance.startPlaying.RemoveListener(ChangeDefaultHand);
        }
    }
    #endregion
    private void Start()
    {
        GameController.Instance.playerInstance = this;
        if(SceneManager.GetActiveScene().name == "MainMenu") gameObject.SetActive(false);
        health = MAX_HEALTH;
        //shopCoins = prestigePoints = 0;
        GetComponent<CamaraController>().enabled = false;
        GetComponent<CamaraController>().enabled = true;
        isDead = false;
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            ReciveDamage(1000);
        }
        if (inputMovement.FindActionMap("Player").FindAction("EmpezarOleada").ReadValue<float>() != 0 && GameController.Instance.puedeEmpezarOleada)
        {
            GameController.Instance.EmpezarSiguienteOlada();
        }
    }
    private void SetPlayerPosAtRespawn()
    {
        GetComponent<PlayerMovimiento>().SetPlayerVelocityToZero();
        GetComponent<CharacterController>().enabled = false;
        if (GameObject.FindGameObjectWithTag("Finish"))
        {
            transform.position = GameObject.FindGameObjectWithTag("Finish").transform.GetChild(1).position;
            transform.LookAt(GameObject.FindGameObjectWithTag("Finish").transform);
        }
        GetComponent<CharacterController>().enabled = true;
    }

    #region VIDA
    public void Heal(int healAmount)
    {
        health += healAmount;
        if(health > MAX_HEALTH) health = MAX_HEALTH;

        //Actualizar la vida en la UI
        HubControlllerGame.instance.UpdateVidaPersonaje(health);
    }
    public void ReciveDamage(float damage)
    {
        if (!isDead)
        {
            MusicController.instance.PlayPlayerHurt();

            health -= damage;
            HubControlllerGame.instance.UpdateVidaPersonaje(health);

            //Aciva el Vignetting
            GetComponent<CamaraController>().cameraPlayer.GetComponent<CinemachineVolumeSettings>().enabled = true;

            //Si se ha muerto
            if (health <= 0)
            {
                if (lastHealingCoroutine != null) StopCoroutine(lastHealingCoroutine);
                health = 0;
                PlayerDie();
            }
            //Si no se ha muerto que empiece la corutina de curarse
            else
            {
                if(lastHealingCoroutine != null) StopCoroutine(lastHealingCoroutine);
                lastHealingCoroutine = StartCoroutine(StartCountHeal());
            }
        }
    }

    /// <summary>
    /// Funcion que se llama cuando el jugador muere
    /// </summary>
    private void PlayerDie()
    {
        print("Te has muerto");
        isDead = true;
        StartCoroutine(RespawnPlayerAfterDead());
    }

    IEnumerator RespawnPlayerAfterDead()
    {
        //Se desactiva la camara cuando mueres
        GetComponent<CamaraController>().cameraPlayer.enabled = false;

        //Se quita el inputMovement para que no se pueda mover 
        inputMovement.Disable();
        yield return new WaitForSeconds(3f);
        inputMovement.Enable();

        //Se vuelve a poner al jugador en el punto de spawn
        SetPlayerPosAtRespawn();
        isDead = false;
        GetComponent<CamaraController>().cameraPlayer.enabled = true;

        //Se cura a tope y se va el vignetting
        Heal((int)MAX_HEALTH); 
        GetComponent<CamaraController>().cameraPlayer.GetComponent<CinemachineVolumeSettings>().enabled = false;
    }

    IEnumerator StartCountHeal()
    {
        yield return new WaitForSeconds(3f);
        
        //Desactiva el Vignetting
        GetComponent<CamaraController>().cameraPlayer.GetComponent<CinemachineVolumeSettings>().enabled = false;

        while(health < 100)
        {

            //Cura 1 de vida cada .1s
            Heal(1);
            yield return new WaitForSeconds(.1f);

        }
    }
    #endregion

    #region COINS
    public void ReciveShopCoins(int amount)
    {
        HubControlllerGame.instance.ActualizarDinero(amount);
    }
    public void RecivePrestigeCoins(int amount)
    {
        prestigePoints += amount;
    }
    public void SpendShopCoins(int amount)
    {
        if (shopCoins - amount < 0) print("NO TIENES MONEDAS SUFICIENTES");
        else shopCoins -= amount;
    }
    public void SpendPrestigeCoins(int amount)
    {
        if (prestigePoints - amount < 0) print("NO TIENES PUNTOS SUFICIENTES");
        else prestigePoints -= amount;
    }
    #endregion

    #region Manos
    private void ChangeToHammerHand()
    {
        parentBrazos.GetChild(0).gameObject.SetActive(true);
        parentBrazos.GetChild(1).gameObject.SetActive(false);
    }

    private void ChangeDefaultHand()
    {
        parentBrazos.GetChild(0).gameObject.SetActive(false);
        parentBrazos.GetChild(1).gameObject.SetActive(true);
    }

    private void ChangeRangeMeleeWeapon()
    {
        if (parentBrazos.GetChild(1).gameObject.activeInHierarchy)
        {
            parentBrazos.GetChild(1).gameObject.SetActive(false);
            parentBrazos.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            parentBrazos.GetChild(1).gameObject.SetActive(false);
            parentBrazos.GetChild(1).gameObject.SetActive(true);
        }

    }

    public void AvanzarArma()
    {
        if( SceneManager.GetActiveScene().name == "EdadMedia_Nueva")
        {
            if(weapon_0 != null) Destroy(weapon_0);
        }
        else if(SceneManager.GetActiveScene().name == "Future")
        {
            if(weapon_0 != null) Destroy(weapon_0);
            if(weapon_1 != null) Destroy(weapon_1);
        }
    }
    #endregion
}
