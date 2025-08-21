using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesController : MonoBehaviour
{
    #region SINGLETONE
    public static EnemiesController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    float cdInstantiate;
    [SerializeField] Transform[] spawnPoints;

    private Dia diaActual;
    private Oleada oleadaActual;

    public int idxDiaActual;
    private int idxOleadaActual;

    int numEnemigosTotal = 0;
    int[] cantidadDeEnemigosPorTipo;

    private List<GameObject> listaEnemigos;

    private bool waveInstantiated;

    public Coroutine lastCoroutine;

    int waveCount = 0;

    private void Start()
    {
        idxDiaActual = GameController.Instance.idxDiaActual;

        listaEnemigos = new List<GameObject>();

        idxOleadaActual = 0;
        diaActual = GameController.Instance.GetGameConfig().dias[idxDiaActual];
        oleadaActual = diaActual.oleadas[idxOleadaActual];
    }

    private void OnDisable()
    {
        //Se lo paso al game controller en caso de no haber cerrado el juego
        GameController.Instance.idxDiaActual = idxDiaActual;
    }

    public void EmpezarSiguienteOleada()
    {
        //Musica de oleada
        waveCount++;
        if (waveCount % 3 == 0) MusicController.instance.PlayStartRoundBoss();
        MusicController.instance.PlayStartRound();
        waveCount %= 3;

        //Empieza la corutina para spawnear mobs
        lastCoroutine = StartCoroutine(SiguinteOleada());
    }

    /// <summary>
    /// Funcion que instancia los enemigos con un retraso entre cada uno
    /// </summary>
    /// <returns></returns>
    public IEnumerator SiguinteOleada()
    {

        waveInstantiated = false;

        CalcularCantidadEnemigos();
        bool instanciado = true;
        //Instancio tantos enemigos como tenga la oleada
        for (int i = 0; i < numEnemigosTotal;)
        {
            //Espero los segundos solo si se ha instanciado
            if (instanciado) yield return new WaitForSeconds(cdInstantiate);

            instanciado = false;

            //Genero un indice random entre la cantidad de 
            int idxEnemigo = Random.Range(0, oleadaActual.enemigos.Length);
            //Comprobar que el enemigo escogido se pueda instanciar
            if (cantidadDeEnemigosPorTipo[idxEnemigo] > 0)
            {
                instanciado = true;

                //Se instancia en uno de los spawns aleatoriamente
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject nuevoEnemigo = Instantiate(oleadaActual.enemigos[idxEnemigo], spawnPoint.position, Quaternion.identity, null);
                listaEnemigos.Add(nuevoEnemigo); //Añade el enemigo a la lista de enemigos
                cantidadDeEnemigosPorTipo[idxEnemigo]--;
                i++;

                //El cd entre spawn y espawn de enemigos es random entre .3 y 1 segundos
                cdInstantiate = Random.Range(0.3f, 1f);
            }
        }

        waveInstantiated = true;
    }

    public void CalcularSiguienteOleada()
    {
        //Calcular siguiente oleadaDia
        idxOleadaActual++;

        //Si el dia ya no tiene mas oleadas pasa al siguiente dia
        if (idxOleadaActual >= diaActual.oleadas.Length)
        {
            idxOleadaActual = 0;
            idxDiaActual++;

            try
            {
                //Intento actualizar el dia al siguiente
                diaActual = GameController.Instance.GetGameConfig().dias[idxDiaActual];
            }
            catch
            {
                //Si no hay mas dias no se actualiza el dia, se queda en el que está
                idxDiaActual--;
                diaActual = GameController.Instance.GetGameConfig().dias[idxDiaActual];
                //Se establece la ultima oleada repetitivamente
                idxOleadaActual = GameController.Instance.GetGameConfig().dias[idxOleadaActual].oleadas.Length - 1;

                GameController.Instance.PartidaGanada();
            }
        }

        //Actualizo la siguiete oleada
        oleadaActual = diaActual.oleadas[idxOleadaActual];
    }

    private void CalcularCantidadEnemigos()
    {
        numEnemigosTotal = 0;
        //Lista con la cantidad de enemigos por tipo
        cantidadDeEnemigosPorTipo = new int[oleadaActual.enemigos.Length];

        //Calcular la cantidad total de enemigos
        for (int i = 0; i < cantidadDeEnemigosPorTipo.Length; i++)
        {
            cantidadDeEnemigosPorTipo[i] = oleadaActual.cantidadEnemigos[i];
            numEnemigosTotal += oleadaActual.cantidadEnemigos[i];
        }
    }

    public void KillEnemy(GameObject enemigo)
    {
        listaEnemigos.Remove(enemigo);
        Destroy(enemigo,1.5f);
        if(listaEnemigos.Count == 0 && waveInstantiated) GameController.Instance.OleadaGanada();
    }

    public void PararOleada()
    {
        waveInstantiated = false;
        if(lastCoroutine != null) StopCoroutine(lastCoroutine);
        int count = listaEnemigos.Count;
        for(int i = 0; i < count; i++)
        {
            KillEnemy(listaEnemigos[0]);
        }
    }

    public void HaveToPassEra()
    {
        switch(diaActual.epoca)
        {
            case Dia.Epoca.Prehistoria:
                if (SceneManager.GetActiveScene().name == "TutorialScene")
                {
                    GameController.Instance.AvanzarEpoca();
                }
                break;
            case Dia.Epoca.Edad_Media:
                if (SceneManager.GetActiveScene().name == "Prehistory")
                {
                    GameController.Instance.AvanzarEpoca();
                }
                break;
            case Dia.Epoca.Futuro:
                if (SceneManager.GetActiveScene().name == "EdadMedia_Nueva")
                {
                    GameController.Instance.AvanzarEpoca();
                }
                break;
        }
    }

    [ContextMenu("KillAllEnemies")]
    public void KillAllEnemies()
    {
        int enemiesLenght = listaEnemigos.Count;
        for(int i = 0; i < enemiesLenght; i++)
        {
            KillEnemy(listaEnemigos[0]);
        }
    }
}
