using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnPauseButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject pauseMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Volver" del menu principal de pausa.
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>

    public void OnPointerUp(PointerEventData eventData)
    {
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());
    }

    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Volver" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues hace que desaparezca el menu de pausa y se vuelva a jugar..
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        MusicController.instance.PlayButtonClick();

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

        pauseMenu.SetActive(false);

        Time.timeScale = 1;

        lastCoroutine = null;
    }
}
