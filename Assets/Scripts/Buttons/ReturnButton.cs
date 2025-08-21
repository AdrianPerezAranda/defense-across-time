using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject previousMenu;
    [SerializeField] GameObject actualMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Volver", tanto en los submenus del main menu
    /// como en los de el menu de pausa, no el boton "Volver" en el menu principal de pausa
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se preione el boton de "Volver" y lastCoroutine sea null.
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

        actualMenu.SetActive(false);

        previousMenu.SetActive(true);

        lastCoroutine = null;
    }
}
