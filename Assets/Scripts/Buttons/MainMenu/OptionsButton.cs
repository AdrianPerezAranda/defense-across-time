using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }
    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Opciones", en el main menu
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Opciones" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues hace que se desactive el main menu y se active el menu de opciones..
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        animatior.SetTrigger("Normal");

        yield return new WaitForSeconds(0.12f);

        mainMenu.SetActive(false);

        optionsMenu.SetActive(true);

        lastCoroutine = null;
    }
}
