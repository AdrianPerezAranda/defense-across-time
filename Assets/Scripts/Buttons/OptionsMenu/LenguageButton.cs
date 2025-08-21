using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LenguageButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject lenguageMenu;
    [SerializeField] GameObject optionsMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se presione el boton de "Idioma", tanto en el main menu como en el menu de pausa
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Idioma" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues hace que se desactive el menu actual y se active el menu de idioma.
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        optionsMenu.SetActive(false);

        lenguageMenu.SetActive(true);

        lastCoroutine = null;
    }
}
