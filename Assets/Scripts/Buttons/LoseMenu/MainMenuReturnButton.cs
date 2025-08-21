using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuReturnButton : MonoBehaviour, IPointerUpHandler
{
    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Menu principal", en la escena de derrota
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Menu principal" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues hace que cargue la escena del main menu..
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {
        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        Time.timeScale = 1;

        //Efecto de sonido
        MusicController.instance.PlayButtonClick();

        //Desactivo la instancia del jugador
        if(GameController.Instance.playerInstance) GameController.Instance.playerInstance.gameObject.SetActive(false);

        //Guardo las estructuras del mapa
        GameController.Instance.GetComponent<SaveableObjectsController>().SaveGame();

        //Cargo la escena del main menu
        SceneManager.LoadScene("MainMenu");

        lastCoroutine = null;
    }


}
