using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(EnemyMovement),typeof(Rigidbody),typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider),typeof(EnemyAttack))]
public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("[5-15]")] public float speed;
    [SerializeField, Tooltip("[10-100]")] public float health;
    [SerializeField] private bool isBoss;
    [SerializeField] private GameObject prestigePointPrefab;

    [Header("Attacks")]
    [SerializeField] public float damage;
    [SerializeField, Tooltip("Tiene que ser mayor a la distancia de ataque")] public float viewDistance;
    [SerializeField, Tooltip("Si el enemigo es melee deberia ser 2 aprox.")] public float attackDistance;
    
    [HideInInspector] public UnityEvent onStartPathToBase;
    [HideInInspector] public UnityEvent onStartChasing;
    [HideInInspector] public UnityEvent onStartAttacking;
    [HideInInspector] public UnityEvent onParalized;
    [HideInInspector] public UnityEvent onStartDying;

    [HideInInspector] public GameObject player;
    
    [HideInInspector] public enum Estados
    {
        PathingBase,
        FollowingPlayer,
        Attacking,
        Paralized,
        Dying
    }
    [HideInInspector] public Estados estado;

    private bool hasScreamed = false;
    private float screamCooldown = 5f; // Tiempo antes de que pueda gritar de nuevo
    private float screamTimer = 0f;
    private float soundCooldown = 2f; // Tiempo antes de que pueda hacer nu sonido de nuevo
    private float soundTimer = 0f;

    private void Awake()
    {
        estado = Estados.FollowingPlayer;
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<NavMeshAgent>().speed = speed;
    }

    private void OnEnable()
    {
        onStartPathToBase.AddListener(OnStartPathingToBase);
        onStartChasing.AddListener(OnStartChasing);
        onStartAttacking.AddListener(OnStartAttacking);
        onStartDying.AddListener(OnStartDying);
        onParalized.AddListener(OnParalized);
    }

    private void OnDisable()
    {
        onStartPathToBase?.RemoveListener(OnStartPathingToBase);
        onStartChasing?.RemoveListener(OnStartChasing);
        onStartAttacking?.RemoveListener(OnStartAttacking);
        onStartDying?.RemoveListener(OnStartDying);
        onParalized?.RemoveListener(OnParalized);
    }


    private void Update()
    {
        if (estado != Estados.Attacking) CheckNewState();

        // Si el enemigo ha gritado, iniciar el temporizador para permitir otro grito después de un tiempo
        if (hasScreamed)
        {
            screamTimer -= Time.deltaTime;
            if (screamTimer <= 0f)
            {
                hasScreamed = false; // Permitir que el enemigo grite de nuevo
            }
        }

        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0f)
        {
            MusicController.instance.PlayEnemySound(transform);

            soundTimer = soundCooldown;
        }
    }

    /// <summary>
    /// Funcion que se encarga de definir el nuevo estado de el enemigo.
    /// Entra en cada frame mientras no este atacando, y al acabar de atacar
    /// </summary>
    public void CheckNewState()
    {
        //Si el campo de vision entre el enemigo y el jugador no esta bloqueado
        if (Physics.Linecast(transform.position, player.transform.position, out RaycastHit hitInfo))
        {
            //Si el enemigo no esta muerto
            if (estado != Estados.Dying)
            {
                //El jugador entra en el rango de ataque del enemigo
                if (hitInfo.collider.gameObject == player && hitInfo.distance <= attackDistance && estado != Estados.Attacking)
                {
                    onStartAttacking?.Invoke();
                }

                //El jugador entra en el rango de vision del enemigo
                else if (hitInfo.collider.gameObject == player && hitInfo.distance <= viewDistance && estado != Estados.FollowingPlayer)
                {
                    onStartChasing?.Invoke();
                }

                //El jugador sale del campo de vision del enemigo
                else if ((hitInfo.collider.gameObject != player && estado != Estados.PathingBase)  //El raycast choca con un objeto del mapa (hay un obstaculo en medio)
                    || (hitInfo.collider.gameObject == player && hitInfo.distance > viewDistance && estado != Estados.PathingBase)) // O el jugador esta fuera del rango de vision
                {
                    onStartPathToBase?.Invoke();
                }
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        //Si llega a la maquina del tiempo la base pierde una vida
        if (other.gameObject.CompareTag("Finish"))
        {
            ArrivedToBase();
        }
    }
    public void ReciveDamage(float damage)
    {
        health -= damage;

        MusicController.instance.PlayHitmarker();

        if (health <= 0)
            onStartDying?.Invoke();
    }
    private void ArrivedToBase()
    {
        GameController.Instance.DoDamageToBase();
        onStartDying?.Invoke();
    }
    private void OnStartPathingToBase()
    {
        estado = Estados.PathingBase;
    }
    private void OnStartChasing()
    {
        if (!hasScreamed)
        {
            MusicController.instance.PlayEnemyScream(transform);
            hasScreamed = true;
            screamTimer = screamCooldown; // Reinicia el temporizador
        }

        estado = Estados.FollowingPlayer;
    }
    private void OnStartAttacking()
    {
        estado = Estados.Attacking;
    }
    private void OnStartDying()
    {
        estado = Estados.Dying;
        PlayerController.instance.ReciveShopCoins(100);
        DropPrestigePoints();
    }
    private void OnParalized()
    {
        estado = Estados.Paralized;
    }
    private void DropPrestigePoints()
    {
        int prob = 5;
        if(isBoss)
        {
            prob = 100;
        }

        int rand = Random.Range(0, 100);
        if(rand <= prob) Instantiate(prestigePointPrefab, transform.position , Quaternion.identity, null);
    }
}
