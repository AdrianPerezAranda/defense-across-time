using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour, IPointerUpHandler
{
    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se preione el boton de "Salir", en el main menu
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se preione el boton de "Salir" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues se cierra el juego.
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        Application.Quit();

        lastCoroutine = null;
    }
}
