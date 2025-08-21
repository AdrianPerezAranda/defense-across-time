using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject optionsMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se presione el boton de "Controles", tanto en el main menu como en el menu de pausa
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    /// 
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }


    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Controles" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues el menu actual se desactive y que se active el menu de controles.
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        optionsMenu.SetActive(false);

        controlsMenu.SetActive(true);

        lastCoroutine = null;
    }
}
