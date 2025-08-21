using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject crosshair;

    // Referencia al objeto de la mano con arma (activada más adelante)
    [SerializeField] GameObject manoArma;

    // Prefab del enemigo a instanciar en el tutorial
    [SerializeField] GameObject enemy;

    // Padre donde se instancia el primer enemigo
    [SerializeField] GameObject parentEnemy1;

    // Padre donde se instancia el segundo enemigo
    [SerializeField] GameObject parentEnemy2;

    // Objeto que muestra el mensaje final del tutorial
    [SerializeField] GameObject mensajeFinTutorial;

    // Enum que define los pasos del tutorial
    [SerializeField]
    public enum PasoTutorial
    {
        MoverCamara,
        Moverse,
        Correr,
        Saltar,
        Interactuar,
        Disparar,
        Apuntar
    }

    // Clase que asocia cada paso con su mensaje UI y texto
    [System.Serializable]
    public class MensajePaso
    {
        [SerializeField] public PasoTutorial paso;
        [SerializeField] public GameObject mensajeUI; // GameObject que contiene el mensaje en pantalla
        [SerializeField] public TMP_Text textoTutorial; // Texto a mostrar para ese paso
    }

    [Header("Pasos del tutorial")]
    [SerializeField] public List<MensajePaso> pasos;

    // Índice del paso actual que se está ejecutando
    private int pasoActualIndex = 0;

    // Conjunto de pasos ya completados
    private HashSet<PasoTutorial> completados = new HashSet<PasoTutorial>();

    void Start()
    {
        ActivarPasoActual();
    }
    /// <summary>
    /// Marca que un paso fue completado si corresponde con el paso actual.
    /// </summary>
    public void AccionRealizada(PasoTutorial paso)
    {
        // Si ya se completó este paso, no hacemos nada
        if (completados.Contains(paso)) return;

        // Si el paso coincide con el paso actual, lo completamos
        if (pasos[pasoActualIndex].paso == paso)
        {
            completados.Add(paso);
            StartCoroutine(CompletarPasoConDelay(paso, 2f)); // Espera 2 segundos antes de pasar al siguiente paso
        }
    }

    /// <summary>
    /// Retorna true si el paso dado es el actual en el tutorial.
    /// </summary>
    public bool EsPasoActual(PasoTutorial paso)
    {
        return pasoActualIndex < pasos.Count && pasos[pasoActualIndex].paso == paso;
    }

    /// <summary>
    /// Activa el mensaje correspondiente al paso actual y realiza acciones específicas como instanciar enemigos.
    /// </summary>
    private void ActivarPasoActual()
    {
        // Obtiene el paso actual desde la lista
        PasoTutorial pasoActual = pasos[pasoActualIndex].paso;

        // Recorre todos los pasos y activa solo el mensaje del paso actual
        for (int i = 0; i < pasos.Count; i++)
        {
            bool activo = (i == pasoActualIndex);
            pasos[i].mensajeUI.SetActive(activo);

            // Restaura el color del texto a blanco
            if (pasos[i].textoTutorial != null)
                pasos[i].textoTutorial.color = Color.white;
        }

        // Acciones especiales para ciertos pasos
        switch (pasoActual)
        {
            case PasoTutorial.Disparar:
                // Se cambia la mano normal por la que tiene arma y se instancia el primer enemigo
                crosshair.SetActive(true);
                manoArma.SetActive(true);
                Instantiate(enemy, parentEnemy1.transform);
                MusicController.instance.PlaySpawnEnemyTutorial(transform);
                break;

            case PasoTutorial.Apuntar:
                // Se instancia un segundo enemigo
                Instantiate(enemy, parentEnemy2.transform);
                MusicController.instance.PlaySpawnEnemyTutorial(transform);
                break;
        }
    }

    /// <summary>
    /// Rutina que cambia el color del texto a verde, espera un tiempo, oculta el mensaje y pasa al siguiente paso.
    /// </summary>
    private IEnumerator CompletarPasoConDelay(PasoTutorial paso, float delay)
    {
        // Obtiene el objeto del mensaje del paso actual
        MensajePaso mp = pasos[pasoActualIndex];

        // Cambia el color del texto a verde si es válido
        if (mp != null && mp.textoTutorial != null)
        {
            mp.textoTutorial.color = Color.green;
        }

        // Espera el tiempo indicado
        yield return new WaitForSeconds(delay);

        // Oculta el mensaje del paso
        if (mp != null)
        {
            mp.mensajeUI.SetActive(false);
        }

        // Avanza al siguiente paso
        pasoActualIndex++;

        if (pasoActualIndex < pasos.Count)
        {
            ActivarPasoActual(); // Activa el nuevo paso
        }
        else
        {
            // El tutorial ha terminado
            Debug.Log("Tutorial completado.");

            // Guarda progreso
            PlayerPrefs.SetInt("TutorialCompletado", 1);
            PlayerPrefs.Save();

            // Muestra mensaje final
            mensajeFinTutorial.SetActive(true);
            yield return new WaitForSeconds(3f);

            // Carga siguiente escena
            SceneManager.LoadScene("LoadingScreen");

            // Marca que el jugador ahora está en la era 1 (¿?)
            PlayerPrefs.SetInt("Eras", 1);
        }
    }

    /// <summary>
    /// Retorna true si el paso puede realizarse actualmente (ya fue completado o es el actual).
    /// </summary>
    public bool PermitePaso(PasoTutorial paso)
    {
        // Si ya se terminó el tutorial, se permite todo
        if (pasoActualIndex >= pasos.Count) return true;

        // Se permite si es el paso actual o ya fue completado
        return paso == pasos[pasoActualIndex].paso || completados.Contains(paso);
    }
}
